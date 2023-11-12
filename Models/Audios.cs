using SQLite;

namespace Ejer2_3.Models
{
    public class Audios
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string url { get; set; }
        public string descripcion { get; set; }
        public DateTime fecha { get; set; }
    }
}
