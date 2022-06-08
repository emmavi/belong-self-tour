using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belong.SelfTours.Infra.Proxies.Responses
{
    public class HomeResponse
    {
        public HomeListingInfo listingInfo { get; set; }
    }
    public class HomeListingInfo
    {
        public bool isSelfServeVisitsAllowed { get; set; }
    }
}
