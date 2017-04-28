using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LoyaltyOne.WebApi.Controllers
{
    public class DefaultController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Index()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            
            string response = string.Format("LoyaltyOne Web API v{0}\r\n\r\nDocumentation: http://docs.test13325.apiary.io", fvi.FileVersion);
            HttpResponseMessage httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
            httpResponse.Content = new StringContent(response, System.Text.Encoding.UTF8, "text/plain");

            return httpResponse;
        }
    }
}
