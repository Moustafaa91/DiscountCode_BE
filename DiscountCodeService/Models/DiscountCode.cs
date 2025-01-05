using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


namespace DiscountCodeService.Models
{
    public class DiscountCode
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Code")]
        public string Code { get; set; }

        [BsonElement("IsUsed")]
        public bool IsUsed { get; set; }

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("UsedAt")]
        public DateTime? UsedAt { get; set; }

        [BsonElement("DiscountPercentage")]
        public int DiscountPercentage { get; set; } 

        
        public DiscountCode(string code, int discountPercentage)
        {
            Code = code;
            IsUsed = false;
            CreatedAt = DateTime.UtcNow;
            UsedAt = null;
            DiscountPercentage = discountPercentage;
        }
    }
}
