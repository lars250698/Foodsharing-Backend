using System;

namespace Foodsharing_app.Exceptions
{
    public class UserNotAuthorizedException : Exception
    {
        public UserNotAuthorizedException(string reason) : base(ModifyReason(reason))
        {
        }

        public UserNotAuthorizedException(string username, string reason) : base(ModifyReason(reason))
        {
        }

        private static string ModifyReason(string reason) =>
            $"User could not be authorized: {reason}";

        private static string ModifyReason(string username, string reason) =>
            $"User {username} could not be authorized: {reason}";
    }
}