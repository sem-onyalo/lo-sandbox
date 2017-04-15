using LoyaltyOne.Data.Models;
using LoyaltyOne.Services;
using LoyaltyOne.WebApi.Models;
using System;
using System.Collections.Generic;
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
                IList<TextDto> textDtos = _textService.GetTexts(name);

                response.Name = name;
                response.Texts = new List<TextResponse>();

                foreach (TextDto textDto in textDtos)
                    response.Texts.Add(new TextResponse
                    {
                        Id = textDto.Id.ToString(),
                        ParentId = textDto.ParentId.ToString(),
                        Text = textDto.Value
                    });
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
                    Value = request.Text,
                    ParentId = Convert.ToInt32(request.ParentId)
                });

                response.Text = textDto.Value;
                response.Id = textDto.Id.ToString();
                response.ParentId = textDto.ParentId.ToString();
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
