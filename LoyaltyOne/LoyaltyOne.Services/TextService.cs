using LoyaltyOne.Data;
using LoyaltyOne.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoyaltyOne.Services
{
    public class TextService : ITextService
    {
        private readonly ITextRepository _textRepository;

        public TextService(ITextRepository textRepository)
        {
            if (textRepository == null) throw new ArgumentNullException("textRepository");

            _textRepository = textRepository;
        }

        public string PingText(string text)
        {
            return text != null ? text : string.Empty;
        }

        public IList<string> GetTexts(string name)
        {
            List<string> texts = _textRepository
                .SelectTextsByName(name)
                .Select(x => x.Value)
                .ToList();

            return texts;
        }
        
        public TextDto SaveText(TextDto text)
        {
            text.CreatedDateTimeUtc = DateTime.UtcNow;

            _textRepository.InsertText(text);

            return text;
        }
    }
}
