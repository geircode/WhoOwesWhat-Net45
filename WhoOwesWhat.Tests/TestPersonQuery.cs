using System;
using System.Collections.Generic;
using WhoOwesWhat.Domain.DTO;
using WhoOwesWhat.Service.DTO;

namespace WhoOwesWhat.Tests
{

    public interface ITestPersonQuery
    {
    }

    public class TestPersonQuery : ITestPersonQuery
    {

        public static Person MariannePersonDomain = new Person()
        {
            Displayname = "Marianne",
            PersonGuid = new Guid("8035804C-4747-4747-4747-098FA53C3416")
        };        
        
        public static Person GeirPersonDomain = new Person()
        {
            Displayname = "Geir",
            PersonGuid = new Guid("8035804C-1337-1337-1337-098FA53C3416")
        };

        public static DataProvider.Entity.Person GeirPersonEntity = new WhoOwesWhat.DataProvider.Entity.Person()
        {
            Displayname = "Geir",
            PersonGuid = new Guid("8035804C-1337-1337-1337-098FA53C3416")
        };


        public static Person VictorPersonDomain = new Person()
        {
            Displayname = "Victor",
            PersonGuid = new Guid("73e59037-adcc-86bf-473c-6015fec2b296")
        };

        public static DataProvider.Entity.Person VictorPersonEntity = new DataProvider.Entity.Person()
        {
            Displayname = "Victor",
            PersonGuid = new Guid("73e59037-adcc-86bf-473c-6015fec2b296")
        };        
        
        public static Person BeatePersonDomain = new Person()
        {
            Displayname = "Beate",
            PersonGuid = new Guid("f6aa22b2-fce3-4abf-4733-6aa6c7350e3e")
        };
    }
}
