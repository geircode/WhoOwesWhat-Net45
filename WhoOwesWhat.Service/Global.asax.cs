using System;
using ServiceStack.Common.Web;
using ServiceStack.Logging;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.Cors;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.WebHost.Endpoints.Extensions;
using WhoOwesWhat.Ninject;

namespace WhoOwesWhat.Service
{
    public class Global : System.Web.HttpApplication
    {
        public class WhoOwesWhatServiceHost : AppHostBase
        {
            //Register your web service with ServiceStack.
            public WhoOwesWhatServiceHost()
                : base("WhoOwesWhat Service", typeof (WhoOwesWhatService).Assembly)
            {
            }

            public override void Configure(Funq.Container container)
            {
                JsConfig.DateHandler = JsonDateHandler.ISO8601; 

                //Register any dependencies your services use here.
                NinjectKernel.Load(container);

                Plugins.Add(new CorsFeature());

                SetConfig(new EndpointHostConfig
                {
                    DefaultContentType = ContentType.Json
                });

                RequestFilters.Add((httpReq, httpRes, requestDto) =>
                {
                    if (httpReq.HttpMethod == "OPTIONS")
                    {
                        httpRes.AddHeader("Access-Control-Allow-Origin", "*");
                        httpRes.AddHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
                        httpRes.AddHeader("Access-Control-Allow-Headers", "X-Requested-With, Content-Type");
                        httpRes.End();
                    }
                });

                //LogManager.LogFactory = new Log4NetFactory(configureLog4Net: true);
                var log = log4net.LogManager.GetLogger(typeof (WhoOwesWhatServiceHost));
                //Handle Exceptions occurring in Services:
                ServiceExceptionHandler = (httpReq, request, exception) =>
                {
                    log.Error(exception);
                    return DtoUtils.HandleException(this, request, exception as Exception);
                };

                //Handle Unhandled Exceptions occurring outside of Services, 
                //E.g. in Request binding or filters:
                ExceptionHandler = (httpReq, httpRes, operationName, ex) =>
                {
                    var errorMessage = String.Format("Error occured while Processing Request: {0}", ex.Message);
                    var statusCode = ex.ToStatusCode();

                    //httpRes.WriteToResponse always calls .Close in it's finally statement so 
                    //if there is a problem writing to response, by now it will be closed
                    log.Error(ex);
                    if (!httpRes.IsClosed)
                    {
                        httpRes.WriteErrorToResponse(httpReq, httpReq.ResponseContentType, operationName, errorMessage,
                            ex, statusCode);
                    }
                };
            }

            

            
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            //Initialize your web service on startup.
            new WhoOwesWhatServiceHost().Init();

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var log = LogManager.GetLogger(typeof(WhoOwesWhatServiceHost));
            log.Error(e);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}