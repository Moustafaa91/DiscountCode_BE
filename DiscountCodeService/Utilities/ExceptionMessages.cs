namespace DiscountCodeService.Utilities
{
    public class ExceptionMessages
    {
        public const string InvalidCount = "Count must be between 1 and 2000.";
        public const string InvalidLength = "Length must be either 7 or 8 characters.";
        public const string InvalidDiscountPercentage = "Discount percentage must be between 1 and 100.";
        public const string MongoDBNotConfigured = "MongoDB connection string, database name, or collection name is not configured.";
    }
}
