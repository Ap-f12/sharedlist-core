using SharedListModels;
using Microsoft.Azure.Cosmos;
using System.Net;
using System.Web;

namespace SharedListApi.Data
{

    public class CosmosDbDataService : IDataService
    {

        private CosmosClient _client;
        private Database _database;
        private Container _container { get; set; }


        public CosmosDbDataService(IConfiguration configuration)
        {
            _client = new CosmosClient(accountEndpoint: configuration["CosmosDbEndpoint"], authKeyOrResourceToken: configuration["CosmosDbKey"]);
            _database = _client.GetDatabase(configuration["DbName"]);
            _container = _database.GetContainer("CheckListContainer");
        }
        public async Task<bool> UpsertItemAsync(CheckListModel checkListModel)
        {

            var itemRespone = await _container.UpsertItemAsync<CheckListModel>(checkListModel, new PartitionKey(checkListModel.UserId));
            return itemRespone.StatusCode == HttpStatusCode.OK || itemRespone.StatusCode == HttpStatusCode.Created;


        }


        public async Task<CheckListModel> GetItemByIdAsync(string userId, string checkListId)
        {
            return await _container.ReadItemAsync<CheckListModel>(
                id: checkListId,
                partitionKey: new PartitionKey(userId)
                );
        }

        public async Task<List<CheckListModel>> GetAllItemsByUserAsync(string userId)
        {

            var iterator = _container.GetItemQueryIterator<CheckListModel>(queryDefinition: null, requestOptions: new QueryRequestOptions()
            {
                PartitionKey = new PartitionKey(userId)
            }); ;

            var checkLists = new List<CheckListModel>();
            while (iterator.HasMoreResults)
            {
                var result = await iterator.ReadNextAsync();
                checkLists.AddRange(result.Resource);
            }
            return checkLists;

        }

        public async Task<bool> DeleteItemByIdAsync(string userId, string id)
        {
            var itemResponse = await _container.DeleteItemAsync<CheckListModel>(
                    id: id,
                    partitionKey: new PartitionKey(userId)
                );

            return itemResponse.StatusCode == HttpStatusCode.NoContent;


        }
    }


}
