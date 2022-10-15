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

        //public async Task UpdateItemAsync(CheckListModel checkListModel)
        //{

        //    List<PatchOperation> operations = new()
        //    {
        //            PatchOperation.Replace("/CheckListItems", checkListModel.CheckListItems),
        //            PatchOperation.Replace("/UpdatedOn", checkListModel.UpdatedOn),

        //    };
        //    await _container.PatchItemAsync<CheckListModel>(
        //        id: checkListModel.id,
        //        partitionKey: new PartitionKey("/UserId"),
        //        patchOperations: operations
        //        );
        //}

        public async Task<CheckListModel> GetItemAsync(string userId, string checkListId)
        {
            return await _container.ReadItemAsync<CheckListModel>(
                id: checkListId,
                partitionKey: new PartitionKey(userId)
                );
        }


    }

    
}
