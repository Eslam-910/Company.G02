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
    public class EmpolyeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly CompanyDbContext _context;

        public EmpolyeeRepository(CompanyDbContext context):base(context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetByNameAsync(string name)
        {
            return await _context.Employees.Include(E=>E.department).Where(E => E.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
    }
}
