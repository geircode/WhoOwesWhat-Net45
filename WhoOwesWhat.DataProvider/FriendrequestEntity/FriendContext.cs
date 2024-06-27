using System.Linq;
using WhoOwesWhat.DataProvider.Entity;

namespace WhoOwesWhat.DataProvider.FriendrequestEntity
{
    public interface IFriendrequestContext
    {
        Entity.Friendrequest GetFriendrequest(int friendrequestId);
    }

    public class FriendrequestContext : IFriendrequestContext
    {
        private readonly IWhoOwesWhatContext _whoOwesWhatContext;

        public FriendrequestContext(IWhoOwesWhatContext whoOwesWhatContext)
        {
            _whoOwesWhatContext = whoOwesWhatContext;
        }

        public Friendrequest GetFriendrequest(int friendrequestId)
        {
            return _whoOwesWhatContext.GetFriendrequestSqlRepository().GetAll().SingleOrDefault(a => a.FriendrequestId == friendrequestId);
        }
    }
}
