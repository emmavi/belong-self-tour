using System;

namespace Belong.SelfTours.Domain.Entities
{
    public class SelfTour : BaseEntity
    {
        public Home Home { get; set; }
        public int HomeId { get; set; }
        
        public DateTime Slot { get; set; }

        public int UserId { get; set; }
    }
}
