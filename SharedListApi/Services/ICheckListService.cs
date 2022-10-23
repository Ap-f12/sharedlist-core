using SharedListModels;

namespace SharedListApi.Services
{
    public interface ICheckListService
    {
        Task<bool> CreateOrUpdateCheckListAsync(CheckListModel checkListModel, string token);
        Task<bool> DeleteCheckListAsync(string userId, string checkListId, string token);
        string? GenerateCheckListShareCode(CheckListPermissionModel checkListPermissionModel, string token);
        Task<List<CheckListModel>?> GetAllCheckListsByUserIdAsync(string userId, string token);
        Task<CheckListModel?> GetCheckListByIdAsync(string userId, string checkListId, string token);
        Task<CheckListModelWithPermissions> GetCheckListFromCodeAsync(string code);
        UserRegistrationModel RegisterUser();
        Task<bool> UpdateSharedCheckListAsync(CheckListModel checkListModel, string code);
    }
}