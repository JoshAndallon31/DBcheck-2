using Microsoft.EntityFrameworkCore;
using UserApi.Models;

namespace UserApi.Data;

public class UserInfosRepository : IUserInfosRepository
{
    private readonly UserInfoDataContext _context;
    
    public UserInfosRepository(UserInfoDataContext context)
    {
        _context = context;
    }

    public void Add(UserInfos newT)
    {
        _context.Add(newT);
    }
    public void Delete(UserInfos newT)
    {
        _context.Remove(newT);
    }
    public async Task<bool> SaveAllChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
    public async void Update<K>(K id, UserInfos input)
    {
        // Get the pupil
        var theUsers = await _context.UserInfos.FindAsync(id);
        theUsers.UserId = input.UserId;
        theUsers.UserName = input.UserName;
        theUsers.Password = input.Password;
    }
    public async Task<List<UserInfos>> GetAllAsync()
    {
        return await _context.UserInfos.ToListAsync();
    }
    public async Task<UserInfos?> GetById(string id)
    {
        return await _context.UserInfos.FirstOrDefaultAsync(x => x.UserId == id);
    }
    public async Task<UserInfos> GetByUserName(string userName)
    {
        return await _context.UserInfos.FirstOrDefaultAsync(u => u.UserName == userName);
    }

}