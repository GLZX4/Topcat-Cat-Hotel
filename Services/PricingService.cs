public class PricingService
{
    private readonly List<PricePerNight> _prices;

    public PricingService()
    {
        _prices = new List<PricePerNight>
        {
            new PricePerNight(1, 12.50m),
            new PricePerNight(2, 21.00m),
            new PricePerNight(3, 28.50m)
        };
    }

    public decimal GetPrice(int numberOfCats)
    {
        var pricePerNight = _prices.FirstOrDefault(p => p.NumberOfCats == numberOfCats);
        if (pricePerNight != null)
        {
            return pricePerNight.Price;
        }
        throw new ArgumentException("Invalid number of cats");
    }

    public List<PricePerNight> GetPrices()
    {
        return _prices;
    }
}
