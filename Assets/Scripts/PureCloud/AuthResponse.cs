namespace Assets.Scripts.PureCloud
{
    public class AuthResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string error { get; set; }

        public override string ToString()
        {
            return "[AuthResponse]\n" +
                "  access_token=" + access_token + "\n" +
                "  token_type=" + token_type + "\n" +
                "  expires_in=" + expires_in + "\n" +
                "  error=" + error;
        }
    }
}
