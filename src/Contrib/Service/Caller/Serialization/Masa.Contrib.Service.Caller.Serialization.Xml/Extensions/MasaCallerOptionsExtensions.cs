﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

// ReSharper disable once CheckNamespace

namespace Masa.BuildingBlocks.Service.Caller;

public static class MasaCallerOptionsExtensions
{
    /// <summary>
    /// Set the request handler and response handler for the specified Caller
    /// </summary>
    /// <param name="masaCallerOptions"></param>
    /// <returns></returns>
    public static MasaCallerClient UseXml(this MasaCallerClient masaCallerOptions)
    {
        masaCallerOptions.RequestMessageFactory = _ => new XmlRequestMessage();
        masaCallerOptions.ResponseMessageFactory = serviceProvider =>
        {
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            return new XmlResponseMessage(loggerFactory);
        };
        return masaCallerOptions;
    }
}