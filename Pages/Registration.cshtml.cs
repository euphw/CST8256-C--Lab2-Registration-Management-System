using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AcademicManagement;

namespace Lab2.Pages
{
    public class RegistrationModel : PageModel
    {
        [BindProperty]
        public string SelectedStudentId { get; set; }
        [BindProperty]
        public List<string> CheckedCoursesCode { get; set; }
        public List<Course> RegisteredCourses { get; set; }
        public int CountRegistered { get; set; }
        public bool HasSelectedStudent { get; set; }
        public bool notSelectedCourse { get; set; }

        public int CountChecked { get; set; }


        public void OnGet()
        {
            CountRegistered = -1;
            HasSelectedStudent = false;
        }

        public void OnPostStudentSelected()
        {
            RegisteredCourses = new List<Course>();

            if (SelectedStudentId != "-1")
            {
                HasSelectedStudent = true;
                foreach (AcademicRecord record in DataAccess.GetAcademicRecordsByStudentId(SelectedStudentId))
                {
                    if (record.StudentId == SelectedStudentId)
                    {
                        //RegisteredCourseCode.Add(record.CourseCode);
                        foreach (Course course in DataAccess.GetAllCourses())
                        {
                            if (record.CourseCode == course.CourseCode)
                            {
                                RegisteredCourses.Add(course);
                            }
                        };

                    }
                }

            }
            else if (SelectedStudentId == "-1")
            {
                HasSelectedStudent = false;
            };
            CountRegistered = RegisteredCourses.Count;

        }

        public void OnPostRegister()
        {
            //RegisteredCourses = new List<Course>();
            //AcademicRecord academicRecord = new AcademicRecord();
           
            if (CheckedCoursesCode.Count > 0)
            {
                notSelectedCourse = false;
                foreach (string courseCode in CheckedCoursesCode)
                {
                    AcademicRecord academicRecord = new AcademicRecord();
                    academicRecord.CourseCode = courseCode;
                    academicRecord.StudentId = SelectedStudentId;
                    DataAccess.AddAcademicRecord(academicRecord);
                }
            }else if(CheckedCoursesCode.Count == 0)
            {
                notSelectedCourse = true;
            }
            CountChecked = CheckedCoursesCode.Count;
            RegisteredCourses = new List<Course>();

            if (SelectedStudentId != "-1")
            {
                HasSelectedStudent = true;
                foreach (AcademicRecord record in DataAccess.GetAcademicRecordsByStudentId(SelectedStudentId))
                {
                    if (record.StudentId == SelectedStudentId)
                    {
                        //RegisteredCourseCode.Add(record.CourseCode);
                        foreach (Course course in DataAccess.GetAllCourses())
                        {
                            if (record.CourseCode == course.CourseCode)
                            {
                                RegisteredCourses.Add(course);
                            }
                        };

                    }
                }

            };
            CountRegistered = RegisteredCourses.Count;
        }
    }
}
