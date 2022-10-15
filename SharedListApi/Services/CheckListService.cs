using SharedListModels;

namespace SharedListApi.Services
{
    public class CheckListService : ICheckListService
    {
        public async Task<List<CheckListModel>> GetAllChecklistsByUserAsync(string username)
        {
            throw new NotImplementedException();
        }

        public async  Task<CheckListItemModel> GetCheckListByCodeAsync(string code)
        {
            throw new NotImplementedException();
        }

        public async Task SaveCheckListAsync(CheckListModel checkList)
        {
            throw new NotImplementedException();
        }
    }
}
