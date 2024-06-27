
#if DEBUG

using WhoOwesWhat.Service.DTO;
namespace WhoOwesWhat.Service
{
    public partial class WhoOwesWhatService
    {

        public BasicResponse Any(ResetDatabaseRequest request)
        {
            _dataProviderDebug.ResetDatabase();
            return new BasicResponse(){isSuccess = true};
        }

        public BasicResponse Any(SandBoxRequest request)
        {
            _dataProviderDebug.SandBox();
            return new BasicResponse(){isSuccess = true};
        }

    }
}

#endif