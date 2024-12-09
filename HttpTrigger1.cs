using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace MyFunction
{
    public class HttpTrigger1
    {
        private readonly ILogger<HttpTrigger1> _logger;

        public HttpTrigger1(ILogger<HttpTrigger1> logger)
        {
            _logger = logger;
        }

        [Function("HttpTrigger1")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req, FunctionContext executionContext)
        {
            string ipAddressString = "NOT FOUND";
            _logger.LogInformation("C# HTTP trigger function processed a request.");                     
            //Retrieve client IP
            ipAddressString = GetIpFromRequestHeaders(req,ipAddressString);
            return new OkObjectResult(ipAddressString);
        }

        private string GetIpFromRequestHeaders(HttpRequest request, string ipAddressString)
        {
            // fetching all the headers
            foreach (var header in request.Headers)
            {
                _logger.LogInformation($"Header Key: {header.Key}, Value: {string.Join(", ", header.Value)}");
            }
            
            ipAddressString = request.Headers["X-Forwarded-For"].FirstOrDefault() ?? ipAddressString;
            _logger.LogInformation($"X-Forwarded-For: {ipAddressString}");
            return ipAddressString?.Split(new char[] { ':' }).FirstOrDefault() ?? string.Empty;
        }

    }
}
