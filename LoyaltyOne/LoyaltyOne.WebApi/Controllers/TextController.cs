using LoyaltyOne.WebApi.Models;
using LoyaltyOne.Services;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LoyaltyOne.WebApi.Controllers
{
    public class TextController : ApiController
    {
        private readonly ITextService _textService;

        /// <summary>
        /// Initializes a new instance of <see cref="TextController"/>.
        /// </summary>
        /// <param name="textService">The text service.</param>
        public TextController(ITextService textService)
        {
            if (textService == null) throw new ArgumentNullException("textService");

            _textService = textService;
        }

        [Route("v1/text/{text}")]
        [HttpGet]
        public virtual HttpResponseMessage GetText(string text)
        {
            GetTextResponse response = new GetTextResponse();

            try
            {
                response.Text = _textService.PingText(text);
            }
            catch (Exception ex)
            {
                response.Text = string.Empty;
                response.Error = string.Format("Internal server error: {0}", ex.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
