using LoyaltyOne.Data;
using Moq;
using NUnit.Framework;

namespace LoyaltyOne.Services.Tests
{
    [TestFixture]
    public class TextServiceTest
    {
        public ITextService _textService;
        public Mock<ITextRepository> _textRepository;

        [SetUp]
        public void TestSetUp()
        {
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
    }
}
