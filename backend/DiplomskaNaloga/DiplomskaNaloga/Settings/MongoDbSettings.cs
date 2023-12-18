using System;
namespace DiplomskaNaloga.Settings
{
	public class MongoDbSettings
	{
        public const string Section = "MongoDbConnection";
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string CollectionDetailsName { get; set; } = null!;
        public string CollectionUsersName { get; set; } = null!;
        public string CollectionGroupName { get; set; } = null!;
    }
}

