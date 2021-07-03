using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EF.Manager
{
    public class MigrationHostedService : IHostedService
    {
        private IServiceProvider Services { get; }
        private IHostApplicationLifetime HostApplicationLifetime { get; }

        public MigrationHostedService(IServiceProvider services, IHostApplicationLifetime hostApplicationLifetime)
        {
            Services = services;
            HostApplicationLifetime = hostApplicationLifetime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var context = Services.GetRequiredService<AppDbContext>();

            var migrations = context.Database.GetMigrations().ToList();

            await context.Database.MigrateAsync(cancellationToken);

            HostApplicationLifetime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}