using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Newtonsoft.Json;
public class ForwardedForHeaderMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        await AddForwardedForHeaderValueAsync(context);

        await next(context);
    }

    private static async Task AddForwardedForHeaderValueAsync(FunctionContext context)
    {
        var val = ExtractForwardedForHeaderValue(context);
        var req = await context.GetHttpRequestDataAsync();
        req?.Headers.TryAddWithoutValidation(ForwardedHeadersDefaults.XForwardedForHeaderName, val);
    }

    private static string? ExtractForwardedForHeaderValue(FunctionContext context)
    {
        var hdr = context.BindingContext.BindingData["Headers"];
        dynamic obj = JsonConvert.DeserializeObject(hdr.ToString());

        var val = obj[ForwardedHeadersDefaults.XForwardedForHeaderName]?.Value as string;
        return val;
    }
}