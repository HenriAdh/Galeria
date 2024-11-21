using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoDAM
{
    public class PhotosDatabase
    {
        SQLiteAsyncConnection Database;

        public PhotosDatabase() { }

        async Task Init()
        {
            if (Database is not null)
                return;

            Console.WriteLine(Constants.DatabasePath);

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await Database.CreateTableAsync<Photos>();
        }

        public async Task<List<Photos>> GetPhotosAsync()
        {
            await Init();
            return await Database.Table<Photos>().ToListAsync();
        }

        public async Task<List<Photos>> GetPhotosWithPaginationAsync(int page, int take = 10)
        {
            await Init();
            return await Database.Table<Photos>().Skip(page).Take(take).ToListAsync();
        }

        public async Task<Photos> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<Photos>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(Photos item)
        {
            await Init();
            if (item.Id != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(Photos item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
