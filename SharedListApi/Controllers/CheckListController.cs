using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedListApi.Data;
using SharedListModels;

namespace SharedListApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckListController : Controller
    {
        
        IDataService _dataAccess;
        public CheckListController(IDataService dataAccess)
        {
            _dataAccess = dataAccess;

        }
        [HttpGet]
        public async Task<ActionResult> Index(string UserId, string Id)
        {
            CheckListModel checkList =  await _dataAccess.GetItemAsync(UserId, Id);

            return Ok(checkList);
        }
        [HttpPost]
        [Route("Create")]
        
        
        public ActionResult Create([FromBody]CheckListModel checkListModel)
        {
            _dataAccess.UpsertItemAsync(checkListModel);
            return Ok();
        }
        [HttpPost]
        [Route("Update")]

        public ActionResult Update([FromBody] CheckListModel checkListModel)
        {
            _dataAccess.UpsertItemAsync(checkListModel);
            return Ok();
        }


    }
}
