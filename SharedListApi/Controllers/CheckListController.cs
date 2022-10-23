using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
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

        ICheckListService _checkListService;
       
       
        public CheckListController(ICheckListService checkListService)
        {
            _checkListService = checkListService;

        }

        [HttpGet]
        [Route("Register")]
        public ActionResult RegisterNewUser()
        {
            var user = _checkListService.RegisterUser();
            return Ok(user);
        }
        [HttpGet]
        public async Task<ActionResult> GetCheckListById(string userId, string id)
        {

            var isTokenPresentInRequest = TryGetAuthorizationTokenFromRequest(out string? token);

            if(isTokenPresentInRequest == true && token != null)
            {
                CheckListModel? checkList = await _checkListService.GetCheckListByIdAsync(userId,id,token);
                return Ok(checkList);
            }
            return Unauthorized();

          
        }

        

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult> GetAllCheckListsByUserId(string userId)
        {
            var isTokenPresentInRequest = TryGetAuthorizationTokenFromRequest(out string? token);

            if (isTokenPresentInRequest == true && token != null)
            {
                List<CheckListModel>? checkLists = await _checkListService.GetAllCheckListsByUserIdAsync(userId, token);
                return Ok(checkLists);
            }
            return Unauthorized();


        }
        [HttpPost]
        [Route("Create")]
        [Route("Update")]


        public async Task<ActionResult> Create([FromBody]CheckListModel checkList)
        {
            var isTokenPresentInRequest = TryGetAuthorizationTokenFromRequest(out string? token);

            if (isTokenPresentInRequest == true && token != null)
            {
                var success = await _checkListService.CreateOrUpdateCheckListAsync(checkList, token);
                if(success == true)
                {
                    return Ok("success");
                }
                else
                {
                    return BadRequest("something went wrong");
                }
            };
            return Unauthorized();
        }

        [HttpPost]
        [Route("Delete")]

        public async Task<ActionResult> Delete( string userId, string id)
        {
            var isTokenPresentInRequest = TryGetAuthorizationTokenFromRequest(out string? token);

            if (isTokenPresentInRequest == true && token != null)
            {
                var success = await _checkListService.DeleteCheckListAsync(userId, id, token);
                if (success == true)
                {
                    return Ok("success");
                }
                else
                {
                    return BadRequest("something went wrong");
                }
            }

            return Unauthorized();
           
        }


        [HttpPost]
        [Route("UpdateShared")]

        public async Task<ActionResult> UpdateSharedChecklist(CheckListModel checklist, string code)
        {
            var success = await _checkListService.UpdateSharedCheckListAsync(checklist, code);
            if(success == true)
            {
                return Ok("Updated");
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("GetShareCode")]
        public ActionResult GetShareCode([FromBody]CheckListPermissionModel checkListPermissions)
        {
            var isTokenPresentInRequest = TryGetAuthorizationTokenFromRequest(out string? token);

            if (isTokenPresentInRequest == true && token != null)
            {
                var code = _checkListService.GenerateCheckListShareCode(checkListPermissions, token);
                return Ok(code);
            }

            return Unauthorized();
        }

        [HttpGet]
        [Route("GetChecklist/{code}")]
        public async Task<ActionResult> GetChecklistDetailsFromCode(string code)
        {
            var checklist = await _checkListService.GetCheckListFromCodeAsync(code);


            return Ok(checklist);
        }

        [NonAction]
        public bool TryGetAuthorizationTokenFromRequest(out string? token)
        {
            if (Request.Headers.TryGetValue("Authorization", out var authToken) == true)
            {
                token = authToken;
                return true;
            }
            else
            {
                token = null;
                return false;
            }
          ;
        }


    }
}
