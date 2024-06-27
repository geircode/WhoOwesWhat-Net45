using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoOwesWhat.DataProvider.Entity;
using WhoOwesWhat.DataProvider.Interfaces;
using WhoOwesWhat.DataProvider.PersonEntity;
using WhoOwesWhat.DataProvider.PostEntity;
using WhoOwesWhat.Domain.DTO;

namespace WhoOwesWhat.DataProvider.Debug
{
    public interface IDataProviderDebug
    {
        void ResetDatabase();
        void SandBox();
    }

    public class DataProviderDebug : IDataProviderDebug
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;
        private readonly IPostQuery _postQuery;
        private readonly IPostContext _postContext;
        private readonly IPersonContext _personContext;
        private readonly ITransactionCommand _transactionCommand;
        private readonly ITransactionQuery _transactionQuery;
        private readonly IGroupCommand _groupCommand;

        public DataProviderDebug(
            IWhoOwesWhatContext whoOwesWhatContext
            , IPostQuery postQuery
            , IPostContext postContext
            , IPersonContext personContext
            , ITransactionCommand transactionCommand
            , ITransactionQuery transactionQuery
            , IGroupCommand groupCommand
            )
        {
            _whoOwesWhatContext = whoOwesWhatContext;
            _postQuery = postQuery;
            _postContext = postContext;
            _personContext = personContext;
            _transactionCommand = transactionCommand;
            _transactionQuery = transactionQuery;
            _groupCommand = groupCommand;
        }

        public void ResetDatabase()
        {
            _whoOwesWhatContext.ResetDatabase();
        }

        public void SandBox()
        {
            //_whoOwesWhatContext.Sandbox();

            //var personIsUsed = _transactionQuery.GetPostsInUseByPersonGuid(new Guid("674E4EC8-38C6-4118-820A-D2A3ED4DD354"));

            _groupCommand.SaveGroup(new Domain.DTO.Group()
            {
                GroupGuid = new Guid("8d972370-6ee5-7ebb-4e45-c74f0b93a90b"),
                CreatedByPersonGuid = new Guid("674E4EC8-38C6-4118-820A-D2A3ED4DD354"),
                IsDeleted = false,
                Name = "Hawaii2012"
            });
        }

    }
}
