using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Nancy;
using Nancy.ErrorHandling;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Service.Controller;
using WhoOwesWhat.Service.DTO;

namespace WhoOwesWhat.NancyServer
{
    public class LoggingErrorHandler : IStatusCodeHandler
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(LoggingErrorHandler));

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            object errorObject;
            context.Items.TryGetValue(NancyEngine.ERROR_EXCEPTION, out errorObject);
            var error = errorObject as Exception;

            _logger.Error("Unhandled error", error);
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
         return statusCode == HttpStatusCode.InternalServerError;
        }
    }
}