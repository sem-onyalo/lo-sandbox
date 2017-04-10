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

            IList<string> actual = _textService.GetTexts(name);

            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        [Test]
        public void GetTexts_ShouldReturnTextsByName()
        {
            string name = "Joe";

            _textRepository.Setup(x => x.SelectTextsByName(name)).Returns(new List<TextDto>
            {
                new TextDto {  Name = "Joe", Value = "Hello" },
                new TextDto {  Name = "Joe", Value = "how" },
                new TextDto {  Name = "Joe", Value = "are" },
                new TextDto {  Name = "Joe", Value = "you?" }
            });

            IList<string> actual = _textService.GetTexts(name);

            Assert.AreEqual(4, actual.Count);
            Assert.AreEqual("Hello", actual[0]);
            Assert.AreEqual("how", actual[1]);
            Assert.AreEqual("are", actual[2]);
            Assert.AreEqual("you?", actual[3]);
        }
    }
}
