using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Identity
{
    public class PersistedGrantDbContextFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
    {
        private readonly DbContextOptions<PersistedGrantDbContext> _options;

        public PersistedGrantDbContextFactory()
        {
            var opts = new DbContextOptionsBuilder<PersistedGrantDbContext>();
            opts.UseSqlServer(@"Server=(localdb)\\mssqllocaldb;Database=FullStackJobs;Trusted_Connection=True;MultipleActiveResultSets=true");
            _options = opts.Options;
        }

        public PersistedGrantDbContextFactory(DbContextOptions<PersistedGrantDbContext> options)
        {
            _options = options;
        }
        public PersistedGrantDbContext CreateDbContext(string[] args)
        {
            return new PersistedGrantDbContext(_options, new OperationalStoreOptions());
        }
    }
}
