using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RecAPI.Models;

namespace RecAPI.Tests
{
    public class SqliteInMemoryIngredientControllerTest : IngredientControllerTests, IDisposable
    {
        private readonly DbConnection dbConnection;

        public SqliteInMemoryIngredientControllerTest()
            : base(
                  new DbContextOptionsBuilder<RecAPIContext>()
                  .UseSqlite(CreateInMemoryDatabase())
                  .Options)
        {
            dbConnection = RelationalOptionsExtension.Extract(ContextOptions).Connection;
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }

        public void Dispose() => dbConnection.Dispose();
    }
}
