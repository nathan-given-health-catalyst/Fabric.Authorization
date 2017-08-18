﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nancy;
using Nancy.Routing;
using Nancy.Metadata.Modules;
using Nancy.Swagger.Services;


namespace Fabric.Authorization.API.Modules
{
    public class DocsModule : NancyModule
    {
        public DocsModule(ISwaggerMetadataProvider converter) : base("/v1/docs")
        {
            Get("/", _ => GetSwaggerUrl());
            Get("/swagger.json", _ => converter.GetSwaggerJson(Context).ToJson());
        }

        private Response GetSwaggerUrl()
        {            
            return Response.AsRedirect($"{Request.Url.SiteBase}/swagger/index.html?url={Request.Url.SiteBase}/docs/swagger.json");
        }
    }
}