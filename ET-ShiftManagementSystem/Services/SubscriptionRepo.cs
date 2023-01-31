using ET_ShiftManagementSystem.Data;
using ET_ShiftManagementSystem.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;

namespace ET_ShiftManagementSystem.Servises
{
    public interface ISubscriptionRepo
    {
       public Task<IEnumerable<Subscription>> GetSubscriptionAsync();
       public Task<Subscription> GetSubscriptionByIdAsync(int id);

        public  long AddSubscription(Subscription subs);

        public Task<Subscription> UpdateSubscriptionByIdAsync(int id , Subscription subscription);

        public Task<Subscription> DeleteSubscriptionByIdAsync(int id);
    }
    public class SubscriptionRepo : ISubscriptionRepo
    {
        private readonly ShiftManagementDbContext shiftManagementDb;

        public SubscriptionRepo(ShiftManagementDbContext shiftManagementDb)
        {
            this.shiftManagementDb = shiftManagementDb;
        }

        public long AddSubscription(Subscription subs)
        {
            shiftManagementDb.Subscriptions.Add(subs);
            shiftManagementDb.SaveChanges();
            return subs.Id;
        }

        public async Task<Subscription> DeleteSubscriptionByIdAsync(int id)
        {
            var delete = await shiftManagementDb.Subscriptions.FirstOrDefaultAsync( x => x.Id == id);

            if (delete == null)
            {
                return null; 
            }


            shiftManagementDb.Subscriptions.Remove(delete);
            shiftManagementDb.SaveChanges();
            return delete;
        }

        public  async Task<IEnumerable<Subscription>> GetSubscriptionAsync()
        {
            var subscription = await shiftManagementDb.Subscriptions.ToListAsync();

            //var expiredSub = await shiftManagementDb.Subscriptions.FirstOrDefaultAsync(x => x.EndDate <= DateTime.Now);

            //if(expiredSub.Id == null)
            //{
            //    return subscription;
            //}
            //else
            //{
            //   var delete = await shiftManagementDb.Subscriptions.FirstOrDefaultAsync(x => x.Id ==  expiredSub.Id);
            //    shiftManagementDb.Subscriptions.Remove(delete);
            //    //shiftManagementDb.SaveChanges();
            //}

            return subscription;
            
        }

        public async Task<Subscription> GetSubscriptionByIdAsync(int id)
        {
            var subs = await shiftManagementDb.Subscriptions.FirstOrDefaultAsync(s => s.Id == id);

            return subs;
        }

        public async Task<Subscription> UpdateSubscriptionByIdAsync(int id, Subscription subscription)
        {
           var Existingsubs = await shiftManagementDb.Subscriptions.FirstOrDefaultAsync(x => x.Id== id);

            if (Existingsubs == null)
            {
                return null;
            }

            Existingsubs.Id = subscription.Id;
            Existingsubs.CompanyName= subscription.CompanyName;
            Existingsubs.SubscriptionLevel = subscription.SubscriptionLevel;
            Existingsubs.StartDate  = subscription.StartDate;
            Existingsubs.EndDate= subscription.EndDate;
            Existingsubs.Status = subscription.Status;
            
            await shiftManagementDb.SaveChangesAsync();

            return Existingsubs;
        }
    }
}
