using ElasticEmbeddings.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ElasticEmbeddings.Jobs;

public class DocumentProcessingJob(IServiceProvider serviceProvider) : BackgroundService
{
    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            
            var documentProcessingService = scope.ServiceProvider.GetRequiredService<IDocumentProcessingService>();
                
            await documentProcessingService.ProcessDocumentBatchAsync(stoppingToken);
            await Task.Delay(Delay, stoppingToken);
        }
    }
}