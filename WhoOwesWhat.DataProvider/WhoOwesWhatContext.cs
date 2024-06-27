using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using log4net;
using WhoOwesWhat.DataProvider.Entity;

namespace WhoOwesWhat.DataProvider
{


    public interface IWhoOwesWhatContext : IDisposable, IObjectContextAdapter
    {
        ISqlRepository<UserCredential> GetUserCredentialSqlRepository();
        ISqlRepository<Person> GetPersonSqlRepository();
        ISqlRepository<Friend> GetFriendSqlRepository();
        ISqlRepository<Friendrequest> GetFriendrequestSqlRepository();
        ISqlRepository<Error> GetErrorSqlRepository();
        ISqlRepository<Post> GetPostSqlRepository();
        ISqlRepository<Group> GetGroupSqlRepository();
        ISqlRepository<Transaction> GetTransactionSqlRepository();
        int SaveChanges();
        void ResetDatabase();
        void Sandbox();
        DbEntityEntry Entry(object entity);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        void LoadProperty<TEntity, TProperty>(TEntity friendDb,
            Expression<Func<TEntity, TProperty>> navigationProperty) where TEntity : class where TProperty : class;

        void LoadCollection<TEntity, TElement>(TEntity friendDb,
            Expression<Func<TEntity, ICollection<TElement>>> navigationProperty)
            where TEntity : class
            where TElement : class;

        Database GetDatabase();
    }

    public class WhoOwesWhatContext : DbContext, IWhoOwesWhatContext
    {
        // ReSharper disable UnusedMember.Local
        //http://stackoverflow.com/questions/14695163/cant-find-system-data-entity-sqlserver-sqlproviderservices
        private static Type _hack = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        // ReSharper restore UnusedMember.Local

        public WhoOwesWhatContext()
            : base("DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WhoOwesWhatContext, MigrationConfigurations>());

        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<UserCredential> UserCredentials { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Friendrequest> Friendrequests { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

                //modelBuilder.Entity<Friend>().HasRequired(p => p.Person)
                //    .WithMany(b => b.IsAFriend)
                //    .HasForeignKey(a => a.PersonId).WillCascadeOnDelete(false);
        }

        public void Sandbox()
        {
            //var person = Persons.Single(a => a.PersonId == 2);
            //Persons.Remove(person);
            //SaveChanges();

           var friend = Friends.First(a => a.FriendId == 1);

            this.Entry(friend).Reference(p => p.Owner).Load(); 
            //this.Entry(friend).Reference(p => p.Person).Load(); 

        }

        public void LoadProperty<TEntity, TProperty>(TEntity friendDb,
            Expression<Func<TEntity, TProperty>> navigationProperty) where TEntity : class where TProperty : class
        {
            Entry(friendDb).Reference(navigationProperty).Load();
        }

        public void LoadCollection<TEntity, TElement>(TEntity friendDb,
            Expression<Func<TEntity, ICollection<TElement>>> navigationProperty)
            where TEntity : class
            where TElement : class
        {
            Entry(friendDb).Collection<TElement>(navigationProperty).Load();
        }

        public Database GetDatabase()
        {
            return Database;
        }

        public void ResetDatabase()
        {
            Database.Delete();
        }


        public ISqlRepository<UserCredential> GetUserCredentialSqlRepository()
        {
            return new SqlRepository<UserCredential>(
                this,
                UserCredentials);
        }

        public ISqlRepository<Person> GetPersonSqlRepository()
        {
            return new SqlRepository<Person>(
                this,
                Persons);
        }        
        
        public ISqlRepository<Friend> GetFriendSqlRepository()
        {
            return new SqlRepository<Friend>(
                this,
                Friends);
        }

        public ISqlRepository<Friendrequest> GetFriendrequestSqlRepository()
        {
            return new SqlRepository<Friendrequest>(
                this,
                Friendrequests);
        }

        public ISqlRepository<Error> GetErrorSqlRepository()
        {
            return new SqlRepository<Error>(
                this,
                Errors);
        }


        public ISqlRepository<Post> GetPostSqlRepository()
        {
            return new SqlRepository<Post>(
                this,
                Posts);
        }        
        
        public ISqlRepository<Group> GetGroupSqlRepository()
        {
            return new SqlRepository<Group>(
                this,
                Groups);
        }        
        
        public ISqlRepository<Transaction> GetTransactionSqlRepository()
        {
            return new SqlRepository<Transaction>(
                this,
                Transactions);
        }

        //public DbSet<Handleliste> Feedlist { get; set; }
        //public DbSet<SharedHandleliste> SharedFeedList { get; set; }
        //public DbSet<Commodity> Commodities { get; set; }
        //public DbSet<Error> Errors { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //modelBuilder.Entity<UserCredential>()
        //    //    .HasRequired(c => c.SharedHandleliste)
        //    //    .WithMany()
        //    //    .WillCascadeOnDelete(false);
        //}

        //public DbSet<Handleliste> Feedlist    
        //{
        //    get
        //    {
        //        return Set<Handleliste>();
        //    }
        //}

        //public DbSet<Commodity> Commodities
        //{
        //    get
        //    {
        //        return Set<Commodity>();
        //    }
        //}


        //public DbSet<TestMigration> TestMigrations
        //{
        //    get
        //    {
        //        return Set<TestMigration>();
        //    }
        //}

    }


}
