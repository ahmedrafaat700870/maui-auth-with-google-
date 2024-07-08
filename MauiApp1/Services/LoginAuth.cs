namespace MauiApp1.Services
{
    public class LoginAuth
    {
        private string auth_uri = "https://accounts.google.com/o/oauth2/auth";
        private string client_id = "301265092072-me4vjbna9i3tr8upqrq7tturnbmv82dg.apps.googleusercontent.com";
        private string token_ur = "https://oauth2.googleapis.com/token";

        public async Task<string> AuthWithGoogle()
        {
            var authUrl = $"{auth_uri}?response_type=code" +
                    $"&redirect_uri=com.maui.login://" +
                    $"&client_id={client_id}" +
                    $"&scope=https://www.googleapis.com/auth/userinfo.email" +
                    $"&include_granted_scopes=true" +
                    $"&state=state_parameter_passthrough_value";


            var callbackUrl = "com.maui.login://";

            try
            {

                var response = await WebAuthenticator.AuthenticateAsync(new WebAuthenticatorOptions()
                {
                    Url = new Uri(authUrl),
                    CallbackUrl = new Uri(callbackUrl)
                });

                var codeToken = response.Properties["code"];

                var parameters = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string,string>("grant_type","authorization_code"),
                        new KeyValuePair<string,string>("client_id",client_id),
                        new KeyValuePair<string,string>("redirect_uri",callbackUrl),
                        new KeyValuePair<string,string>("code",codeToken),
                    });


                HttpClient client = new HttpClient();
                var accessTokenResponse = await client.PostAsync(token_ur, parameters);


                if (accessTokenResponse.IsSuccessStatusCode)
                {
                    var data = await accessTokenResponse.Content.ReadAsStringAsync();
                    return data;
                    
                }
            }
            catch (TaskCanceledException e)
            {
                // Use stopped auth
                throw;
            }
            return null;
        }
    }
}
