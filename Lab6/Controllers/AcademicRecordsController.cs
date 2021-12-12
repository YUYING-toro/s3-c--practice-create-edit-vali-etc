using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab6.Models.DataAccess;
using Microsoft.AspNetCore.Http;


namespace Lab6.Controllers
{

    public class AcademicRecordsController : Controller
    {
        
        private readonly StudentRecordContext _context;
        public string ErrorMessage { get; set; }

        public AcademicRecordsController(StudentRecordContext context)
        {
            _context = context;
        }

        // GET: AcademicRecords
        public IActionResult Index(string sortBy)
        {
            var studentRecordContext =  _context.AcademicRecords.Include(a => a.CourseCodeNavigation).Include(a => a.Student).AsQueryable();

            // pdf >２ 
            if (sortBy != null)
            {

                HttpContext.Session.SetString("sortBy", sortBy);

                if (sortBy == "sortCourse")
                    studentRecordContext = studentRecordContext.OrderBy(x => x.CourseCodeNavigation.Title).ThenBy(y => y.Student.Name);
                else if (sortBy == "sortSt")
                    studentRecordContext = studentRecordContext.OrderBy(x => x.Student.Name).ThenBy(y => y.CourseCodeNavigation.Title);
            }
            else if (HttpContext.Session.GetString("sortBy") != null)
            {
                sortBy = HttpContext.Session.GetString("sortBy");
                if (sortBy == "sortCourse")
                    studentRecordContext = studentRecordContext.OrderBy(x => x.CourseCodeNavigation.Title).ThenBy(y => y.Student.Name);
                else if (sortBy == "sortSt")
                    studentRecordContext = studentRecordContext.OrderBy(x => x.Student.Name).ThenBy(y => y.CourseCodeNavigation.Title);

            }


            return View( studentRecordContext.ToList());
        }
        //post from EditAll



        // GET: AcademicRecords/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicRecord = await _context.AcademicRecords
                .Include(a => a.CourseCodeNavigation)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (academicRecord == null)
            {
                return NotFound();
            }

            return View(academicRecord);
        }

        // GET: AcademicRecords/Create
        public IActionResult Create()
        {

            ViewData["CourseCode"] = new SelectList(_context.Courses, "Code", "Code");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id");
            return View();
        }

        // POST: AcademicRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseCode,StudentId,Grade")] AcademicRecord academicRecord)
        {
            // previous record?
            if (_context.AcademicRecords.Any(x => x.StudentId == academicRecord.StudentId && x.CourseCode == academicRecord.CourseCode))
            {
                // 防error 若沒載入 option 
                //Students = _context.Students.ToList();
                //Courses = _context.Courses.ToList();

                ModelState.AddModelError("Id", "The specific academic record already exist!");
                // no return View()
                
            }

            if (ModelState.IsValid)
            {
                _context.Add(academicRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseCode"] = new SelectList(_context.Courses, "Code", "Code", academicRecord.CourseCode);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", academicRecord.StudentId);
            return View(academicRecord);
        }

        // GET: AcademicRecords/Edit/5
        public async Task<IActionResult> Edit(string id, string courseCode)
        //public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicRecord = await _context.AcademicRecords.FindAsync(id, courseCode);  //defined with a 2-part composite key, but 1 values were passed to the 'DbSet.Find' method.
            //var academicRecord = _context.AcademicRecords.Any(y => y.StudentId == id);
            //var academicRecord = _context.AcademicRecords.Find(x => x.sty); // no work
            
            if (academicRecord == null)
            {
                return NotFound();
            }
            //ViewData["CourseCode"] = new SelectList(_context.Courses, "Code", "Code", academicRecord.CourseCode);
            //ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", academicRecord.StudentId);
            // Bai 用法 但我沒效
            ViewData["CourseTitle"] = _context.Courses.FirstOrDefault(a => a.Code == courseCode).Title;
            ViewData["CourseCode"] = _context.Courses.FirstOrDefault(a => a.Code == courseCode).Code;
            ViewData["StID"] = _context.Students.FirstOrDefault(a => a.Id == id).Id;
            ViewData["Name"] = _context.Students.FirstOrDefault(a => a.Id == id).Name;


            return View(academicRecord);
        }

        // POST: AcademicRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CourseCode,StudentId,Grade")] AcademicRecord academicRecord)
        {
            if (id != academicRecord.StudentId)
            {
                return NotFound();
            }
            //Vali error contect but name and title gone??
            if (academicRecord.Grade>100 || academicRecord.Grade < 0) 
            {
                ModelState.AddModelError("Grade", "Must between 0 and 100");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academicRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademicRecordExists(academicRecord.StudentId))
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
            //ViewData["CourseCode"] = new SelectList(_context.Courses, "Code", "Code", academicRecord.CourseCode);
            //ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", academicRecord.StudentId);
            ViewData["CourseTitle"] = _context.Courses.FirstOrDefault(a => a.Code == academicRecord.CourseCode).Title;
            ViewData["CourseCode"] = _context.Courses.FirstOrDefault(a => a.Code == academicRecord.CourseCode).Code;
            ViewData["StID"] = _context.Students.FirstOrDefault(a => a.Id == id).Id;
            ViewData["Name"] = _context.Students.FirstOrDefault(a => a.Id == id).Name;
            return View(academicRecord);
        }

        #region test1
        //public async Task<IActionResult> test1()
        ////public IActionResult Edit(string id)
        //{
        //    var studentRecordContext = _context.AcademicRecords.Include(a => a.CourseCodeNavigation).Include(a => a.Student);



        //    return View(studentRecordContext.ToList());

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> test1(string id, [Bind("CourseCode,StudentId,Grade")] AcademicRecord academicRecord)
        {
            if (id != academicRecord.StudentId)
            {
                return NotFound();
            }
            //Vali error contect but name and title gone??
            if (academicRecord.Grade > 100 || academicRecord.Grade < 0)
            {
                ModelState.AddModelError("Grade", "Must between 0 and 100");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academicRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademicRecordExists(academicRecord.StudentId))
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
            return View(academicRecord);
        }
        #endregion

        #region Edit all
        public IActionResult EditAll(string sortBy)
        {
            var studentRecordContext = _context.AcademicRecords.Include(a => a.CourseCodeNavigation).Include(a => a.Student).AsQueryable();
            if (sortBy != null)
            {
                HttpContext.Session.SetString("sortBy", sortBy);

                if (sortBy == "sortCourse")
                    studentRecordContext = studentRecordContext.OrderBy(x => x.CourseCodeNavigation.Title).ThenBy(y => y.Student.Name);
                else if (sortBy == "sortSt")
                    studentRecordContext = studentRecordContext.OrderBy(x => x.Student.Name).ThenBy(y => y.CourseCodeNavigation.Title);
            }else if (HttpContext.Session.GetString("sortBy") != null)
            {
                sortBy = HttpContext.Session.GetString("sortBy");
                if (sortBy == "sortCourse")
                    studentRecordContext = studentRecordContext.OrderBy(x => x.CourseCodeNavigation.Title).ThenBy(y => y.Student.Name);
                else if (sortBy == "sortSt")
                    studentRecordContext = studentRecordContext.OrderBy(x => x.Student.Name).ThenBy(y => y.CourseCodeNavigation.Title);

            }
            return View(studentRecordContext.ToList());
        }

        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAll(List<AcademicRecord> postGrades)  //型別 多筆AR型別資料 在HTML lin1 引用 記得 list<DB~~~>
        {           
            //Vali error contect but name and title gone??
            if (postGrades.Any(a=>a.Grade>100)||postGrades.Any(a=>a.Grade<0))
            {
                ModelState.AddModelError("Grade", "Must between 0 and 100");
                return View(postGrades);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (AcademicRecord item in postGrades)   //卡死  > html 問題!
                    {                       
                        _context.Update(item);
                    }                                   
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(postGrades);
        }

        #endregion











        // GET: AcademicRecords/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicRecord = await _context.AcademicRecords
                .Include(a => a.CourseCodeNavigation)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (academicRecord == null)
            {
                return NotFound();
            }

            return View(academicRecord);
        }

        // POST: AcademicRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var academicRecord = await _context.AcademicRecords.FindAsync(id);
            _context.AcademicRecords.Remove(academicRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcademicRecordExists(string id)
        {
            return _context.AcademicRecords.Any(e => e.StudentId == id);
        }
    }
}
