using Belong.SelfTours.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Belong.SelfTours.Domain.Repositories
{
    public interface IHomeRepository
    {
        Task<Home> GetAsync(string externalHomeId);
        Task UpdateAsync(Home home);
    }
}
