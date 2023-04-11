﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Web.Http;

namespace Microsoft.Bot.Builder.TestBot.WebApi
{
#pragma warning disable SA1649 // File name should match first type name
    public class WebApiApplication : System.Web.HttpApplication
#pragma warning restore SA1649 // File name should match first type name
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
