namespace Foodsharing_app.Models
{
    public class FoodSharingJwtSettings : IFoodSharingJwtSettings
    {
        public int RefreshTokenValidityHours { get; set; }
        
        public int AuthTokenValidityHours { get; set; }
        
        public string Issuer { get; set; }
        
        public string Audience { get; set; }
        
        public string Secret { get; set; }
    }

    public interface IFoodSharingJwtSettings
    {
        public int RefreshTokenValidityHours { get; set; }
        
        public int AuthTokenValidityHours { get; set; }
        
        public string Issuer { get; set; }
        
        public string Audience { get; set; }
        
        public string Secret { get; set; }
    }
}