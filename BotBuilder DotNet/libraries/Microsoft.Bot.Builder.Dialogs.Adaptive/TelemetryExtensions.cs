﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Builder.Dialogs.Adaptive
{
    /// <summary>
    /// Implements extension methods for telemetry.
    /// </summary>
    public static class TelemetryExtensions
    {
        /// <summary>
        /// Register IBotTelemetryClient as default langugage generator.
        /// </summary>
        /// <param name="dialogManager">botAdapter to add services to.</param>
        /// <param name="telemetryClient">IBotTelemetryClient to use.</param>
        /// <returns>botAdapter.</returns>
        public static DialogManager UseTelemetry(this DialogManager dialogManager, IBotTelemetryClient telemetryClient)
        {
            dialogManager.InitialTurnState.Set<IBotTelemetryClient>(telemetryClient);
            dialogManager.Dialogs.TelemetryClient = telemetryClient;
            return dialogManager;
        }
    }
}
