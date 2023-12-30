namespace UserApi.Models;

public class UserInfos
{
    public UserInfos(string userId)
    {
        UserId = userId;
    }
    public string UserId { get;  set; } 
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}