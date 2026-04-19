using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HealthKeeper;

public class DatabaseService
{
    private SQLiteAsyncConnection _database;

    
    async Task Init()
    {
        
        if (_database is not null)
            return;

       
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "HealthKeeper.db3");

        _database = new SQLiteAsyncConnection(dbPath);

        
        await _database.CreateTableAsync<HealthTask>();
    }

   
    public async Task<List<HealthTask>> GetTasksAsync()
    {
        await Init();
        return await _database.Table<HealthTask>().ToListAsync();
    }

    public async Task<int> SaveTaskAsync(HealthTask task)
    {
        await Init();
        
        if (task.Id != 0)
            return await _database.UpdateAsync(task);
        else
            return await _database.InsertAsync(task);
    }

    
    public async Task<int> DeleteTaskAsync(HealthTask task)
    {
        await Init();
        return await _database.DeleteAsync(task);
    }
}