using System;
namespace DiplomskaNaloga.Settings
{
	public class MongoDbSettings
	{
        public const string Section = "MongoDbConnection";
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string CollectionName { get; set; } = null!;
    }
}

