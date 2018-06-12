using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppClientes.DAL
{
    static class Database
    {
        internal static async Task InitializeDataAsync(DatabaseContext db)
        {
            // Auto generated Guid value in Composite Key:
            // https://github.com/aspnet/EntityFrameworkCore/issues/6958

            await db.Database.EnsureCreatedAsync();
        }
    }
}
