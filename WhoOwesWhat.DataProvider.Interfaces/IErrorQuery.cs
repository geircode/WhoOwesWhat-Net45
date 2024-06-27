using System;
using System.Collections.Generic;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.Interfaces
{
    public interface IErrorQuery
    {
        List<Error> GetErrors();
    }
}