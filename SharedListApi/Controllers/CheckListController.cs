using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.RecordIO;
using SharedListApi.Data;
using SharedListApi.Services;
using SharedListModels;
using System.IO;

namespace SharedListApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckListController : Controller
    {
        
        IDataService _dataService;
        IApiKeyService _apiKeyService;
       
        public CheckListController(IDataService dataService, IApiKeyService apiKeyService)
        {
            _dataService = dataService;
            _apiKeyService = apiKeyService;

        }

        [HttpGet]
        [Route("OnBoard")]
        public ActionResult OnBoardNewUser()
        {
            dynamic user = new System.Dynamic.ExpandoObject();
            user.UserId = UserIdService.GenerateUserId();
            user.apiKey = _apiKeyService.GenerateApiKey(user.UserId);
            return Ok(user);
        }
        [HttpGet]
        public async Task<ActionResult> GetCheckListById(string UserId, string Id)
        {
            CheckListModel checkList =  await _dataService.GetItemAsync(UserId, Id);

            return Ok(checkList);
        }

        [HttpGet]
        [Route("test")]
        public ActionResult TestMethod(string UserId)
        {
            if (Request.Headers.TryGetValue("ApiKey", out var apiKey) == false)
            {
                return BadRequest("Api key missing");
            };
            var isvalid = _apiKeyService.IsApiKeyValid(UserId, apiKey);
           
            return Ok(isvalid);
           
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult> GetAllCheckListsByUserId(string UserId)
        {
            
            if(Request.Headers.TryGetValue("ApiKey", out var apiKey) == false)
            {
                return BadRequest("Api key missing");
            };
            var isApiKeyValid = _apiKeyService.IsApiKeyValid(UserId, apiKey);
            if (isApiKeyValid == true){
                List<CheckListModel> checkLists = await _dataService.GetAllItemsByUserAsync(UserId);
                return Ok(checkLists);
            }
            return Ok("");
            

           
        }
        [HttpPost]
        [Route("Create")]
        [Route("Update")]


        public async Task<ActionResult> Create([FromBody]CheckListModel checkListModel)
        {
           await _dataService.UpsertItemAsync(checkListModel);
            return Ok();
        }

        [HttpPost]
        [Route("Delete")]

        public async Task<ActionResult> Delete( string UserId, string Id)
        {
            await _dataService.DeleteItemByIdAsync(UserId, Id);
            return Ok();
        }
      


    }
}
