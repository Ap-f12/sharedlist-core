using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.RecordIO;
using SharedListApi.Filters;
using SharedListApi.Data;
using SharedListApi.Services;
using SharedListModels;
using System.IO;

namespace SharedListApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(ValidateTokenFilter))]
    public class CheckListController : Controller
    {

        ICheckListService _checkListService;


        public CheckListController(ICheckListService checkListService)
        {
            _checkListService = checkListService;

        }


        [HttpGet]
        public async Task<IActionResult> GetCheckListByIdAsync(string userId, string checkListId)
        {


            CheckListModel? checkList = await _checkListService.GetCheckListByIdAsync(userId, checkListId);
            return Ok(checkList);



        }



        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllCheckListsByUserIdAsync(string userId)
        {

            List<CheckListModel>? checkLists = await _checkListService.GetAllCheckListsByUserIdAsync(userId);
            return Ok(checkLists);



        }
        [HttpPost]
        [Route("Create")]
        [Route("Update")]


        public async Task<IActionResult> CreateAsync([FromBody] CheckListModel checkList)
        {

            return Ok(await _checkListService.CreateOrUpdateCheckListAsync(checkList));


        }

        [HttpPost]
        [Route("Delete")]

        public async Task<IActionResult> DeleteAsync(string userId, string checkListId)
        {
            return Ok(await _checkListService.DeleteCheckListAsync(userId, checkListId));

        }


        [HttpPost]
        [Route("UpdateShared")]

        public async Task<IActionResult> UpdateSharedChecklistAsync([FromBody] CheckListModel checklist, string code)
        {
            return Ok(await _checkListService.UpdateSharedCheckListAsync(checklist, code));
        }

        [HttpPost]
        [Route("GenerateShareCode")]
        public IActionResult GetShareCode([FromBody] CheckListPermissionModel checkListPermissions)
        {
          
            var code = _checkListService.GenerateCheckListShareCode(checkListPermissions);
            return Ok(code);
            
        }
        
        
        

        [HttpGet]
        [Route("GetChecklist")]
        public async Task<IActionResult> GetChecklistDetailsFromCodeAsync(string code)
        {
            var checklist = await _checkListService.GetCheckListFromCodeAsync(code);


            return Ok(checklist);
        }

     


    }
}
