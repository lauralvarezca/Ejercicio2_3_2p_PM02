using Ejer2_3.Models;
using SQLite;

namespace Ejer2_3.Controllers
{
    public class BDAudio
    {
        SQLiteAsyncConnection db;

        public BDAudio(string pathdb)
        {
            db = new SQLiteAsyncConnection(pathdb);
            db.CreateTableAsync<Audios>().Wait();
        }

        public Task<List<Audios>> ListaAudios()
        {
            return db.Table<Audios>().ToListAsync();
        }

        public Task<Audios> ReproducirAudio(int pid)
        {
            return db.Table<Audios>().Where(i => i.id == pid).FirstOrDefaultAsync();
        }

        public Task<int> GrabarAudio(Audios audi)
        {
            if (audi.id != 0)
            {
                return db.UpdateAsync(audi);
            }
            else
            {
                return db.InsertAsync(audi);
            }
        }

        public async Task<bool> EliminarAudio(Audios audi)
        {
            try
            {
                int result = await db.DeleteAsync(audi);
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el audio: {ex.Message}");
                return false;
            }
        }
    }
}
