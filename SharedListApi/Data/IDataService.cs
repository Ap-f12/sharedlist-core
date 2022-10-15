using SharedListModels;

namespace SharedListApi.Data
{
    public interface IDataService
    {
        Task UpsertItemAsync(CheckListModel checkListModel);
        
        Task<CheckListModel> GetItemAsync(string userId, string id);
    }
}
