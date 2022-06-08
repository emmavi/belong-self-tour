namespace Belong.SelfToursAPI.Models
{
    public class SelftTourPutRequest
    {
        public int SelftTourId { get; set; }
        public int UserId { get; set; }
        public string HomeId { get; set; }
        public DateTime Slot { get; set; }
        public DateTime NewSlot { get; set; }
    }
}
