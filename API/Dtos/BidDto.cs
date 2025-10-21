public class BidDto
{
    public int Id{ get; set; }
    public int HouseId{ get; set; }
    public string Bidder{ get; set; }
    public int Amount{ get; set; }

    public BidDto(int id, int houseId, string bidder, int amount)
    {
        this.Id = id;
        this.HouseId = houseId;
        this.Bidder = bidder;
        this.Amount = amount;
    }
}