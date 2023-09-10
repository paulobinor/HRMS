namespace hrms_be_backend_data.ViewModel
{
    public class AuthResponse
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
        public string Message { get; set; }
    }
}
