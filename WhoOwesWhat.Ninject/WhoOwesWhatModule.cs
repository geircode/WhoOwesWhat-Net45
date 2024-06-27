//using Microsoft.Web.Infrastructure.DynamicModuleHelper;

using System;
using System.Collections.Generic;
using System.Reflection;
using Funq;
using log4net;
using log4net.Config;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using ServiceStack.Configuration;
using WhoOwesWhat.DataProvider;
using WhoOwesWhat.DataProvider.Debug;
using WhoOwesWhat.DataProvider.FriendEntity;
using WhoOwesWhat.DataProvider.FriendrequestEntity;
using WhoOwesWhat.DataProvider.GroupEntity;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.DataProvider.PersonEntity;
using WhoOwesWhat.DataProvider.PostEntity;
using WhoOwesWhat.DataProvider.TransactionEntity;
using WhoOwesWhat.DataProvider.UserCredentialEntity;
using WhoOwesWhat.Domain;
using WhoOwesWhat.Domain.Friend;
using WhoOwesWhat.Domain.Friendrequest;
using WhoOwesWhat.Domain.Group;
using WhoOwesWhat.Domain.Interfaces;
using WhoOwesWhat.Domain.Post;
using WhoOwesWhat.Service.Controller;
using Ninject.Extensions.NamedScope;

namespace WhoOwesWhat.Ninject
{
    public class WhoOwesWhatModule : NinjectModule
    {
        public override void Load()
        {

            const string scopeName = "WhoOwesWhat";

            //Bind<IWhoOwesWhatContext>().To<WhoOwesWhatContext>();
            Bind<IWhoOwesWhatContext>().To<WhoOwesWhatContext>().InCallScope();
            //Bind<IWhoOwesWhatContext>().To<WhoOwesWhatContext>().InRequestScope();

            Bind<IUserCredentialContext>().To<UserCredentialContext>().InRequestScope();
            Bind<IPersonContext>().To<PersonContext>().InRequestScope();
            Bind<IFriendContext>().To<FriendContext>().InRequestScope();
            Bind<IFriendrequestContext>().To<FriendrequestContext>().InRequestScope();
            Bind<IPostContext>().To<PostContext>().InRequestScope();
            Bind<IGroupContext>().To<GroupContext>().InRequestScope();
            Bind<ITransactionContext>().To<TransactionContext>().InRequestScope();

            Bind<IUserController>().To<UserController>().InRequestScope();
            Bind<ISyncController>().To<SyncController>().InRequestScope();
            Bind<IErrorController>().To<ErrorController>().InRequestScope();
            Bind<IFriendrequestController>().To<FriendrequestController>().InRequestScope();

            Bind<IUserRepository>().To<UserRepository>().InRequestScope();
            Bind<IUserRepositoryLogic>().To<UserRepositoryLogic>().InRequestScope();
            Bind<IPersonRepositoryLogic>().To<PersonRepositoryLogic>().InRequestScope();

            Bind<IUserCredentialQuery>().To<UserCredentialQuery>().InRequestScope();
            Bind<IUserCredentialCommand>().To<UserCredentialCommand>().InRequestScope();
            Bind<IUserCredentialDataProviderLogic>().To<UserCredentialDataProviderLogic>().InRequestScope();

            Bind<IPersonRepository>().To<PersonRepository>().InRequestScope();
            Bind<IPersonQuery>().To<PersonQuery>().InRequestScope();
            Bind<IPersonCommand>().To<PersonCommand>().InRequestScope();
            Bind<IPersonDataProviderLogic>().To<PersonDataProviderLogic>().InRequestScope();

            Bind<IFriendRepository>().To<FriendRepository>().InRequestScope();
            Bind<IFriendRepositoryLogic>().To<FriendRepositoryLogic>().InRequestScope();
            Bind<IFriendQuery>().To<FriendQuery>().InRequestScope();
            Bind<IFriendCommand>().To<FriendCommand>().InRequestScope();
            Bind<IDeleteFriendCommand>().To<DeleteFriendCommand>().InRequestScope();

            Bind<IFriendrequestRepository>().To<FriendrequestRepository>().InRequestScope();
            Bind<IFriendrequestQuery>().To<FriendrequestQuery>().InRequestScope();
            Bind<IFriendrequestCommand>().To<FriendrequestCommand>().InRequestScope();
            Bind<IAcceptFriendrequestLogic>().To<AcceptFriendrequestLogic>().InRequestScope();

            Bind<IPostQuery>().To<PostQuery>().InRequestScope();
            Bind<IPostCommand>().To<PostCommand>().InRequestScope();
            Bind<IPostDataProviderLogic>().To<PostDataProviderLogic>().InRequestScope();

            Bind<ISyncPostQuery>().To<SyncPostQuery>().InRequestScope();
            Bind<ISyncPostsCommand>().To<SyncPostsCommand>().InRequestScope();
            Bind<ISyncPostCommand>().To<SyncPostCommand>().InRequestScope();
            Bind<ISyncControllerLogic>().To<SyncControllerLogic>().InRequestScope();

            Bind<ISyncGroupQuery>().To<SyncGroupQuery>().InRequestScope();
            Bind<ISyncGroupsCommand>().To<SyncGroupsCommand>().InRequestScope();
            Bind<ISyncGroupCommand>().To<SyncGroupCommand>().InRequestScope();

            Bind<IGroupQuery>().To<GroupQuery>().InRequestScope();
            Bind<IGroupCommand>().To<GroupCommand>().InRequestScope();
            Bind<IGroupDataProviderLogic>().To<GroupDataProviderLogic>().InRequestScope();

            Bind<IErrorCommand>().To<ErrorCommand>().InRequestScope();

            Bind<ITransactionQuery>().To<TransactionQuery>().InRequestScope();
            Bind<ITransactionCommand>().To<TransactionCommand>().InRequestScope();
            Bind<ITransactionDataProviderLogic>().To<TransactionDataProviderLogic>().InRequestScope();
            Bind<ITransactionQueryHelper>().To<TransactionQueryHelper>().InRequestScope();
            Bind<IFriendDataProviderLogic>().To<FriendDataProviderLogic>().InRequestScope();

            Bind<IHashUtils>().To<HashUtils>().InRequestScope();

            Bind<IDataProviderDebug>().To<DataProviderDebug>().InRequestScope();
            
            Bind<ILog>().ToMethod(context => LogManager.GetLogger(context.Request.Target.Member.ReflectedType));

        }
    }

    // TODO: Fungerer ikke på Entity Framework
    //public static class FunqModule
    //{
    //    public static void Load(Container container)
    //    {
    //        container.RegisterAutoWiredAs<WhatToBuyContext, WhatToBuyContext>().ReusedWithin(ReuseScope.Request);
    //        container.RegisterAutoWiredAs<HandlelisteDataRepository, IHandlelisteDataRepository>().ReusedWithin(ReuseScope.Request);
    //        container.RegisterAutoWiredAs<RequestResponseRepository, IRequestResponseRepository>().ReusedWithin(ReuseScope.Request);
    //    }
    //}

    public static class NinjectKernel
    {
        private static Container Container;

        public static void Load(Container container)
        {
            var kernel = CreateKernel();
            container.Adapter = new NinjectIocAdapter(kernel);
            Container = container;
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                var config = XmlConfigurator.Configure();

                var modules = new List<INinjectModule>
                {
                    new WhoOwesWhatModule()
                };
                kernel.Load(modules);

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        public static IKernel UpdateKernel(IKernel kernel)
        {
            try
            {
                var config = XmlConfigurator.Configure();

                var modules = new List<INinjectModule>
                {
                    new WhoOwesWhatModule()
                };
                kernel.Load(modules);

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        // TODO: lag en kernel for hver request
        public static void Refresh()
        {
            // TODO: possible memoryleak
            var kernel = CreateKernel();
            Container.Adapter = new NinjectIocAdapter(kernel);
        }
    }

    public class NinjectIocAdapter : IContainerAdapter
    {
        private readonly IKernel kernel;

        public NinjectIocAdapter(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public T Resolve<T>()
        {
            try
            {
                return this.kernel.Get<T>();
            }
            catch (Exception ex)
            {
                var log = LogManager.GetLogger(typeof (NinjectIocAdapter));
                //log.Error(ex);
                throw new NinjectIocAdapterException("Error resolving dependencies: " + ex.Message, ex);
            }
           
        }

        public T TryResolve<T>()
        {
            return this.kernel.TryGet<T>();
        }


        public class NinjectIocAdapterException : Exception
        {
            public NinjectIocAdapterException(string message)
                : base(message)
            {
            }

            public NinjectIocAdapterException(string message, Exception exception)
                : base(message, exception)
            {
            }
        }
    }
}
