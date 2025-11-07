using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.UT
{
    public class AppForSEII2526SqliteUT
    {
        protected readonly DbConnection _connection;
        protected readonly ApplicationDbContext _context;
        protected readonly DbContextOptions<ApplicationDbContext> _contextOptions;

        // Si en algún test quieres un contexto "fresco"
        protected ApplicationDbContext CreateContext() => new(_contextOptions);

        // Importante: cierra la conexión cuando termine el test
        void Dispose() => _connection.Dispose();

        public AppForSEII2526SqliteUT()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new ApplicationDbContext(_contextOptions);

            if (_context.Database.EnsureCreated())
            {
                using var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = @"CREATE VIEW AllProducts AS SELECT Name FROM Products;";
                cmd.ExecuteNonQuery();
            }
        }
    }
}
