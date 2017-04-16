using LoyaltyOne.Data.Models;
using LoyaltyOne.Services;
using LoyaltyOne.Services.Models;
using LoyaltyOne.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Linq;
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
        public virtual HttpResponseMessage GetTextsByName(string name)
        {
            GetTextsByNameResponse response = new GetTextsByNameResponse();

            try
            {
                GetTextsResponse getTextsResponse = _textService.GetTexts(name);

                response.Name = name;
                response.Texts = new List<TextResponse>();

                foreach (TextDto textDto in getTextsResponse.Texts)
                {
                    CityDto cityDto = getTextsResponse.Cities.First(x => x.Name == textDto.CityName);

                    TextResponse textResponse = new TextResponse();
                    textResponse.Id = textDto.Id.ToString();
                    textResponse.ParentId = textDto.ParentId.ToString();
                    textResponse.Text = textDto.Value;
                    textResponse.City = cityDto.Name;
                    textResponse.Lat = cityDto.Latitude.ToString();
                    textResponse.Lon = cityDto.Longitude.ToString();
                    textResponse.Temp = cityDto.Temperature.ToString();

                    response.Texts.Add(textResponse);
                }
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

                SaveTextRequest saveTextRequest = new SaveTextRequest();
                saveTextRequest.City = request.City;
                saveTextRequest.Text = new TextDto();
                saveTextRequest.Text.Name = request.Name;
                saveTextRequest.Text.Value = request.Text;
                saveTextRequest.Text.ParentId = Convert.ToInt32(request.ParentId);

                SaveTextResponse saveTextResponse = _textService.SaveText(saveTextRequest);

                response.Data = new TextResponse();
                response.Data.Text = saveTextResponse.Text.Value;
                response.Data.Id = saveTextResponse.Text.Id.ToString();
                response.Data.ParentId = saveTextResponse.Text.ParentId.ToString();
                response.Data.City = saveTextResponse.City.Name;
                response.Data.Lat = saveTextResponse.City.Latitude.ToString();
                response.Data.Lon = saveTextResponse.City.Longitude.ToString();
                response.Data.Temp = saveTextResponse.City.Temperature.ToString();
            }
            catch (Exception ex)
            {
                response.Error = string.Format("Internal server error: {0}", ex.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
