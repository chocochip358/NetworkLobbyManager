using MySql.Data.Entity;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace GameLobbyServer.Models
{
    public class Room
    {
        public int ID { get; set; }
        public string HostPlayerName { get; set; }
        public string Description { get; set; }
        public int CurrentPlayer { get; set; }
        public int MaxPlayer { get; set; }
        public string HostIP { get; set; }

    }

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class RoomDBContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }

        public RoomDBContext() : base("name=MySqlConnection")
        {
            Database.SetInitializer<RoomDBContext>(new DropCreateDatabaseAlways<RoomDBContext>());
        }
    }
}