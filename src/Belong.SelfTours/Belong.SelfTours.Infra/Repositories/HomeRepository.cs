using Belong.SelfTours.Domain.Entities;
using Belong.SelfTours.Domain.Repositories;
using Belong.SelfTours.Infra.Proxies.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belong.SelfTours.Infra.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly SelfTourDbContext _SelfTourDbContext;
        private readonly IHomeProxy _HomeProxy;

        public HomeRepository(SelfTourDbContext selfTourDbContext, IHomeProxy homeProxy)
        {
            this._SelfTourDbContext = selfTourDbContext ?? throw new ArgumentNullException(nameof(selfTourDbContext));
            this._HomeProxy = homeProxy ?? throw new ArgumentNullException(nameof(homeProxy));
        }

        public async Task<Home> GetAsync(string externalHomeId)
        {
            var home = await _SelfTourDbContext.Homes.FirstOrDefaultAsync(x => x.ExternalHomeId == externalHomeId);

            //If not in db, check with external service and add to db
            if (home is null)
            {
                var homeIsSelfServiceAllowed = await _HomeProxy.IsSelfServiceAllowedAsync(externalHomeId);

                if (homeIsSelfServiceAllowed.HasValue)
                {
                    home = new Home()
                    {
                        ExternalHomeId = externalHomeId,
                        IsSelfServeVisitsAllowed = homeIsSelfServiceAllowed.Value
                    };

                    _SelfTourDbContext.Homes.Add(home);
                    await _SelfTourDbContext.SaveChangesAsync();
                }
            }

            return home;
        }

        public Task UpdateAsync(Home home)
        {
            throw new NotImplementedException();
        }
    }
}
