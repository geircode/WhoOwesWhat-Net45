using System;
using System.Collections.Generic;
using System.Linq;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Service.Controller;
using WhoOwesWhat.Service.DTO;

namespace WhoOwesWhat.NancyServer
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        private readonly IErrorController _errorController;
        private readonly IErrorQuery _errorQuery;

        public IndexModule(IErrorController errorController, IErrorQuery errorQuery)
        {
            _errorController = errorController;
            _errorQuery = errorQuery;
            Get["/"] = parameters =>
            {
                return View["index"];
            };

            Get["/errors"] = parameters =>
            {
                List<Error> model = _errorQuery.GetErrors().OrderByDescending(a => a.Created).ToList();
                return View["error", model];
            };

            Get["/authenticateUser"] = parameters =>
            {
                var response = new BasicResponse();
                response.isSuccess = true;

                return Response.AsJson(response);
            };

            Post["/user/new"] = parameters =>
            {
                var model = this.Bind<DynamicDictionary>();

                var response = new BasicResponse();
                response.isSuccess = true;

                return Response.AsJson(response);
            };

            Post["/user/new"] = parameters =>
            {
                var model = this.Bind<DynamicDictionary>();

                var response = new BasicResponse();
                response.isSuccess = true;
                return Response.AsJson(response);
            };
        }
    }

}