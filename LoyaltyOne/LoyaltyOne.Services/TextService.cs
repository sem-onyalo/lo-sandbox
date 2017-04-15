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

        public IList<TextDto> GetTexts(string name)
        {
            List<TextDto> textsByName = _textRepository
                .SelectTextsByName(name)
                .ToList();

            List<int> parentIds = textsByName.Select(x => x.Id).Distinct().ToList();

            List<TextDto> textsByParentIds = _textRepository
                .SelectTextsByParentIds(parentIds)
                .ToList();

            List<TextDto> texts = new List<TextDto>();
            foreach (TextDto text in textsByName)
            {
                texts.Add(text);

                if (textsByParentIds.Select(x => x.ParentId).Contains(text.Id))
                {
                    foreach (TextDto childText in textsByParentIds.Where(x => x.ParentId == text.Id))
                    {
                        texts.Add(childText);
                    }
                }
            }

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
