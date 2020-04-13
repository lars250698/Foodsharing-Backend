using Foodsharing_app.Models;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace Foodsharing_app.Services
{
    public class AuthorizationService
    {
        private readonly UserService _userService;

        public AuthorizationService(UserService userService)
        {
            _userService = userService;
        }

        private AuthTokenPayload DecodeJwt()
        {
            
        }

        public void AuthorizeUser(string header)
        {
            
        }

        public void GenerateRefreshToken()
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
        }
    }
}