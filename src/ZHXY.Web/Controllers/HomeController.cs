using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
namespace ZHXY.Web.Controllers
{
    [LoginAuthentication]
    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<ViewResult> Default() => await Task.Run(() => View());

        [HttpGet]
        public async Task<ViewResult> Index() => await Task.Run(() => View());

        [HttpGet]
        public async Task<ViewResult> Import() => await Task.Run(() => View());

        [HttpGet]
        public async Task<ViewResult> About() => await Task.Run(() => View());

        //考勤模块跳转
        [HttpGet]
        public async Task<ViewResult> Kq() => await Task.Run(() => View());


    }
}