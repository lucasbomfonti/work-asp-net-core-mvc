using SalesWebMvc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services
{
    public class SellerService : ServiceBase<Seller>
    {

        public SellerService(SalesWebMvcContext context) : base(context)
        {
        }

    
        public async Task InsertAsync(Seller obj)
        {
            Context.Add(obj);
            await Context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await Context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await Context.Seller.FindAsync(id);
                Context.Seller.Remove(obj);
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new IntegrityException("Can't delete seller because he/she has sales");
            }
        }

        public async Task UpdateAsync(Seller obj)
        {
            bool hasAny = await Context.Seller.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
                Context.Update(obj);
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}