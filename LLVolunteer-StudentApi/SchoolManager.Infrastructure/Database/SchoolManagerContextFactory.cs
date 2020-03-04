using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SchoolManager.Infrastructure.Database
{
    public class SchoolManagerContextFactory: IDesignTimeDbContextFactory<SchoolManagerContext>
    {
        public SchoolManagerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SchoolManagerContext>();
            optionsBuilder.UseMySql("server=localhost;database=VolunteerServices.SchoolManager;port=3306;user=root;password=12345678;sslmode=none");

            return new SchoolManagerContext(optionsBuilder.Options);
        }
    }
}
