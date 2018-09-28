using SalesWebMvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMvc.Services
{
    public class DepartmentService : ServiceBase<Department>
    {
        public DepartmentService(SalesWebMvcContext context) : base(context)
        {
        }

        protected override IQueryable<Department> Ordenar(IQueryable<Department> query)
        {
            return query.OrderBy(x => x.Name);
        }
    }

    public class ServiceBase<T> where T : class
    {
        protected readonly SalesWebMvcContext Context;

        public ServiceBase(SalesWebMvcContext context)
        {
            Context = context;
        }

        public async Task<List<T>> FindAllAsync()
        {
            var query = Context.Set<T>().AsQueryable();
            var temp = Ordenar(query);
            return await temp.ToListAsync();
        }

        protected virtual IQueryable<T> Ordenar(IQueryable<T> query)
        {
            return query;
        }
    }
}