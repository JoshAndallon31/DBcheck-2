using UserApi.Models;

namespace UserApi.Data;

public interface IUserInfosRepository : IRepository<UserInfos>
{
    Task<List<UserInfos>> GetAllAsync();
    Task<UserInfos?> GetById(string id);
    Task<UserInfos> GetByUserName(string userName);

}