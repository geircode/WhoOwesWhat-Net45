using System;
using System.Linq;
using WhoOwesWhat.DataProvider.Entity;
using WhoOwesWhat.DataProvider.TransactionEntity;
using Post = WhoOwesWhat.Domain.DTO.Post;

namespace WhoOwesWhat.DataProvider.PostEntity
{
    public interface IPostDataProviderLogic
    {
        Post MapToDomain(DataProvider.Entity.Post source);
    }

    public class PostDataProviderLogic : IPostDataProviderLogic
    {
        private readonly ITransactionDataProviderLogic _transactionDataProviderLogic;

        public PostDataProviderLogic(
            ITransactionDataProviderLogic transactionDataProviderLogic
            )
        {
            _transactionDataProviderLogic = transactionDataProviderLogic;
        }

        public Domain.DTO.Post MapToDomain(DataProvider.Entity.Post source)
        {
            Guard.NotNull(() => source, source);

            var targetDomain = new Domain.DTO.Post();
            targetDomain.PostGuid = source.PostGuid;
            targetDomain.Description = source.Description;
            targetDomain.PurchaseDate = source.PurchaseDate;
            targetDomain.TotalCost = source.TotalCost;
            targetDomain.Iso4217CurrencyCode = source.Iso4217CurrencyCode;
            targetDomain.Version = source.Version;
            targetDomain.IsDeleted = source.IsDeleted;
            targetDomain.Comment = source.Comment;
            if (source.Group != null)
            {
                targetDomain.GroupGuid = source.Group.GroupGuid;
            }

            targetDomain.LastUpdatedByPersonGuid = source.LastUpdatedBy.PersonGuid;
            targetDomain.LastUpdated = source.LastUpdated;
            targetDomain.CreatedByPersonGuid = source.CreatedBy.PersonGuid;
            targetDomain.Created = source.Created;
            if (source.Transactions != null)
            {
                targetDomain.PayerTransactions = source.Transactions.OfType<Payer>().Select(a => _transactionDataProviderLogic.MapPayerToDomain(a)).ToList();
                targetDomain.ConsumerTransactions = source.Transactions.OfType<Consumer>().Select(a => _transactionDataProviderLogic.MapConsumerToDomain(a)).ToList();
            }

            return targetDomain;
        }
    }
}
