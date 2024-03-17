using EmployeePortal.Data;
using EmployeePortal.Models;
using EmployeePortal.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace EmployeePortal.Controllers
{
    public class EmployeesController : Controller
    {

        private readonly MVCDemoDbContext mvcDemoDbContext;
        public EmployeesController(MVCDemoDbContext mvcDemoDbContext)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
        }

        public async Task<IActionResult>Index()
        {
           var employees =  await mvcDemoDbContext.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest) {

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            Name = addEmployeeRequest.Name,
            Email = addEmployeeRequest.Email,
            Sallary = addEmployeeRequest.Sallary,
            Department = addEmployeeRequest.Department,
            DateOfBirth = addEmployeeRequest.DateOfBirth
        };

        await mvcDemoDbContext.Employees.AddAsync(employee);
        await mvcDemoDbContext.SaveChangesAsync();

        return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await mvcDemoDbContext.Employees.FirstOrDefaultAsync(x=>x.Id == id);

            if (employee != null) 
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Sallary = employee.Sallary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth
                };
                return await Task.Run(() => View("view",viewModel));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);
            if (employee != null) 
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Sallary = model.Sallary;
                employee.Department = model.Department;
                employee.DateOfBirth = model.DateOfBirth;

                await mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            };
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);
                if(employee != null)
                {
                    mvcDemoDbContext.Employees.Remove(employee);
                    await mvcDemoDbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            return RedirectToAction("Index");

        }
    }
}
