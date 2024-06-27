using System;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.Interfaces
{
    public interface IErrorCommand
    {
        void SaveError(DateTime createdTime, string message, string error);
    }
}