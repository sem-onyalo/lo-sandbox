using LoyaltyOne.Data;
using LoyaltyOne.Services;
using LoyaltyOne.Services.Models;
using LoyaltyOne.WebApi.Controllers;
using LoyaltyOne.WebApi.Models;
using NUnit.Framework;
using System;
using System.IO;
using System.Net.Http;
using System.Web.Http;

namespace LoyaltyOne.WebApi.Tests
{
    [TestFixture]
    public class TextControllerTest
    {
        private string _dbConnStr;
        private ICityRepository _cityRepository;
        private ITextRepository _textRepository;

        private IApiService _apiService;
        private ILocationService _locationService;
        private ITextService _textService;

        private TextController _textController;

        [OneTimeSetUp]
        public void TestFixtureSetUp()
        {
            Uri dbConnStrUri = new Uri(string.Format("{0}\\{1}", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), "LoyaltyOne.db"));

            _dbConnStr = dbConnStrUri.LocalPath;

            if (File.Exists(_dbConnStr))
                File.Delete(_dbConnStr);
        }

        [SetUp]
        public void TestSetUp()
        {
            _cityRepository = new CityRepository(_dbConnStr);
            _textRepository = new TextRepository(_dbConnStr);
            _apiService = new ApiService();
            _locationService = new LocationService(_cityRepository, _apiService);
            _textService = new TextService(_cityRepository, _textRepository, _locationService);

            _textController = new TextController(_textService);
            _textController.Request = new HttpRequestMessage();
            _textController.Configuration = new HttpConfiguration();
        }
        
        [Test]
        public void TextController_SinglePostTextIntegrationTest()
        {
            string city = "Toronto";
            string text = "Hello";
            string name = "Joe1";

             HttpResponseMessage httpResponse = _textController.PostText(new PostTextRequest
            {
                City = city,
                Name = name,
                ParentId = "0",
                Text = text
            });

            PostTextResponse postTextResponse;

            Assert.IsTrue(httpResponse.TryGetContentValue(out postTextResponse));
            Assert.IsTrue(Convert.ToInt32(postTextResponse.Data.Id) > 0);
            Assert.AreEqual(city, postTextResponse.Data.City);
            Assert.AreEqual(text, postTextResponse.Data.Text);
            Assert.IsNotNull(postTextResponse.Data.Lat);
            Assert.IsNotNull(postTextResponse.Data.Lon);
            Assert.IsNotNull(postTextResponse.Data.Temp);
        }

        [Test]
        public void TextController_SinglePostAndGetTextIntegrationTest()
        {
            string name = "Joe2";

            _textController.PostText(new PostTextRequest
            {
                City = "Toronto",
                Name = name,
                ParentId = "0",
                Text = "Hello"
            });

            HttpResponseMessage httpResponse = _textController.GetTextsByName(name);

            GetTextsByNameResponse getTextsByNameResponse;

            Assert.IsTrue(httpResponse.TryGetContentValue(out getTextsByNameResponse));
            Assert.AreEqual(1, getTextsByNameResponse.Texts.Count);
            Assert.AreEqual("Hello", getTextsByNameResponse.Texts[0].Text);
        }

        [Test]
        public void TextController_MultiplePostAndGetTextIntegrationTest()
        {
            string name = "Joe3";

            _textController.PostText(new PostTextRequest
            {
                City = "Toronto",
                Name = name,
                ParentId = "0",
                Text = "Hello"
            });

            _textController.PostText(new PostTextRequest
            {
                City = "Toronto",
                Name = name,
                ParentId = "0",
                Text = "World"
            });

            HttpResponseMessage httpResponse = _textController.GetTextsByName(name);

            GetTextsByNameResponse getTextsByNameResponse;

            Assert.IsTrue(httpResponse.TryGetContentValue(out getTextsByNameResponse));
            Assert.AreEqual(2, getTextsByNameResponse.Texts.Count);
            Assert.AreEqual("Hello", getTextsByNameResponse.Texts[0].Text);
            Assert.AreEqual("World", getTextsByNameResponse.Texts[1].Text);
        }

        [Test]
        public void TextController_SinglePostAndGetReplyTextIntegrationTest()
        {
            string name = "Joe4";

            HttpResponseMessage httpResponse = _textController.PostText(new PostTextRequest
            {
                City = "Toronto",
                Name = name,
                ParentId = "0",
                Text = "Hello"
            });

            PostTextResponse postTextResponse;
            httpResponse.TryGetContentValue(out postTextResponse);

            // reply
            _textController.PostText(new PostTextRequest
            {
                City = "Toronto",
                ParentId = postTextResponse.Data.Id,
                Text = "hi"
            });

            httpResponse = _textController.GetTextsByName(name);

            GetTextsByNameResponse getTextsByNameResponse;

            Assert.IsTrue(httpResponse.TryGetContentValue(out getTextsByNameResponse));
            Assert.AreEqual(2, getTextsByNameResponse.Texts.Count);
            Assert.AreEqual("Hello", getTextsByNameResponse.Texts[0].Text);
            Assert.AreEqual("hi", getTextsByNameResponse.Texts[1].Text);
            Assert.IsTrue(getTextsByNameResponse.Texts[0].Id == getTextsByNameResponse.Texts[1].ParentId);
        }

        [Test]
        public void TextController_MultiplePostAndGetReplyTextIntegrationTest()
        {
            string name = "Joe5";

            HttpResponseMessage httpResponse = _textController.PostText(new PostTextRequest
            {
                City = "Toronto",
                Name = name,
                ParentId = "0",
                Text = "Hello"
            });

            PostTextResponse postTextResponse;
            httpResponse.TryGetContentValue(out postTextResponse);

            // replies
            _textController.PostText(new PostTextRequest
            {
                City = "Toronto",
                ParentId = postTextResponse.Data.Id,
                Text = "hi"
            });

            _textController.PostText(new PostTextRequest
            {
                City = "Ottawa",
                ParentId = postTextResponse.Data.Id,
                Text = "howdy"
            });

            httpResponse = _textController.GetTextsByName(name);

            GetTextsByNameResponse getTextsByNameResponse;

            Assert.IsTrue(httpResponse.TryGetContentValue(out getTextsByNameResponse));
            Assert.AreEqual(3, getTextsByNameResponse.Texts.Count);
            Assert.AreEqual("Hello", getTextsByNameResponse.Texts[0].Text);
            Assert.AreEqual("hi", getTextsByNameResponse.Texts[1].Text);
            Assert.AreEqual("howdy", getTextsByNameResponse.Texts[2].Text);
            Assert.IsTrue(getTextsByNameResponse.Texts[0].Id == getTextsByNameResponse.Texts[1].ParentId);
            Assert.IsTrue(getTextsByNameResponse.Texts[0].Id == getTextsByNameResponse.Texts[2].ParentId);
        }
    }
}
