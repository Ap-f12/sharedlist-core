using SharedListModels;

namespace SharedListApi.Services
{
    public interface ICheckListService
    {
        Task<bool> CreateOrUpdateCheckListAsync(CheckListModel checkListModel);
        Task<bool> DeleteCheckListAsync(string userId, string checkListId);
        string? GenerateCheckListShareCode(CheckListPermissionModel checkListPermissionModel);
        Task<List<CheckListModel>?> GetAllCheckListsByUserIdAsync(string userId);
        Task<CheckListModel?> GetCheckListByIdAsync(string userId, string checkListId);
        Task<CheckListModelWithPermissions> GetCheckListFromCodeAsync(string code);
        
        Task<bool> UpdateSharedCheckListAsync(CheckListModel checkListModel, string code);
    }
}