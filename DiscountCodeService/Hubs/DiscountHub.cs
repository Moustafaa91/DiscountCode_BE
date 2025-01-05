using DiscountCodeService.Models;
using DiscountCodeService.Services;
using DiscountCodeService.Utilities;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DiscountCodeService.Hubs
{
    public class DiscountHub : Hub
    {
        private readonly DiscountService _discountService;
        private readonly Logger _logger;

        public DiscountHub(DiscountService discountService, Logger logger)
        {
            Console.WriteLine("DiscountHub instantiated.");
            _discountService = discountService;
            _logger = logger;
        }

        // Just for checking backend is working.
        public async Task<string> Ping()
        {
            await _logger.LogInfo($"Ping method invoked by client: {Context?.ConnectionId}");
            return "Hello, world! Backend is up and running.";
        }


        public async Task GenerateCodes(ushort count, byte length)
        {
            try
            {
                var result = await _discountService.GenerateCodesAsync(count, length, new Random().Next(1, 100));

                await Clients.Caller.SendAsync("ReceiveGeneratedCodesResult", true);
                await _logger.LogInfo($"Generated {count} codes of length {length}.");
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ReceiveGeneratedCodesResult", false);
                await _logger.LogError("Error in GenerateCodes", ex);
            }

        }

        public async Task UseCode(string code)
        {
            try
            {
                var result = await _discountService.UseCodeAsync(code); 
                await Clients.Caller.SendAsync("ReceiveCodeUsageResult", result);
                if (result)
                {
                    await _logger.LogInfo($"Successfully used code: {code}");
                }
                else
                {
                    await _logger.LogInfo($"Failed to use code: {code} (already used or does not exist).");
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ReceiveCodeUsageResult", false);
                await _logger.LogError("Error in UseCode", ex);
            }
        }

        public async Task<List<DiscountCode>> GetUsedCodes()
        {
            var usedCodes = await _discountService.GetUsedCodesAsync();
            return usedCodes;
        }

        public async Task<List<DiscountCode>> GetUnusedCodes()
        {
            var unusedCodes = await _discountService.GetUnusedCodesAsync();
            return unusedCodes;
        }

    }

}