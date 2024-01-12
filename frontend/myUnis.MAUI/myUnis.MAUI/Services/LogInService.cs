using myUnis.MAUI.Interfaces;
using Newtonsoft.Json;

namespace myUnis.MAUI.Services;

public class LogInService : ILogInRepository
{ 
    private readonly IAlertService _alertService;
    public async Task<UserInfo> Login(string username, string password)
    {
        try
        {
            var userInfo = new List<UserInfo>();
            var client = new HttpClient();
            //Put here the published api, it is an example and it's not recommended to do like this.
            var url = $"$https://api.example.com/users?username{username}=&password={password}" ;
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync(" ");
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                userInfo = JsonConvert.DeserializeObject<List<UserInfo>>(content);
                if (userInfo.Any())
                {
                    return await Task.FromResult(userInfo.FirstOrDefault());
                }
                else
                {
                    // Handle the case when userInfo is empty
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            await _alertService.ShowAlertAsync("Error", ex.Message);
            return null;
        }
    }
}