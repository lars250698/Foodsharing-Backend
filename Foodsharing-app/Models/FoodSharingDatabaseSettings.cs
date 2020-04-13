namespace Foodsharing_app.Models
{
    public class FoodSharingDatabaseSettings : IFoodSharingDatabaseSettings
    {
        public string ItemsCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
        public string MasterDatabaseName { get; set; }
        public string DatabaseServer { get; set; }
        public int DatabasePort { get; set; }
        public string DatabaseUser { get; set; }
        public string DatabasePassword { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IFoodSharingDatabaseSettings
    {
        public string ItemsCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
        public string MasterDatabaseName { get; set; }
        public string DatabaseServer { get; set; }
        public int DatabasePort { get; set; }
        public string DatabaseUser { get; set; }
        public string DatabasePassword { get; set; }
        public string DatabaseName { get; set; }
    }
}