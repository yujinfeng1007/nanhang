
using ZHXY.Application;
namespace ZHXY.Dorm.Web.Controllers
{

    public class ChangeLogsController : ZhxyWebControllerBase
	{
		private ChangeLogsAppService App { get; }
        public ChangeLogsController(ChangeLogsAppService app) => App = app;
    }
}