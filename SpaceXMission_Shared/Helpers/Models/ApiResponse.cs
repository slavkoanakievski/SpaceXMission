namespace SpaceXMission_Shared.Helpers.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public T? Data { get; set; }

        public ApiResponse() { }

        public ApiResponse(T? data)
        {
            Data = data;
            Success = true;
        }

        public ApiResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
            Success = false;
        }
    }

}
