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
using Error = WhoOwesWhat.DataProvider.Entity.Error;
using UserCredential = WhoOwesWhat.Domain.DTO.UserCredential;

namespace WhoOwesWhat.DataProvider
{
    public class ErrorCommand : IErrorCommand
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;

        public ErrorCommand(IWhoOwesWhatContext whoOwesWhatContext)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
        }

        public void SaveError(DateTime createdTime, string message, string error)
        {
            _whoOwesWhatContext.GetErrorSqlRepository().Add(new Error() { Created = createdTime, Message = message, ErrorJson = error});

            
            _whoOwesWhatContext.SaveChanges();

            
        }        
        
    }

}
