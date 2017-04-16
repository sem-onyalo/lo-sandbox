using LoyaltyOne.Data;
using LoyaltyOne.Data.Models;
using LoyaltyOne.Services.Models;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace LoyaltyOne.Services.Tests
{
    [TestFixture]
    public class TextServiceTest
    {
        public Mock<ICityRepository> _cityRepository;
        public Mock<ITextRepository> _textRepository;
        public Mock<ILocationService> _locationService;
        public ITextService _textService;

        [SetUp]
        public void TestSetUp()
        {
            _cityRepository = new Mock<ICityRepository>();
            _textRepository = new Mock<ITextRepository>();
            _locationService = new Mock<ILocationService>();
            _textService = new TextService(_cityRepository.Object, _textRepository.Object, _locationService.Object);
        }

        [Test]
        public void PingText_ShouldNotBeNull()
        {
            Assert.IsNotNull(_textService);
        }

        [Test]
        public void PingText_ShouldReturnTextPassedIn()
        {
            string text = "test";
            string actual = _textService.PingText(text);

            Assert.AreEqual(text, actual);
        }

        [Test]
        public void PingText_ShouldReturnEmptyStringIfNullPassedIn()
        {
            string actual = _textService.PingText(null);

            Assert.AreEqual("", actual);
        }
        
        [TestCase(null)]
        [TestCase("")]
        public void GetTexts_ShouldReturnEmptyList(string name)
        {
            _textRepository.Setup(x => x.SelectTextsByName(name)).Returns(new List<TextDto>());

            GetTextsResponse actual = _textService.GetTexts(name);

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Texts);
            Assert.AreEqual(0, actual.Texts.Count);
        }

        [Test]
        public void GetTexts_ShouldReturnTextsByName()
        {
            string name = "Joe";

            _textRepository.Setup(x => x.SelectTextsByName(name)).Returns(new List<TextDto>
            {
                new TextDto { Name = name, Value = "Hello", CityName = "London" },
                new TextDto { Name = name, Value = "how", CityName = "London" },
                new TextDto { Name = name, Value = "are", CityName = "London" },
                new TextDto { Name = name, Value = "you?", CityName = "Paris" }
            });

            List<string> cityNames = new List<string> { "London", "Paris" };
            _cityRepository.Setup(x => x.SelectCitiesByName(cityNames)).Returns(new List<CityDto>
            {
                new CityDto { Name = "London" },
                new CityDto { Name = "Paris" }
            });

            GetTextsResponse actual = _textService.GetTexts(name);

            Assert.AreEqual(4, actual.Texts.Count);
            Assert.AreEqual("Hello", actual.Texts[0].Value);
            Assert.AreEqual("how", actual.Texts[1].Value);
            Assert.AreEqual("are", actual.Texts[2].Value);
            Assert.AreEqual("you?", actual.Texts[3].Value);
        }

        [Test]
        public void GetTexts_ShouldReturnTextsInReplyOrder()
        {
            string name = "Joe";

            _textRepository.Setup(x => x.SelectTextsByName(name)).Returns(new List<TextDto>
            {
                new TextDto { Id = 1, ParentId = 0, Name = name, Value = "Test post 1", CityName = "London" },
                new TextDto { Id = 2, ParentId = 0, Name = name, Value = "Test post 2", CityName = "London" }
            });

            _textRepository.Setup(x => x.SelectTextsByParentIds(new List<int> { 1, 2 })).Returns(new List<TextDto>
            {
                new TextDto { Id = 3, ParentId = 1, Value = "Test reply 1-1", CityName = "London" },
                new TextDto { Id = 4, ParentId = 2, Value = "Test reply 2-1", CityName = "London" },
                new TextDto { Id = 5, ParentId = 1, Value = "Test reply 1-2", CityName = "London" }
            });

            List<string> cityNames = new List<string> { "London" };
            _cityRepository.Setup(x => x.SelectCitiesByName(cityNames)).Returns(new List<CityDto>
            {
                new CityDto { Name = "London" }
            });

            GetTextsResponse actual = _textService.GetTexts(name);

            Assert.AreEqual(5, actual.Texts.Count);
            Assert.AreEqual("Test post 1", actual.Texts[0].Value);
            Assert.AreEqual("Test reply 1-1", actual.Texts[1].Value);
            Assert.AreEqual("Test reply 1-2", actual.Texts[2].Value);
            Assert.AreEqual("Test post 2", actual.Texts[3].Value);
            Assert.AreEqual("Test reply 2-1", actual.Texts[4].Value);
        }
    }
}
