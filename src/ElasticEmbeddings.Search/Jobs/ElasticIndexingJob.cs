﻿using ElasticEmbeddings.Search.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ElasticEmbeddings.Search.Jobs;

public class ElasticIndexingJob(IServiceProvider serviceProvider) : BackgroundService
{
    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            
            var documentIndexingService = scope.ServiceProvider.GetRequiredService<IDocumentIndexingService>();
                
            await documentIndexingService.ExecuteAsync(stoppingToken);
            await Task.Delay(Delay, stoppingToken);
        }
    }
}