using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;

namespace ZHXY.Web.Controllers
{
    public class DataController : Controller
    {
        public DataController(DataService app) => App = app;

        public DataService App { get; }

        public async Task<string> GetMenuName(string menuId) => 
            await App.GetMenuName(menuId);
    }
}