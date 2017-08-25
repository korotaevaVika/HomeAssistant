using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace HomeAssistant.Data.Context
{
    public interface IHomeAssistantContext: IDisposable
    {
        Database Database { get; }
        
        //        DbSet<Setting> Settings { get; set; }

        LoginInfo Login(string login, string password);
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}