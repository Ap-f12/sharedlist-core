using SharedListModels;

namespace SharedListApi.Services
{
    public interface ICheckListService
    {
        public  Task<List<CheckListModel>> GetAllChecklistsByUserAsync(string username);

        public Task<CheckListItemModel> GetCheckListByCodeAsync(string code);

        public Task SaveCheckListAsync(CheckListModel checkList);
        
    }
}