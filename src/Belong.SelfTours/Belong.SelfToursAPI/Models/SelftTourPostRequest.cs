namespace Belong.SelfToursAPI.Models
{
    public class SelftTourPostRequest
    {
        public int UserId { get; set; }
        public string HomeId { get; set; }
        public DateTime Slot { get; set; }
    }
}
