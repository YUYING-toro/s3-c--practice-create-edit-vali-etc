using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab6.Models.DataAccess;
using Lab6.Models;

namespace Lab6.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly StudentRecordContext _context;

        public EmployeesController(StudentRecordContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.Include(x=>x.EmployeeRoles).ThenInclude(y=>y.Role).ToListAsync()); //回饋給index 的 Model提供DB
        }

        // GET: Employees/Details/5
        #region detail
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }
        #endregion

        // GET: Employees/Create
        public IActionResult Create()
        {
            //ViewData["Roles"] = _context.Roles.ToList();
            EmployeeRoleSelections employeeRoleSelections = new EmployeeRoleSelections();
            return View(employeeRoleSelections);
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create( EmployeeRoleSelections employeeRoleSelections)  //'System.Int32' to type 'System.String'.???
        {
            if (!employeeRoleSelections.roleSelections.Any(m => m.selected))
            {
                ModelState.AddModelError("roleSelections", "You must select at least one role!");
            }
            if (_context.Employees.Any(x=>x.UserName == employeeRoleSelections.employee.UserName)) 
            {
                ModelState.AddModelError("employee.UserName", "This user name already exists!");
            }

            if (ModelState.IsValid) 
            {
                _context.Add(employeeRoleSelections.employee);
                _context.SaveChanges();
                foreach (RoleSelection item in employeeRoleSelections.roleSelections)   //InvalidOperationException: A relational store has been configured without specifying either the DbConnection 
                {
                    if (item.selected)
                    {
                        EmployeeRole empRole = new EmployeeRole {RoleId = item.role.Id, EmployeeId= employeeRoleSelections.employee.Id};  //using Lab6.Models; ?? 如何連線 models/ viewModel???
                        _context.EmployeeRoles.Add(empRole);
                    }
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employeeRoleSelections);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)//emp id
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                EmployeeRoleSelections employeeRoleSelections = new EmployeeRoleSelections();  //實化一個返回給畫面的格式
                employeeRoleSelections.employee = employee; //填入values
                //save selection
                foreach (EmployeeRole option in _context.EmployeeRoles)
                {
                    //if (option.RoleId != id)
                    //{
                        if (employee.EmployeeRoles.Any(x => x.RoleId == option.RoleId))  // 該學生有any課紀錄 同 資料庫 課堂庫
                        {
                            //畫面上 對此課堂 打勾，此學生有存取該課紀錄
                            employeeRoleSelections.roleSelections.Add(new RoleSelection { selected = true, role = option.Role }); //欄位bool  Role 
                    }
                        else
                        {
                            employeeRoleSelections.roleSelections.Add(new RoleSelection { selected = false, role = option.Role });
                        }
                    //}
                }
                return View(employeeRoleSelections); // 釋出 新創的資料給畫面
            }
            else {
                return NotFound();
            }
            //return View(employeeRoleSelections);

            
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UserName,Password")] Employee employee)
        //public async Task<IActionResult> Edit(string id, EmployeeRoleSelections empR)

        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
        #endregion
    }
}
