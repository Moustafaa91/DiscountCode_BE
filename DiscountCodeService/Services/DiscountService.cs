using DiscountCodeService.Data;
using DiscountCodeService.Models;
using DiscountCodeService.Utilities;
using dotenv.net;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscountCodeService.Services
{
    public class DiscountService
    {
        private readonly IMongoCollection<DiscountCode> _discountCodes;
        private readonly Logger _logger;

        public DiscountService(IMongoDatabase database, Logger logger)
        {
            _discountCodes = database.GetCollection<DiscountCode>("DiscountCodes");
            _logger = logger;
        }

        // Generate codes and save them to the database
        public async Task<bool> GenerateCodesAsync(int count, byte length, int discountPercentage)
        {
            if (count < 1 || count > 2000)
            {
                await _logger.LogError(ExceptionMessages.InvalidCount);
                throw new ArgumentException(ExceptionMessages.InvalidCount);
            }
            if (length != 7 && length != 8)
            {
                await _logger.LogError(ExceptionMessages.InvalidLength);
                throw new ArgumentException(ExceptionMessages.InvalidLength);
            }
            if (discountPercentage <= 0 || discountPercentage > 100)
            {
                await _logger.LogError(ExceptionMessages.InvalidDiscountPercentage);
                throw new ArgumentException(ExceptionMessages.InvalidDiscountPercentage);
            }

            using var session = await _discountCodes.Database.Client.StartSessionAsync();
            session.StartTransaction();

            try
            {
                var random = new Random();
                var codes = new List<DiscountCode>();

                for (int i = 0; i < count; i++)
                {
                    string code;
                    do
                    {
                        code = new string(Enumerable.Range(0, length)
                            .Select(_ => (char)random.Next('A', 'Z' + 1))
                            .ToArray());
                    }
                    while (await CodeExistsAsync(code)); // Ensure uniqueness

                    codes.Add(new DiscountCode(code, discountPercentage));
                }

                await _discountCodes.InsertManyAsync(codes);

                await session.CommitTransactionAsync();
                return true;
            }
            catch
            {
                await session.AbortTransactionAsync();
                return false;
            }
        }



        public async Task<bool> CodeExistsAsync(string code)
        {
            return await _discountCodes.Find(dc => dc.Code == code).AnyAsync();
        }

        public async Task<bool> UseCodeAsync(string code)
        {
            var update = Builders<DiscountCode>.Update
                .Set(dc => dc.IsUsed, true)
                .Set(dc => dc.UsedAt, DateTime.UtcNow);

            var result = await _discountCodes.UpdateOneAsync(
                dc => dc.Code == code && !dc.IsUsed, // Filter: code exists and is not already used
                update
            );

            return result.ModifiedCount == 1;
        }

        public async Task<List<DiscountCode>> GetUnusedCodesAsync()
        {
            return await _discountCodes.Find(dc => !dc.IsUsed).ToListAsync();
        }

        public async Task<List<DiscountCode>> GetUsedCodesAsync()
        {
            return await _discountCodes.Find(dc => dc.IsUsed).ToListAsync();
        }
    }

}