using Belong.SelfTours.Domain.Entities;
using Belong.SelfTours.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belong.SelfTours.Infra.Repositories
{
    public class SelfTourRepository : ISelfTourRepository
    {
        private readonly SelfTourDbContext selfTourDbContext;

        public SelfTourRepository(SelfTourDbContext selfTourDbContext)
        {
            this.selfTourDbContext = selfTourDbContext ?? throw new ArgumentNullException(nameof(selfTourDbContext));
        }
        public async Task<SelfTour> GetAsync(int selfTourId)
        {
            return await selfTourDbContext.SelfTours.Include(x => x.Home).FirstOrDefaultAsync(x => x.Id == selfTourId);
        }

        public async Task<List<DateTime>> GetBusySlotsAsync(int homeId)
        {
            return await selfTourDbContext.SelfTours.Where(x => x.Home.Id == homeId)
                                            .Select(x => x.Slot)
                                            .ToListAsync();
        }


        public async Task<SelfTour> InsertAsync(SelfTour selfTour)
        {
            selfTourDbContext.SelfTours.Add(selfTour);
            await selfTourDbContext.SaveChangesAsync();

            return selfTour;
        }

        public async Task UpdateAsync(SelfTour selfTour)
        {
            selfTourDbContext.SelfTours.Attach(selfTour);
            selfTourDbContext.Entry(selfTour).State = EntityState.Modified;

            await selfTourDbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(SelfTour selfTour)
        {
            selfTourDbContext.SelfTours.Remove(selfTour);
            await selfTourDbContext.SaveChangesAsync();
        }

    }
}
