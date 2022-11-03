using SharedListModels;

namespace SharedListApi.Data
{
    public interface IDataService
    {
        Task<bool> UpsertItemAsync(CheckListModel checkListModel);
        
        Task<CheckListModel> GetItemByIdAsync(string userId, string id);

        Task<List<CheckListModel>> GetAllItemsByUserAsync(string userId);

        Task<bool> DeleteItemByIdAsync(string userId, string id);


    }
}
