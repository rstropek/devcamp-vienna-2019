using System.Linq;
using System.Threading.Tasks;
using HotelsApi.DataAccess;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace HotelsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ODataController
    {
        private readonly HotelsContext Context;
        private readonly DemoDataFiller Filler;

        public HotelsController(HotelsContext context, DemoDataFiller filler)
        {
            Context = context;
            Filler = filler;
        }

        [EnableQuery]
        public IActionResult Get() => Ok(Context.Hotels);

        [EnableQuery]
        public IActionResult Get(int id) => Ok(Context.Hotels.FirstOrDefault(h => h.ID == id));

        [Route("init")]
        [HttpGet]
        public async Task<IActionResult> Initialize()
        {
            await Filler.ClearDatabaseAsync();
            await Filler.FillDatabaseAsync();
            return Ok();
        }
    }
}
