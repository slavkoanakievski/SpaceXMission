using System.Net;

namespace SpaceXMission_Shared.Constants
{
    public static class ErrorMessages
    {
        public const string InvalidEmailOrPassword = "The email or password you entered is incorrect. Please try again.";
        public const string UsernameDoesNotExist = "The username you entered does not exist.Please check your credentials and try again.";
        public const string InvalidPassword  = "The password you entered is incorrect. Please try again.";
        public const string GenericErrorControllerMessage = "An unexpected error occurred. Please try again later or contact support if the problem persists.";
        public const string PasswordsDoNotMatchMessage = "The passwords provided do not match. Please try again.";
        public const string UserAlreadyExistsError = "A user with this email address already exists. Please use a different email.";
        public const string InvalidEmailOrPasswordFormat = "Invalid email or password format. Please check the requirements.";
        public const string InvalidClientRequest = "Invalid client request.";
        public const string GenericMessage = "An unexpected error occurred.Please try again later or contact support.";

    }
}
