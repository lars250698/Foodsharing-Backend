using System;
using System.Collections.Generic;
using Foodsharing_app.Models;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Foodsharing_app.Services
{
    public class FoodItemService : ICrudService<FoodItem>
    {
        private readonly IMongoCollection<FoodItem> _items;

        public FoodItemService(IFoodSharingDatabaseSettings settings, DatabaseService databaseService)
        {
            _items = databaseService.Database.GetCollection<FoodItem>(settings.ItemsCollectionName);
            var indexOptions = new CreateIndexOptions {Background = false};
            var indexKeys = Builders<FoodItem>.IndexKeys.Geo2DSphere(item => item.Position);
            _items.Indexes.CreateOne(new CreateIndexModel<FoodItem>(indexKeys, indexOptions));
        }

        public List<FoodItem> Get() =>
            _items.Find(item => true).ToList();

        public FoodItem Get(string id) =>
            _items.Find(item => item.Id == id).FirstOrDefault();

        public List<FoodItem> GetByRadius(double lat, double lng, int radiusKilometre)
        {
            var radius = radiusKilometre * 1000;
            var point = GeoJson.Point(GeoJson.Geographic(lat, lng));
            var locationQuery = new FilterDefinitionBuilder<FoodItem>()
                .Near(item => item.Position, point, radius);
            return _items.Find(locationQuery).ToList();
        }

        public List<FoodItem> GetByUser(string userId) =>
            _items.Find(item => item.InsertedBy == userId).ToList();

        public FoodItem Create(FoodItem user)
        {
            _items.InsertOne(user);
            return user;
        }

        public void Update(string id, FoodItem foodItem) =>
            _items.ReplaceOne(item => item.Id == id, foodItem);

        public void Remove(FoodItem foodItem) =>
            _items.DeleteOne(item => item.Id == foodItem.Id);

        public void Remove(string id) =>
            _items.DeleteOne(item => item.Id == id);

        public void Claim(string id, string userId)
        {
            var item = Get(id);
            if (string.IsNullOrEmpty(item.ClaimedBy))
            {
                throw new InvalidOperationException($"Item with ID {item.Id} is already claimed");
            }

            item.ClaimedBy = userId;
            Update(id, item);
        }
    }
}