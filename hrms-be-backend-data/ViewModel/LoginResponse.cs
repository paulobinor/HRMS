namespace hrms_be_backend_data.ViewModel
{
    public class LoginResponse
    {       
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }     
    }

    public class RefreshTokenResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
