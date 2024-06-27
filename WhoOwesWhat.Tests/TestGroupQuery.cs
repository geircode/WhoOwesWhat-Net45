using System;

namespace WhoOwesWhat.Tests
{

    public interface ITestGroupQuery
    {
    }

    public class TestGroupQuery : ITestGroupQuery
    {
        public static DataProvider.Entity.Group HawaiiGroupEntity = new DataProvider.Entity.Group()
        {
            Name = "Hawaii2010",
            GroupGuid = new Guid("b2cb2131-9053-e4b0-4a12-bca0ba5a95e1")
        };

        public static Domain.DTO.Group HawaiiGroupsDomain = new Domain.DTO.Group()
        {
            Name = "Hawaii2010",
            GroupGuid = new Guid("b2cb2131-9053-e4b0-4a12-bca0ba5a95e1")
        };

    }
}
