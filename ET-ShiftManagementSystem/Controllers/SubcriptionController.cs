using AutoMapper;
using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Servises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ET_ShiftManagementSystem.Controllers
{
    [ApiController]
    [Route("api/subscriptions")]
    public class SubcriptionController : Controller
    {
        private readonly ISubscriptionRepo _subscription;
        private readonly IMapper mapper;

        public SubcriptionController(ISubscriptionRepo subscription, IMapper mapper)
        {
            this._subscription = subscription;
            this.mapper = mapper;
        }
        // GET: api/subscriptions
        [HttpGet]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetSubscriptions()
        {
            var Subs = await _subscription.GetSubscriptionAsync();

            if (Subs == null)
            {
                return NotFound();
            }
            
            var SubsDTO = mapper.Map<List<Models.SuscriptionDTO>>(Subs);

            return Ok(SubsDTO);

        }

        // GET: api/subscriptions/5
        [HttpGet("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetSubscription(int id)
        {
            var subs = await _subscription.GetSubscriptionByIdAsync(id);

            if (subs == null)
            {
                return NotFound();
            }
            var SubsDTO = mapper.Map<Models.SuscriptionDTO>(subs);
            return Ok(SubsDTO);
        }

        // POST: api/subscriptions
        [HttpPost]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> PostSubscription(Subscription subscripti)
        {
            if (subscripti == null)
            {
                return BadRequest();
            }

            var subs = new Subscription()
            {
                Id = subscripti.Id,
                CompanyName = subscripti.CompanyName,
                SubscriptionLevel = subscripti.SubscriptionLevel,
                StartDate = subscripti.StartDate,
                EndDate = subscripti.EndDate,
                PaymentId = subscripti.PaymentId,
                Status = subscripti.Status,
            };

            var subsc = _subscription.AddSubscription(subs);

            

            return Ok($"keep ur subscription id for future reference {subsc}");
        }

        // PUT: api/subscriptions/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> PutSubscription(int id, Subscription subscription)
        {
            if (id != subscription.Id)
            {
                return BadRequest();
            }
            try
            {
                var subscrib = new ET_ShiftManagementSystem.Entities.Subscription()
                {
                    Id = id,
                    CompanyName = subscription.CompanyName,
                    SubscriptionLevel = subscription.SubscriptionLevel,
                    StartDate = subscription.StartDate,
                    EndDate = subscription.EndDate,
                    PaymentId = subscription.PaymentId,
                    Status = subscription.Status,

                };
                await _subscription.UpdateSubscriptionByIdAsync(id, subscrib);

                return Ok(subscrib);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // DELETE: api/subscriptions/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<Subscription>> DeleteSubscription(int id)
        {
            var subscription = await _subscription.DeleteSubscriptionByIdAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            return subscription;
        }

    }
}
