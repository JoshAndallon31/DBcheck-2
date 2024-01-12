namespace myUnis.MAUI.Interfaces;

public interface ILogInRepository
{
    Task<UserInfo> Login(string username, string password);
}