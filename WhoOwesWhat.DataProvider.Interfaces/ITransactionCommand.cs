using System;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.Interfaces
{
    public interface ITransactionCommand
    {
        void DeleteTransactions(Guid postGuid);

        void SavePayerTransaction(Domain.DTO.PayerTransaction transactionDto);
        void SaveConsumerTransaction(Domain.DTO.ConsumerTransaction transactionDto);
    }
}