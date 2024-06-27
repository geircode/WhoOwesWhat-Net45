using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Linq;
using WhoOwesWhat.DataProvider.Entity;

namespace WhoOwesWhat.DataProvider
{
    public class MigrationConfigurations : DbMigrationsConfiguration<WhoOwesWhatContext>
    {
        public MigrationConfigurations()
        {
            // NICE! Sletter man et felt så forsvinner feltet!
            this.AutomaticMigrationDataLossAllowed = true;
            this.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WhoOwesWhatContext context)
        {
          
            base.Seed(context);

            #if (DEBUG)
            {
                if (!context.Persons.Any())
                {

                    Person asd = new Person()
                    {
                        PersonGuid = new Guid("17EC004E-8DBD-1337-1337-61AF34D6F98E"),
                        Displayname = "asd"
                    };


                    Person geir = new Person()
                    {
                        PersonGuid = new Guid("3E10FE42-A145-4753-9EFF-3A4E7AD6C642"),
                        Displayname = "Geir"
                    };
                    //Person marianne = new Person()
                    //{
                    //    PersonGuid = new Guid("4DD544BB-87CE-4970-A3C6-30EF5F81BE30"),
                    //    Displayname = "Marianne"
                    //};                    
                    //Person beate = new Person()
                    //{
                    //    PersonGuid = new Guid("674E4EC8-38C6-4118-820A-D2A3ED4DD354"),
                    //    Displayname = "Beate"
                    //};

                    asd = context.Persons.Add(asd);
                    geir = context.Persons.Add(geir);
                    //marianne = context.Persons.Add(marianne);
                    //beate = context.Persons.Add(beate);

                    context.Friends.Add(new Friend()
                    {
                        Owner = geir,
                        Person = asd
                    });           
                         
                    context.Friends.Add(new Friend()
                    {
                        Owner = asd,
                        Person = geir
                    });                


                    //Friend geirsFriendMarianne = new Friend()
                    //{
                    //    Owner = geir,
                    //    Person = marianne
                    //};                  

                    //Friend geirsFriendBøtta = new Friend()
                    //{
                    //    Owner = geir,
                    //    Person = beate
                    //};                

                    //Friend mariannesFriendBøtta = new Friend()
                    //{
                    //    Owner = marianne,
                    //    Person = beate
                    //};

                    //Friend asdsFriendBøtta = new Friend()
                    //{
                    //    Owner = asd,
                    //    Person = beate
                    //};             

                    //Friend asdsFriendGeir = new Friend()
                    //{
                    //    Owner = asd,
                    //    Person = geir
                    //};

                    //context.Friends.Add(geirsFriendMarianne);
                    //context.Friends.Add(geirsFriendBøtta);
                    //context.Friends.Add(mariannesFriendBøtta);
                    //context.Friends.Add(asdsFriendBøtta);
                    //context.Friends.Add(asdsFriendGeir);

                    UserCredential asdCredential = new UserCredential()
                    {
                        Person = asd,
                        Email = "asd",
                        PasswordHash = "7815696ECBF1C96E6894B779456D330E", //asd
                        Username = "asd"
                    };

                    context.UserCredentials.Add(asdCredential);
                    context.UserCredentials.Add(new UserCredential()
                    {
                        Person = geir,
                        Email = "geir",
                        PasswordHash = "7815696ECBF1C96E6894B779456D330E", //asd
                        Username = "geir"
                    });

                    //context.UserCredentials.Add(new UserCredential()
                    //{
                    //    Person = beate,
                    //    Email = "beate",
                    //    PasswordHash = "7815696ECBF1C96E6894B779456D330E", //asd
                    //    Username = "beate"
                    //});

                    //context.UserCredentials.Add(new UserCredential()
                    //{
                    //    Person = marianne,
                    //    Email = "marianne",
                    //    PasswordHash = "7815696ECBF1C96E6894B779456D330E", //asd
                    //    Username = "marianne"
                    //});

                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        // TODO Log Exception et eller annet sted!
                        throw;
                    }

                }
               
            } 
#endif

            //var lister = context.Feedlist.ToList();
        }
    }
}