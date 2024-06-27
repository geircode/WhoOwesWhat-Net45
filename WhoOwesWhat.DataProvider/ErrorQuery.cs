using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using WhoOwesWhat.DataProvider.Entity;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.Domain.DTO;
using Error = WhoOwesWhat.Domain.DTO.Error;
using UserCredential = WhoOwesWhat.Domain.DTO.UserCredential;

namespace WhoOwesWhat.DataProvider
{
    public class ErrorQuery : IErrorQuery
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;

        public ErrorQuery(IWhoOwesWhatContext whoOwesWhatContext)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
        }

        public List<Error> GetErrors()
        {
            var errors =_whoOwesWhatContext.GetErrorSqlRepository().GetAllAsList();

            return errors.Select(a => new Error() {Created = a.Created, Message = a.Message, ErrorJson = a.ErrorJson}).ToList();
        }
    }

}
