using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Domain.Interfaces;
using WhoOwesWhat.Service.DTO;

namespace WhoOwesWhat.Service.Controller
{
    public interface IErrorController
    {
        BasicResponse SaveError(SaveErrorRequest request);
    }

    public class ErrorController : IErrorController
    {
        private readonly IErrorCommand _errorCommand;

        public ErrorController(IErrorCommand errorCommand)
        {
            _errorCommand = errorCommand;
        }


        public BasicResponse SaveError(SaveErrorRequest request)
        {
            _errorCommand.SaveError(DateTime.Now, request.message, request.error);
            return new BasicResponse() { isSuccess = true };
        }
    }

}