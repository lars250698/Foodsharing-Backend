using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Foodsharing_app.Models
{
    public class FoodItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public DateTime BestBefore { get; set; }
        
        public DateTime InsertedAt { get; set; }
        
        public string InsertedBy { get; set; }
        
        public bool Claimed { get; set; }
        
        public string ClaimedBy { get; set; }
        
        public bool Closed { get; set; }
        
        public DateTime ClosedAt { get; set; }
        
        public List<double> Coordinates { get; set; }
        
        [JsonIgnore]
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Position { get; private set; }

        public bool Show() => !Closed && !Claimed;
        public void SetPosition(List<double> coordinates) => 
            SetPositionFromCoordinates(coordinates);

        private void SetPositionFromCoordinates(IReadOnlyList<double> coordinates)
        {
            Position = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
                new GeoJson2DGeographicCoordinates(coordinates[0], coordinates[1]));
        }
    }
}