using System;
using System.Collections.Generic;
using System.Text;

namespace Belong.SelfTours.Domain.Entities
{
    public class Home : BaseEntity
    {
        public string ExternalHomeId { get; set; }
        public bool IsSelfServeVisitsAllowed { get; set; }
        public IEnumerable<SelfTour> SelfTours { get; set; }

        public int BookedTours { get; set; }
        public int CanceledTours { get; set; }
        public int RescheduledTours { get; set; }
    }
}
