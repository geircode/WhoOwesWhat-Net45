using System;
using WhoOwesWhat.DataProvider.PersonEntity;
using WhoOwesWhat.DataProvider.PostEntity;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.TransactionEntity
{
    public interface ITransactionDataProviderLogic
    {
        Domain.DTO.PayerTransaction MapPayerToDomain(Entity.Payer source);
        Domain.DTO.ConsumerTransaction MapConsumerToDomain(Entity.Consumer source);
    }

    public class TransactionDataProviderLogic : ITransactionDataProviderLogic
    {

        public TransactionDataProviderLogic(
            )
        {
        }

        public Domain.DTO.PayerTransaction MapPayerToDomain(Entity.Payer source)
        {
            Guard.NotNull(() => source, source);

            return MapTransactionToDomain(new PayerTransaction(), source);
        }        
        
        public Domain.DTO.ConsumerTransaction MapConsumerToDomain(Entity.Consumer source)
        {
            Guard.NotNull(() => source, source);

            return MapTransactionToDomain(new ConsumerTransaction(), source);
        }

        private TEntity MapTransactionToDomain<TEntity>(TEntity targetDomain, Entity.Transaction source) where TEntity : Domain.DTO.Transaction
        {
            Guard.NotNull(() => source, source);

            targetDomain.PostGuid = source.Post.PostGuid;
            targetDomain.PersonGuid = source.Person.PersonGuid;
            targetDomain.Displayname = source.Person.Displayname;
            targetDomain.AmountSetManually = source.Amount;
            targetDomain.IsAmountSetManually = source.IsAmountSetManually;
            return targetDomain;
        }
    }
}
