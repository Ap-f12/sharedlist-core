using SharedListModels;

namespace SharedListApi.Services
{
    public interface IShareCheckListService
    {
        string GenerateShareCode(CheckListPermissionModel checkListPermissionModel);
        CheckListPermissionModel GetCheckListDetailsFromCode(string token);

    }
}