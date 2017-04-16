using LoyaltyOne.Data;
using LoyaltyOne.Data.Models;
using LoyaltyOne.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoyaltyOne.Services
{
    public class TextService : ITextService
    {
        private readonly ICityRepository _cityRepository;
        private readonly ITextRepository _textRepository;
        private readonly ILocationService _locationService;

        public TextService(ICityRepository cityRepository, ITextRepository textRepository, ILocationService locationService)
        {
            if (cityRepository == null) throw new ArgumentNullException("cityRepository");
            if (textRepository == null) throw new ArgumentNullException("textRepository");
            if (locationService == null) throw new ArgumentNullException("locationService");

            _cityRepository = cityRepository;
            _textRepository = textRepository;
            _locationService = locationService;
        }

        public string PingText(string text)
        {
            return text != null ? text : string.Empty;
        }

        public GetTextsResponse GetTexts(string name)
        {
            List<TextDto> texts = GetTextsByName(name);

            List<CityDto> cities = _cityRepository
                .SelectCitiesByName(texts.Select(x => x.CityName).Distinct().ToList())
                .ToList();

            GetTextsResponse response = new GetTextsResponse();
            response.Texts = texts;
            response.Cities = new List<CityDto>();

            foreach (TextDto text in texts)
            {
                if (!response.Cities.Select(x => x.Name).Contains(text.CityName))
                    response.Cities.Add(cities.First(x => x.Name == text.CityName));
            }

            return response;
        }
        
        public SaveTextResponse SaveText(SaveTextRequest request)
        {
            CityDto city = _locationService.GetCity(request.City);

            request.Text.CityName = city.Name;
            request.Text.CreatedDateTimeUtc = DateTime.UtcNow;
            _textRepository.InsertText(request.Text);

            SaveTextResponse response = new SaveTextResponse();
            response.City = city;
            response.Text = request.Text;

            return response;
        }

        private List<TextDto> GetTextsByName(string name)
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
    }
}
