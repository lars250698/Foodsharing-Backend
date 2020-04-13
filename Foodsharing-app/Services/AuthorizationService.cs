using System;
using System.Text.Json;
using Foodsharing_app.Exceptions;
using Foodsharing_app.Models;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace Foodsharing_app.Services
{
    public class AuthorizationService
    {
        private readonly UserService _userService;
        private readonly FoodSharingJwtSettings _settings;

        private readonly IJwtAlgorithm _algorithm = new HMACSHA256Algorithm();
        private readonly IJsonSerializer _serializer = new JsonNetSerializer();
        private readonly IBase64UrlEncoder _urlEncoder = new JwtBase64UrlEncoder();

        public AuthorizationService(UserService userService, FoodSharingJwtSettings settings)
        {
            _userService = userService;
            _settings = settings;
        }

        public User AuthorizeUser(string header)
        {
            var splitHeader = header.Split(" ");
            if (splitHeader.Length != 2 || splitHeader[0] != "Bearer")
            {
                throw new UserNotAuthorizedException("Authorization header contains wrong payload");
            }

            var token = splitHeader[1];
            var decodedTokenPayload = DecodeAuthToken(token);
            ValidateToken(decodedTokenPayload);
            return _userService.Get(decodedTokenPayload.Subject);
        }

        public string Login(string username, string plainTextPassword)
        {
            var user = _userService.GetByUsername(username);
            var passwordIsCorrect = UserService.PasswordIsCorrect(user, plainTextPassword);
            if (!passwordIsCorrect)
            {
                throw new UserNotAuthorizedException(username, "Provided password is incorrect");
            }

            return GenerateRefreshToken(user.Id);
        }

        public string IssueAuthToken(string refreshToken) =>
            IssueAuthToken(DecodeRefreshToken(refreshToken));

        public string IssueAuthToken(RefreshTokenPayload refreshToken)
        {
            ValidateToken(refreshToken);
            var user = _userService.Get(refreshToken.Subject);
            return GenerateAuthToken(user);
        }

        private string GenerateRefreshToken(string userId)
        {
            RefreshTokenPayload payload = new RefreshTokenPayload
            {
                Issuer = _settings.Issuer,
                Subject = userId,
                Audience = _settings.Audience,
                IssuedAt = DateTime.Now.Millisecond,
                Expires = DateTime.Now.AddHours(_settings.RefreshTokenValidityHours).Millisecond,
            };
            return GenerateToken(payload);
        }

        private string GenerateAuthToken(User user)
        {
            AuthTokenPayload payload = new AuthTokenPayload
            {
                Issuer = _settings.Issuer,
                Subject = user.Id,
                Audience = _settings.Audience,
                IssuedAt = DateTime.Now.Millisecond,
                Expires = DateTime.Now.AddHours(_settings.RefreshTokenValidityHours).Millisecond,
                User = user
            };
            return GenerateToken(payload);
        }

        private void ValidateToken(ITokenPayload payload)
        {
            var expiresAsDateTime = DateTimeOffset.FromUnixTimeMilliseconds(payload.Expires).Date;
            var isTokenValid = payload.Audience == _settings.Audience &&
                               payload.Issuer == _settings.Issuer &&
                               DateTime.Compare(expiresAsDateTime, DateTime.Now) <= 0;
            if (!isTokenValid)
            {
                throw new UserNotAuthorizedException("JWT payload could not be validated");
            }
        }

        private string GenerateToken(ITokenPayload payload) =>
            new JwtEncoder(_algorithm, _serializer, _urlEncoder).Encode(payload, _settings.Secret);

        private RefreshTokenPayload DecodeRefreshToken(string token) =>
            JsonSerializer.Deserialize<RefreshTokenPayload>(DecodeTokenToJson(token));

        private AuthTokenPayload DecodeAuthToken(string token) =>
            JsonSerializer.Deserialize<AuthTokenPayload>(DecodeTokenToJson(token));

        private string DecodeTokenToJson(string token)
        {
            var provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(_serializer, provider);
            IJwtDecoder decoder = new JwtDecoder(_serializer, validator, _urlEncoder, _algorithm);
            return decoder.Decode(token, _settings.Secret, verify: true);
        }
    }
}