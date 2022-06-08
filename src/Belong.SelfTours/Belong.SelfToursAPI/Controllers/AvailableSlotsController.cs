using Belong.SelfTours.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Belong.SelfToursAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AvailableSlotsController : ControllerBase
    {
        private readonly ILogger<AvailableSlotsController> _logger;
        private readonly IHomeRepository _HomeRepo;
        private readonly ISelfTourRepository _SelfTourRepo;

        public AvailableSlotsController(ILogger<AvailableSlotsController> logger, IHomeRepository homeRepository, ISelfTourRepository selfTourRepository)
        {
            _logger = logger;
            this._HomeRepo = homeRepository;
            this._SelfTourRepo = selfTourRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DateTime>>> Get(string homeId)
        {
            var home = await _HomeRepo.GetAsync(homeId);
            //if (home is null || home.IsSelfServeVisitsAllowed == false) return NotFound();

            var busySlots = await _SelfTourRepo.GetBusySlotsAsync(home.Id);

            var availableSlots = GetAvailableSlots(busySlots);  

            return availableSlots;
        }


        /// <summary>
        /// Get the available slots base on the busy slots.
        /// 
        /// Tours can be booked in half-hour blocks, between 10 am to 5 pm on weekdays. Self-tours aren’t allowed on weekends.
        /// Tours cannot be booked for the same day, or for the next day if the booking is being made after 9 pm.
        /// </summary>
        /// <param name="busySlots"></param>
        /// <returns></returns>
        private static List<DateTime> GetAvailableSlots(List<DateTime> busySlots, int daysAhead = 3)
        {
            var availableSlots = new List<DateTime>();

            var day = DateTime.Now;

            //Trying to book after 9 PM it removes the posibility to book the next day.
            if (day.Day == DateTime.Now.Day && day.Hour >= 21)
                day = day.AddDays(2);
            else
                day = day.AddDays(1);

            int i = 0;
            while (i < daysAhead)
            {
                if (day.DayOfWeek == DayOfWeek.Saturday ||
                    day.DayOfWeek == DayOfWeek.Sunday)
                {
                    day = day.AddDays(1);
                    continue;
                }

                var startTime = new DateTime(day.Year, day.Month, day.Day, 9, 30, 0);

                //14 is the number of slots on a day
                for (int t = 0; t < 14; t++)
                {
                    startTime = startTime.AddMinutes(30);
                    if (busySlots.Contains(startTime))
                    {
                        //Adding the 30 minutes buffer
                        startTime = startTime.AddMinutes(30);
                        continue;
                    }
                    else if (busySlots.Contains(startTime.AddMinutes(30)))
                    {
                        //Skipping the hour. ex: 10:30 is taken, so it can't book 10:00, 10:30 and 11:00
                        startTime = startTime.AddHours(1);
                        continue;
                    }

                    availableSlots.Add(startTime);
                }

                day = day.AddDays(1);
                i++;
            }

            return availableSlots;
        }
    }
}