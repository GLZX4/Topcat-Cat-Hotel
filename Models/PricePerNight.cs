public class PricePerNight
{
    public int NumberOfCats { get; set; }
    public decimal Price { get; set; }

    public PricePerNight(int numberOfCats, decimal price)
    {
        NumberOfCats = numberOfCats;
        Price = price;
    }
}
