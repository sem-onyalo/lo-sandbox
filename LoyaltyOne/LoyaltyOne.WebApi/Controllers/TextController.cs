using LoyaltyOne.Data.Models;
using LoyaltyOne.Services;
using LoyaltyOne.WebApi.Models;
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

        [Route("v1/texts/{name}")]
        [HttpGet]
        public virtual HttpResponseMessage GetTexts(string name)
        {
            GetTextsResponse response = new GetTextsResponse();

            try
            {
                response.Name = name;
                response.Texts = _textService.GetTexts(name);
            }
            catch (Exception ex)
            {
                response.Name = name;
                response.Error = string.Format("Internal server error: {0}", ex.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("v1/text")]
        [HttpPost]
        public virtual HttpResponseMessage PostText(PostTextRequest request)
        {
            PostTextResponse response = new PostTextResponse();

            try
            {
                if (request == null) throw new ArgumentException("Request content is invalid");

                TextDto textDto = _textService.SaveText(new TextDto
                {
                    Name = request.Name,
                    Value = request.Text
                });

                response.Text = textDto.Value;
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
