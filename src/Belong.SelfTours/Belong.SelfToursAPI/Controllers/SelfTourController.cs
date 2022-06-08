using Belong.SelfTours.Domain.Entities;
using Belong.SelfTours.Domain.Repositories;
using Belong.SelfToursAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Belong.SelfToursAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SelfTourController : ControllerBase
    {
        private readonly ILogger<SelfTourController> _Logger;
        private readonly IHomeRepository _HomeRepo;
        private readonly ISelfTourRepository _SelfTourRepo;

        public SelfTourController(ILogger<SelfTourController> logger, IHomeRepository homeRepository, ISelfTourRepository selfTourRepository)
        {
            this._Logger = logger;
            this._HomeRepo = homeRepository;
            this._SelfTourRepo = selfTourRepository;
        }


        /// <summary>
        /// Book a slot for a self-tour.
        ///
        /// 
        ///Tours can be booked in half-hour blocks, between 10 am to 5 pm on weekdays.
        ///Self - tours aren’t allowed on weekends. 
        ///
        ///Tours cannot be booked for the same day, or for the next day if the booking
        ///is being made after 9 pm.
        ///
        ///No two people can book the same time slot, and each time slot will also
        ///include a half - hour buffer afterward. So, for example, if I book 3 pm,
        ///no one will be able to book 3:30 pm.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> BookTour([FromBody]SelftTourPostRequest request)
        {

            if (request.Slot.Hour < 10 || request.Slot.Hour >= 17)
                return BadRequest("You can only book between 10 AM and 9 PM");

            if (request.Slot.Day == DateTime.Now.Day && request.Slot.Hour >= 21)
                return BadRequest("Can't book for tomorrow, too late");

            if (request.Slot.DayOfWeek == DayOfWeek.Saturday || request.Slot.DayOfWeek == DayOfWeek.Sunday)
                return BadRequest("Can't book on weekends");
            
            var home = await _HomeRepo.GetAsync(request.HomeId);
            //if (home is null || home.IsSelfServeVisitsAllowed == false) return NotFound();

            var busySlots = await _SelfTourRepo.GetBusySlotsAsync(home.Id);

            var formatedSlot = new DateTime(request.Slot.Year, request.Slot.Month, request.Slot.Day, request.Slot.Hour, request.Slot.Minute, 0);
            var isATakenSlot = busySlots.Any(x => x.Date == formatedSlot || x.Date == formatedSlot.AddMinutes(-30));

            if (isATakenSlot)
                return BadRequest();

            var selfTour = await _SelfTourRepo.InsertAsync(new SelfTour()
            {
                UserId = request.UserId,
                HomeId = home.Id,
                Slot = formatedSlot
            });

            home.BookedTours++;
            await _HomeRepo.UpdateAsync(home);
            
            return Ok(selfTour.Id);
        }

        /// <summary>
        /// Booked tours can be rescheduled or canceled by the user until the tour start time. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> RescheduleTour([FromBody] SelftTourPutRequest request)
        {    
            var selfTour = await _SelfTourRepo.GetAsync(request.SelftTourId);
            if (selfTour is null) return NotFound();
            
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            if (now >= selfTour.Slot) return BadRequest("Self tour already started");

            var home = await _HomeRepo.GetAsync(request.HomeId);
            //if (home is null || home.IsSelfServeVisitsAllowed == false) return NotFound();

            var busySlots = await _SelfTourRepo.GetBusySlotsAsync(home.Id);

            var formatedSlot = new DateTime(request.NewSlot.Year, request.NewSlot.Month, request.NewSlot.Day, request.NewSlot.Hour, request.NewSlot.Minute, 0);
            var isATakenSlot = busySlots.Any(x => x.Date == now || x.Date == now.AddMinutes(-30));

            if (isATakenSlot)
                return BadRequest();

            selfTour.Slot = formatedSlot;
            await _SelfTourRepo.UpdateAsync(selfTour);

            home.RescheduledTours++;
            await _HomeRepo.UpdateAsync(home);
            
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> CancelTour(int selftTourId)
        {
            var selfTour = await _SelfTourRepo.GetAsync(selftTourId);
            if (selfTour is null) return NotFound();

            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            if (now >= selfTour.Slot) return BadRequest("Self tour already started");


            var home = await _HomeRepo.GetAsync(selfTour.Home.ExternalHomeId);
            
            await _SelfTourRepo.DeleteAsync(selfTour);

            home.CanceledTours++;
            await _HomeRepo.UpdateAsync(home);
            
            return Ok();
        }
    }
}