// GET: AcademicRecords/EditAll/
        [HttpGet]
        public async Task<IActionResult> EditAll(string sortBy, string courseId, string studentId)
        {

            //if column sorting has been selected, save into a session
            if (sortBy != null)
            {
                HttpContext.Session.SetString("SortOrder", sortBy);
            }
            //else no column sorting selected, but a sort order was saved in a sessionn
            else if (HttpContext.Session.GetString("SortOrder") != null)
            {
                sortBy = HttpContext.Session.GetString("SortOrder");
            }

            //sorting by course title then student name
            if (sortBy == "course")
            {
                var studentRecordContext = _context.AcademicRecords
                    .Include(a => a.CourseCodeNavigation)
                    .Include(a => a.Student)
                    .OrderBy(ar => ar.CourseCodeNavigation.Title)
                    .ThenBy(ar => ar.Student.Name)
                    .ToListAsync();

                return View(await studentRecordContext);
            }

            //sorting by student name then course title
            else if (sortBy == "student")
            {
                var studentRecordContext = _context.AcademicRecords
                    .Include(a => a.CourseCodeNavigation)
                    .Include(a => a.Student)
                    .OrderBy(ar => ar.Student.Name)
                    .ThenBy(ar => ar.CourseCodeNavigation.Title)
                    .ToListAsync(); 
                
                return View(await studentRecordContext);
            }
            //if no sorting has been selected
            else
            {
                var studentRecordContext = _context.AcademicRecords
                    .Include(a => a.CourseCodeNavigation)
                    .Include(a => a.Student)
                    .ToListAsync(); 
                
                return View(await studentRecordContext);
            }


            if (courseId == null || studentId == null)
            {
                return NotFound();
            }

            var academicRecord = await _context.AcademicRecords
                 .Include(a => a.CourseCodeNavigation)
                 .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.StudentId == studentId && m.CourseCode == courseId);


            if (academicRecord == null)
            {
                return NotFound();
            }
            return View(academicRecord);         
        }


        // POST: AcademicRecords/EditAll/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAll(List<AcademicRecord> gradeRecords)
            //this works partially if I use IEnumerable and the other part if I use List SIGH
        {
            foreach (var grade in gradeRecords)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(grade);
                        await _context.SaveChangesAsync();

                        //return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AcademicRecordExists(grade.StudentId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    var academicRecords = _context.AcademicRecords
                        .Include(a => a.CourseCodeNavigation)
                        .Include(a => a.Student).ToList();

                    foreach (var grade1 in academicRecords)
                    {
                        foreach (var grade2 in gradeRecords)
                        {
                            if ((grade1.CourseCode == grade2.CourseCode) && (grade1.StudentId == grade2.StudentId))
                            {
                                grade2.CourseCodeNavigation = (grade1.CourseCodeNavigation);
                                grade2.Student = (grade1.Student);
                            }
                        }

                    }
                    return View(gradeRecords);

                }
            }
            return RedirectToAction(nameof(Index));
        }


        private bool AcademicRecordExists(string id)
        {
            return _context.AcademicRecords.Any(e => e.StudentId == id);
        }
    }