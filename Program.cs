using Microsoft.Extensions.Hosting;

var builder = new HostBuilder();

var host = builder.ConfigureFunctionsWebApplication(workerApplication =>
{
    workerApplication.UseMiddleware<ForwardedForHeaderMiddleware>();
}).Build();

host.Run();
