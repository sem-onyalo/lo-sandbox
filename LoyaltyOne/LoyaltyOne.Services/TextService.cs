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
        
        public string SaveText(string text)
        {
            TextDto textDto = new TextDto();
            textDto.Value = text;
            textDto.CreatedDateTimeUtc = DateTime.UtcNow;

            _textRepository.InsertText(textDto);

            return text;
        }
    }
}
