namespace WhoOwesWhat.DataProvider.PersonEntity
{
    public interface IPersonDataProviderLogic
    {
        Domain.DTO.Person MapToDomain(DataProvider.Entity.Person source);
    }

    public class PersonDataProviderLogic : IPersonDataProviderLogic
    {

        public Domain.DTO.Person MapToDomain(DataProvider.Entity.Person source)
        {
            Guard.NotNull(() => source, source);

            var targetDomain = new Domain.DTO.Person();
            targetDomain.PersonGuid = source.PersonGuid;
            targetDomain.Displayname = source.Displayname;
            targetDomain.Mobile = source.Mobile;
            targetDomain.IsDeleted = source.IsDeleted;
            return targetDomain;
        }
    }
}
