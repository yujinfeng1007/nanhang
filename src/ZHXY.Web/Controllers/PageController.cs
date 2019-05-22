using System.Threading.Tasks;
using System.Web.Mvc;

namespace ZHXY.Web.Controllers
{
    public class PageController : Controller
    {
        public async Task<ViewResult> AddMenuForm() => await Task.Run(()=>View());
    }
}