using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
  public static class hello
  {
    [Function("hello")]
    public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
      HttpRequestData req, ILogger log)
    {
      log.LogInformation("C# HTTP trigger function processed a request.");

      var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
      string name = query["name"] ?? "";

      string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
      dynamic data = JsonConvert.DeserializeObject(requestBody);
      name = name ?? data?.name ?? "";

      string responseMessage = string.IsNullOrEmpty(name)
          ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
          : $"Hello {name} {Environment.GetEnvironmentVariable("FUNCTIONS_WORKER_RUNTIME")} " +
              $"Environment variable: {Environment.GetEnvironmentVariable("VARIABLE_FOO")}";

      var response = req.CreateResponse(HttpStatusCode.OK);
      await response.WriteAsJsonAsync(responseMessage, HttpStatusCode.OK);
      return response;
    }
  }
}
