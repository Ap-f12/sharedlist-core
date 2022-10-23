using SharedListModels;
using Microsoft.Azure.Cosmos;

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
        public async Task UpsertItemAsync(CheckListModel checkListModel)
        {
          
            await _container.UpsertItemAsync<CheckListModel>(checkListModel,new PartitionKey(checkListModel.UserId));
          
          
        }


        public async Task<CheckListModel> GetItemByIdAsync(string userId, string id)
        {
            return await _container.ReadItemAsync<CheckListModel>(
                id: id,
                partitionKey: new PartitionKey(userId)
                );
        }

        public async Task<List<CheckListModel>> GetAllItemsByUserAsync( string userId)
        {
            
            var iterator = _container.GetItemQueryIterator<CheckListModel>(queryDefinition:null, requestOptions: new QueryRequestOptions()
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

        public async Task DeleteItemByIdAsync(string userId, string id)
        {
            await _container.DeleteItemAsync<CheckListModel>(
                id: id,
                partitionKey: new PartitionKey(userId)
                );
        }
    }

    
}
