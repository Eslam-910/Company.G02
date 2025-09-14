using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Data.Contexts;

namespace Company.G02.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyDbContext _context;

        public IDepartmentRepository departmentRepository { get; }

        public IEmployeeRepository employeeRepository { get; }

        public UnitOfWork(CompanyDbContext context)
        {
            _context = context;
            employeeRepository=new EmpolyeeRepository(_context);
            departmentRepository=new DepartmenRepository(_context);
        }
    }
}
