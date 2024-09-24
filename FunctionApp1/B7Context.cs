using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B7
{
    public class B7Context : DbContext
    {
        public B7Context(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Book> Books { get; set; }
    }
}
