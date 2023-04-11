﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Streaming.Payloads;
using Microsoft.Bot.Streaming.PayloadTransport;

namespace Microsoft.Bot.Streaming.Transport.WebSockets
{
    /// <summary>
    /// A server for use with the Bot Framework Protocol V3 with Streaming Extensions and an underlying WebSocket transport.
    /// </summary>
    public class WebSocketServer : IStreamingTransportServer, IDisposable
    {
        private readonly RequestHandler _requestHandler;
        private readonly RequestManager _requestManager;
        private readonly ProtocolAdapter _protocolAdapter;
        private readonly IPayloadSender _sender;
        private readonly IPayloadReceiver _receiver;
        private readonly WebSocketTransport _webSocketTransport;
        private TaskCompletionSource<string> _closedSignal;
        private bool _isDisconnecting = false;

        // To detect redundant calls to dispose
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketServer"/> class.
        /// Throws <see cref="ArgumentNullException"/> on null arguments.
        /// </summary>
        /// <param name="socket">The <see cref="WebSocket"/> of the underlying connection for this server to be built on top of.</param>
        /// <param name="requestHandler">A <see cref="RequestHandler"/> to process incoming messages received by this server.</param>
        public WebSocketServer(WebSocket socket, RequestHandler requestHandler)
        {
            if (socket == null)
            {
                throw new ArgumentNullException(nameof(socket));
            }

            _webSocketTransport = new WebSocketTransport(socket);
            _requestHandler = requestHandler ?? throw new ArgumentNullException(nameof(requestHandler));
            _requestManager = new RequestManager();
            _sender = new PayloadSender();
            _sender.Disconnected += OnConnectionDisconnected;
            _receiver = new PayloadReceiver();
            _receiver.Disconnected += OnConnectionDisconnected;
            _protocolAdapter = new ProtocolAdapter(_requestHandler, _requestManager, _sender, _receiver);
        }

        /// <summary>
        /// An event to be fired when the underlying transport is disconnected. Any application communicating with this server should subscribe to this event.
        /// </summary>
        public event DisconnectedEventHandler Disconnected;

        /// <summary>
        /// Gets a value indicating whether or not this server is currently connected.
        /// </summary>
        /// <returns>
        /// True if this server is connected and ready to send and receive messages, otherwise false.
        /// </returns>
        /// <value>
        /// A boolean value indicating whether or not this server is currently connected.
        /// </value>
        public bool IsConnected => _sender.IsConnected && _receiver.IsConnected;

        /// <summary>
        /// Used to establish the connection used by this server and begin listening for incoming messages.
        /// </summary>
        /// <returns>A <see cref="Task"/> to handle the server listen operation. This task will not resolve as long as the server is running.</returns>
        public Task StartAsync()
        {
            _closedSignal = new TaskCompletionSource<string>();
            var task = _closedSignal.Task;
            _sender.Connect(_webSocketTransport);
            _receiver.Connect(_webSocketTransport);
            return task;
        }

        /// <summary>
        /// Task used to send data over this server connection.
        /// Throws <see cref="InvalidOperationException"/> if called when server is not connected.
        /// Throws <see cref="ArgumentNullException"/> if request is null.
        /// </summary>
        /// <param name="request">The <see cref="StreamingRequest"/> to send.</param>
        /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> used to signal this operation should be cancelled.</param>
        /// <returns>A <see cref="Task"/> of type <see cref="ReceiveResponse"/> handling the send operation.</returns>
        public Task<ReceiveResponse> SendAsync(StreamingRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (!_sender.IsConnected || !_receiver.IsConnected)
            {
                throw new InvalidOperationException("The server is not connected.");
            }

            return _protocolAdapter.SendRequestAsync(request, cancellationToken);
        }

        /// <summary>
        /// Disconnects the WebSocketServer.
        /// </summary>
        public void Disconnect()
        {
            _sender?.Disconnect();
            _receiver?.Disconnect();
        }

        /// <summary>
        /// Disposes the object and releases any related objects owned by the class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes objected used by the class.
        /// </summary>
        /// <param name="disposing">A Boolean that indicates whether the method call comes from a Dispose method (its value is true) or from a finalizer (its value is false).</param>
        /// <remarks>
        /// The disposing parameter should be false when called from a finalizer, and true when called from the IDisposable.Dispose method.
        /// In other words, it is true when deterministically called and false when non-deterministically called.
        /// </remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                Disconnect();

                // Dispose managed objects owned by the class here.
                if (_webSocketTransport != null)
                {
                    _webSocketTransport.Dispose();
                }

                if (_sender is IDisposable disposableSender)
                {
                    disposableSender?.Dispose();
                }

                if (_receiver is IDisposable disposableReceiver)
                {
                    disposableReceiver?.Dispose();
                }
            }

            _disposed = true;
        }

        private void OnConnectionDisconnected(object sender, EventArgs e)
        {
            if (!_isDisconnecting)
            {
                _isDisconnecting = true;

                if (_closedSignal != null)
                {
                    _closedSignal.SetResult("close");
                    _closedSignal = null;
                }

                if (sender == _sender)
                {
                    _receiver.Disconnect();
                }

                if (sender == _receiver)
                {
                    _sender.Disconnect();
                }

                Disconnected?.Invoke(this, DisconnectedEventArgs.Empty);

                _isDisconnecting = false;
            }
        }
    }
}
