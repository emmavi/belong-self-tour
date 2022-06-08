using Belong.SelfTours.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Belong.SelfTours.Domain.Repositories
{
    public interface ISelfTourRepository
    {
        Task<List<DateTime>> GetBusySlotsAsync(int homeId);
        Task<SelfTour> GetAsync(int selfTourId);
        Task<SelfTour> InsertAsync(SelfTour selfTour);
        Task UpdateAsync(SelfTour selfTour);
        Task DeleteAsync(SelfTour selfTour);
    }
}
