using LoyaltyOne.Data;
using LoyaltyOne.Data.Models;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace LoyaltyOne.Services.Tests
{
    [TestFixture]
    public class TextServiceTest
    {
        public Mock<ITextRepository> _textRepository;
        public ITextService _textService;

        [SetUp]
        public void TestSetUp()
        {
            _textRepository = new Mock<ITextRepository>();
            _textService = new TextService(_textRepository.Object);
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

            IList<TextDto> actual = _textService.GetTexts(name);

            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        [Test]
        public void GetTexts_ShouldReturnTextsByName()
        {
            string name = "Joe";

            _textRepository.Setup(x => x.SelectTextsByName(name)).Returns(new List<TextDto>
            {
                new TextDto { Name = name, Value = "Hello" },
                new TextDto { Name = name, Value = "how" },
                new TextDto { Name = name, Value = "are" },
                new TextDto { Name = name, Value = "you?" }
            });

            IList<TextDto> actual = _textService.GetTexts(name);

            Assert.AreEqual(4, actual.Count);
            Assert.AreEqual("Hello", actual[0].Value);
            Assert.AreEqual("how", actual[1].Value);
            Assert.AreEqual("are", actual[2].Value);
            Assert.AreEqual("you?", actual[3].Value);
        }

        [Test]
        public void GetTexts_ShouldReturnTextsInReplyOrder()
        {
            string name = "Joe";

            _textRepository.Setup(x => x.SelectTextsByName(name)).Returns(new List<TextDto>
            {
                new TextDto { Id = 1, ParentId = 0, Name = name, Value = "Test post 1" },
                new TextDto { Id = 2, ParentId = 0, Name = name, Value = "Test post 2" }
            });

            _textRepository.Setup(x => x.SelectTextsByParentIds(new List<int> { 1, 2 })).Returns(new List<TextDto>
            {
                new TextDto { Id = 3, ParentId = 1, Value = "Test reply 1-1" },
                new TextDto { Id = 4, ParentId = 2, Value = "Test reply 2-1" },
                new TextDto { Id = 5, ParentId = 1, Value = "Test reply 1-2" }
            });

            IList<TextDto> actual = _textService.GetTexts(name);

            Assert.AreEqual(5, actual.Count);
            Assert.AreEqual("Test post 1", actual[0].Value);
            Assert.AreEqual("Test reply 1-1", actual[1].Value);
            Assert.AreEqual("Test reply 1-2", actual[2].Value);
            Assert.AreEqual("Test post 2", actual[3].Value);
            Assert.AreEqual("Test reply 2-1", actual[4].Value);
        }
    }
}
