using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G02.BLL.Interfaces;
using Company.G02.DAL.Data.Contexts;
using Company.G02.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.G02.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly CompanyDbContext _context;

        public GenericRepository(CompanyDbContext context)
        {
           _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await _context.Employees.Include(E=>E.department).ToListAsync();
            }
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(int id)
        {

            if (typeof(T) == typeof(Employee))
            {
                return await _context.Employees.Include(E => E.department).FirstOrDefaultAsync(E=>E.Id==id)as T;
            }
            return  _context.Set<T>().Find(id);
        }

        public async Task<int> AddAsync(T model)
        {
            await _context.Set<T>().AddAsync(model);
            return _context.SaveChanges();
        }
        public int Update(T model)
        {
            _context.Set<T>().Update(model);
            return _context.SaveChanges();
        }

        public int Delete(T model)
        {
            _context.Set<T>().Remove(model);
            return _context.SaveChanges();
        }
      
    }
}
