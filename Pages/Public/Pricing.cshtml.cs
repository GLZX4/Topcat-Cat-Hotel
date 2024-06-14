using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Topcat_Cat_Hotel.Pages.Public
{
    public class PricingModel : PageModel
    {
        private readonly PricingService _pricingService;
        public List<PricePerNight> prices { get; private set; }

        public PricingModel(PricingService pricingService)
        {
            _pricingService = pricingService;
        }

        public void OnGet()
        {
            prices = _pricingService.GetPrices();   
        }
    }
}
