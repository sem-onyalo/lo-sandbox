using System.Collections.Generic;

namespace LoyaltyOne.WebApi.LoadTests
{
    public class TextLoadTest
    {
        public string Name { get; set; }

        public int Threads { get; set; }

        public string ApiUri { get; set; }

        public IList<TextLoadTestData> Data { get; set; }
    }
}
