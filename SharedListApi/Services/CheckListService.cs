using SharedListApi.Data;
using SharedListModels;

namespace SharedListApi.Services
{
    public class CheckListService : ICheckListService
    {
        IUserRegistrationService _userRegistrationService;
        IShareCheckListService _shareCheckListService;
        IDataService _dataService;

        public CheckListService(IUserRegistrationService userRegistrationService, IShareCheckListService shareCheckListService, IDataService dataService)
        {
            _userRegistrationService = userRegistrationService;
            _shareCheckListService = shareCheckListService;
            _dataService = dataService;
        }

       

        public async Task<List<CheckListModel>?> GetAllCheckListsByUserIdAsync(string userId)
        {
          
            return await _dataService.GetAllItemsByUserAsync(userId);
           

        }

        public async Task<bool> CreateOrUpdateCheckListAsync(CheckListModel checkListModel)
        {
            
            return await _dataService.UpsertItemAsync(checkListModel);
           
            
        }

        public async Task<bool> DeleteCheckListAsync(string userId, string checkListId)
        {
           
            await _dataService.DeleteItemByIdAsync(userId, checkListId);
            return true;
            
        }

        public async Task<bool> UpdateSharedCheckListAsync(CheckListModel checkListModel, string code)
        {
            var checkListPermissionModel = _shareCheckListService.GetCheckListDetailsFromCode(code);
            if (checkListPermissionModel.Permissions == PermissionsEnum.ReadWrite)
            {
                await _dataService.UpsertItemAsync(checkListModel);
                return true;
            }
            return false;
        }

        public string? GenerateCheckListShareCode(CheckListPermissionModel checkListPermissionModel)
        {
            
             return _shareCheckListService.GenerateShareCode(checkListPermissionModel);
            

        }

        public async Task<CheckListModel?> GetCheckListByIdAsync(string userId, string checkListId)
        {
           
            return await _dataService.GetItemByIdAsync(userId, checkListId);
            

        }

        public async Task<CheckListModelWithPermissions> GetCheckListFromCodeAsync(string code)
        {
            var checkListPermissionModel = _shareCheckListService.GetCheckListDetailsFromCode(code);
            var checkListModelWithPermission = new CheckListModelWithPermissions();

            checkListModelWithPermission.CheckListModel = await _dataService.GetItemByIdAsync(checkListPermissionModel.UserId, checkListPermissionModel.Id);
            checkListModelWithPermission.Permissions = checkListPermissionModel.Permissions;
            return checkListModelWithPermission;
        }
    }
}
