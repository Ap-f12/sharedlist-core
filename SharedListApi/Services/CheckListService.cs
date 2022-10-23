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

        public UserRegistrationModel RegisterUser()
        {
            UserRegistrationModel userRegistrationModel = new UserRegistrationModel();
            userRegistrationModel.UserId = _userRegistrationService.GenerateUserId();
            userRegistrationModel.Token = _userRegistrationService.GenerateToken(userRegistrationModel.UserId);

            return userRegistrationModel;

        }

        public async Task<List<CheckListModel>?> GetAllCheckListsByUserIdAsync(string userId, string token)
        {
            var isTokenValid = _userRegistrationService.IsTokenValid(userId, token);

            if (isTokenValid == true)
            {
                return await _dataService.GetAllItemsByUserAsync(userId);
            }
            return null;

        }

        public async Task<bool> CreateOrUpdateCheckListAsync(CheckListModel checkListModel, string token)
        {
            var isTokenValid = _userRegistrationService.IsTokenValid(checkListModel.UserId, token);

            if (isTokenValid == true)
            {
                await _dataService.UpsertItemAsync(checkListModel);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteCheckListAsync(string userId, string checkListId, string token)
        {
            var isTokenValid = _userRegistrationService.IsTokenValid(userId, token);
            if (isTokenValid == true)
            {
                await _dataService.DeleteItemByIdAsync(userId, checkListId);
            }
            return false;
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

        public string? GenerateCheckListShareCode(CheckListPermissionModel checkListPermissionModel, string token)
        {
            var isTokenValid = _userRegistrationService.IsTokenValid(checkListPermissionModel.UserId, token);
            if (isTokenValid == true)
            {
                return _shareCheckListService.GenerateShareCode(checkListPermissionModel);
            }
            return null;

        }

        public async Task<CheckListModel?> GetCheckListByIdAsync(string userId, string checkListId, string token)
        {
            var isTokenValid = _userRegistrationService.IsTokenValid(userId, token);
            if (isTokenValid == true)
            {
                return await _dataService.GetItemByIdAsync(userId, checkListId);
            }
            return null;

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
