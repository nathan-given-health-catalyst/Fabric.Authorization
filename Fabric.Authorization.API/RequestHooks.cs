﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Nancy;

namespace Fabric.Authorization.API
{
    public static class RequestHooks
    {
        private static readonly Regex VersionRegex = new Regex("\\/v\\d(.\\d)?\\/");

        public static readonly Func<NancyContext, Response> SetDefaultVersionInUrl = context =>
        {
            var url = context.Request.Url;
            var versionInUrlMatch = VersionRegex.Match(url);

            if (versionInUrlMatch.Success)
            {
                //a version exists so do nothing with url
                return null;
            }

            //modify the url, default to the first version of the api (v1)
            var originalRequest = context.Request;
            var siteBase = url.SiteBase;
            var path = url.Path;

            var version1Url = $"{siteBase}/v1{path}{url.Query}";

            var headers = originalRequest.Headers.ToDictionary(originalRequestHeader => originalRequestHeader.Key, originalRequestHeader => originalRequestHeader.Value);

            var updatedRequest = new Request(
                originalRequest.Method,
                version1Url,
                originalRequest.Body,
                headers,
                originalRequest.UserHostAddress,
                originalRequest.ClientCertificate,
                originalRequest.ProtocolVersion);

            context.Request = updatedRequest;

            return null;
        };
    }
}
