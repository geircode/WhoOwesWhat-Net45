// ReSharper disable InconsistentNaming
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;

namespace WhoOwesWhat.Service.DTO
{
    [Route("/authenticateUser")]
    public class AuthenticateUserRequest
    {
        // username or email. Todo: Email confirmation on account creation
        public string username { get; set; }
        public string password { get; set; }
    }

    [Route("/user/new")]
    public class CreateUserRequest
    {
        public Guid personGuid { get; set; }
        public string displayname { get; set; }
        public string email { get; set; }
        public string mobil { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }

    [Route("/user/getPersonByEmail")]
    public class GetPersonByEmailRequest
    {
        public string username { get; set; }
        public string password { get; set; }
        public string emailCriteria { get; set; }
    }

    public class GetPersonByEmailResponse
    {
        public bool isSuccess { get; set; }

        public string username { get; set; }
        public string displayname { get; set; }
    }

    [Route("/friendrequest/sendFriendrequest")]
    public class SendFriendrequest
    {
        public string username { get; set; }
        public string password { get; set; }
        public string friendUsername { get; set; }
    }

    [Route("/friendrequest/getFriendrequests")]
    public class GetFriendrequestsRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    [Route("/friendrequest/acceptFriendrequest")]
    public class AcceptFriendrequestRequest
    {
        public string username { get; set; }
        public string password { get; set; }
        public string friendUsername { get; set; }
    }

    public class GetFriendrequestsReponse
    {
        public GetFriendrequestsReponse()
        {
            friendrequests = new List<Friendrequest>();
        }

        public bool isSuccess { get; set; }
        public List<Friendrequest> friendrequests { get; set; }

        public class Friendrequest
        {
            public string username { get; set; }
            public string displayname { get; set; }
        }
    }

    [Route("/debug/resetdatabase")]
    public class ResetDatabaseRequest
    {
    }

    [Route("/debug/sandbox")]
    public class SandBoxRequest
    {
    }

    [Route("/error/save")]
    public class SaveErrorRequest
    {
        public string message { get; set; }
        public string error { get; set; }
    }

    [Route("/sync/syncFriendsToServer")]
    public class SyncFriendsRequest
    {
        public string username { get; set; }
        public string password { get; set; }
        public List<Friend> friends { get; set; }

        public class Friend
        {
            /// <summary>
            /// Read https://www.evernote.com/shard/s292/nl/45749169/856ab15f-93d2-4aab-865d-8f4252852edf
            /// Is in use when automatically merging between same user logins over several devices. Usually in order to get presedence over previous deletion.
            /// </summary>
            public bool isUsedInPosts;
            public Guid personGuid { get; set; }
            public string displayname { get; set; }
            public bool isFriendDeleted { get; set; }
        }
    }

    [Route("/sync/syncFriendsFromServer")]
    public class SyncFriendsToAppRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    public class SyncFriendsToAppResponse
    {
        public SyncFriendsToAppResponse()
        {
            friends = new List<Friend>();
        }

        public bool isSuccess { get; set; }
        public List<Friend> friends { get; set; }
        public long executionTimeInMilliseconds { get; set; }

        public class Friend
        {
            public Guid friendGuid { get; set; }
            public Guid ownerGuid { get; set; }
            public string displayname { get; set; }
            public bool isFriendDeleted { get; set; }
            public bool isFriendOnlinePerson { get; set; }
            public bool isPersonDeleted { get; set; }
        }
    }

    [Route("/sync/syncPostsToServer")]
    public class SyncPostsRequest
    {

        public string username { get; set; }
        public string password { get; set; }
        public DateTime? lastSynchronizedToServer { get; set; }

        public List<Post> posts { get; set; }

        public class Post
        {
            public Post()
            {
                payerTransactions = new List<Transaction>();
                consumerTransactions = new List<Transaction>();
            }

            public Guid postGuid { get; set; }
            public DateTime purchaseDate { get; set; }
            public string description { get; set; }
            public string totalCost { get; set; }
            public string iso4217CurrencyCode { get; set; }
            public int version { get; set; }
            public bool isDeleted { get; set; }
            public string comment { get; set; }
            public Guid? groupGuid { get; set; }
            public Guid lastUpdatedByPersonGuid { get; set; }
            public DateTime lastUpdated { get; set; }
            public Guid createdByPersonGuid { get; set; }
            public DateTime created { get; set; }
            public List<Transaction> payerTransactions { get; set; }
            public List<Transaction> consumerTransactions { get; set; }
        }

        public class Transaction
        {
            public Guid personGuid { get; set; }
            public bool isAmountSetManually { get; set; }
            public string amountSetManually { get; set; }
        }
    }
    public class SyncPostsResponse
    {

        public List<Post> postConflicts { get; set; }
        public List<Post> unsyncPosts { get; set; }

        public SyncPostsResponse()
        {
        }

        public bool isSuccess { get; set; }
        public long executionTimeInMilliseconds { get; set; }

        public class Post
        {
            public Post()
            {
                payerTransactions = new List<Transaction>();
                consumerTransactions = new List<Transaction>();
            }

            public Guid postGuid { get; set; }
            public DateTime purchaseDate { get; set; }
            public string description { get; set; }
            public string totalCost { get; set; }
            public string iso4217CurrencyCode { get; set; }
            public int version { get; set; }
            public bool isDeleted { get; set; }
            public string comment { get; set; }
            public Guid? groupGuid { get; set; }
            public Guid lastUpdatedByPersonGuid { get; set; }
            public DateTime lastUpdated { get; set; }
            public Guid createdByPersonGuid { get; set; }
            public DateTime created { get; set; }
            public List<Transaction> payerTransactions { get; set; }
            public List<Transaction> consumerTransactions { get; set; }
        }

        public class Transaction
        {
            public Guid personGuid { get; set; }
            public string displayname { get; set; }
            public bool isAmountSetManually { get; set; }
            public string amountSetManually { get; set; }
        }
    }

    [Route("/sync/syncGroupsToServer")]
    public class SyncGroupsRequest
    {
        public string username { get; set; }
        public string password { get; set; }
        public List<Group> groups { get; set; }
        public DateTime? lastSynchronizedToServer { get; set; }

        public class Group
        {
            public bool isInUseOnApp { get; set; }
            public Guid groupGuid { get; set; }
            public string name { get; set; }
            public bool isDeleted { get; set; }
            public Guid createdByPersonGuid { get; set; }
        }
    }
    public class SyncGroupsResponse
    {
        public bool isSuccess { get; set; }
        public long executionTimeInMilliseconds { get; set; }
        public List<Group> groups { get; set; }

        public class Group
        {
            public Guid groupGuid { get; set; }
            public string name { get; set; }
            public bool isDeleted { get; set; }
            public Guid createdByPersonGuid { get; set; }
        }
    }



    public class AuthenticateUserResponse
    {
        public bool isSuccess { get; set; }

        public Guid personGuid { get; set; }
        public string displayname { get; set; }
        public string mobil { get; set; }
    }


    public class BasicResponse
    {
        public long executionTimeInMilliseconds { get; set; }
        public bool isSuccess { get; set; }
    }
}

// ReSharper restore InconsistentNaming
