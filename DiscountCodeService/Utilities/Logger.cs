namespace DiscountCodeService.Utilities
{
    using Microsoft.Extensions.Logging.Abstractions;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using System;

    public class Logger
    {
        private readonly IMongoCollection<LogEntry> _logCollection;

        public Logger(IMongoDatabase database)
        {
            _logCollection = database.GetCollection<LogEntry>("Logs");
        }

        public async Task LogInfo(string message)
        {
            await _logCollection.InsertOneAsync(new LogEntry
            {
                Level = "Info",
                Message = message,
                Timestamp = DateTime.UtcNow
            });
        }

        public async Task LogError(string message, Exception ex = null)
        {

            await _logCollection.InsertOneAsync(new LogEntry
            {
                Level = "Error",
                Message = message,
                Exception = ex == null || ex.InnerException == null ? String.Empty : ex.InnerException.ToString(),
                Timestamp = DateTime.UtcNow
            });
        }

        private class LogEntry
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; }
            
            [BsonElement("Level")]
            public string Level { get; set; }

            [BsonElement("Message")]
            public string Message { get; set; }

            [BsonElement("Exception")]
            public string Exception { get; set; }

            [BsonElement("Timestamp")]
            public DateTime Timestamp { get; set; }
        }
    }

}
