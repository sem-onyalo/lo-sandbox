using LoyaltyOne.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LoyaltyOne.WebApi.Controllers
{
    public class DefaultController : ApiController
    {
        private readonly ITextService _textService;

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultController"/>.
        /// </summary>
        /// <param name="textService">The text service.</param>
        public DefaultController(ITextService textService)
        {
            if (textService == null) throw new ArgumentNullException("textService");

            _textService = textService;
        }

        [HttpGet]
        public HttpResponseMessage Index()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "LoyaltyOne Web API");
        }

        [Route("v1/text/{text}")]
        [HttpGet]
        public virtual HttpResponseMessage PingText(string text)
        {
            HttpResponseMessage response;

            try
            {
                string result = _textService.PingText(text);

                response = Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                string error = string.Format("Internal server error: {0}", ex.Message);

                response = Request.CreateResponse(HttpStatusCode.OK, error);
            }

            return response;
        }
    }
}
