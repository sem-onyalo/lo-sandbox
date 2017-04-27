using System;
using System.IO;

namespace LoyaltyOne.Data
{
    public abstract class Repository
    {
        protected string ConnectionString { get; private set; }

        public Repository()
        {
            Uri uri = new Uri(string.Format("{0}\\{1}", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), "LoyaltyOne.db"));

            ConnectionString = uri.LocalPath;
        }

        public Repository(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
