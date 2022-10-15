using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AcademicManagement;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace Lab2.Pages
{
    public class RegistrationModel : PageModel
    {
        [BindProperty]
        public string SelectedStudentId { get; set; }
        [BindProperty]
        public List<string> CheckedCoursesCode { get; set; }
        public int CountRegistered { get; set; }
        public bool HasSelectedStudent { get; set; }
        public bool notSelectedCourse { get; set; }

        [BindProperty]
        public List<double> Grade { get; set; }
        public List<AcademicRecord> AcademicRecords { get; set; }
        public int CountAcademicRecords { get; set; }

        public string OrderBy { get; set; }
        //public string StudentId { get; set; }

        public void OnGet(string orderby, string studentId, int countRecords)
        {
            HasSelectedStudent = false;
            SelectedStudentId=studentId;
            CountAcademicRecords = countRecords;
            //SelectedStudentId=studentId;

            if (HttpContext.Session.GetString("studentId") != null)
            {
                HasSelectedStudent = true;
                SelectedStudentId = HttpContext.Session.GetString("studentId");
                
            }

            if (HttpContext.Session.GetInt32("CountAcademicRecords") != null)
            {
                CountAcademicRecords = (int)HttpContext.Session.GetInt32("CountAcademicRecords");
            }
            
            if (orderby != null)
            {
                HttpContext.Session.SetString("orderby", orderby);
                OrderBy = orderby;

            }
            else if (HttpContext.Session.GetString("orderby") != null)
            {
                OrderBy = HttpContext.Session.GetString("orderby");
            }
            else
            {
                OrderBy = null;
            }
        }

        public void OnPostStudentSelected()
        {

            AcademicRecords = new List<AcademicRecord>();
            if (SelectedStudentId != "-1")
            {
                HasSelectedStudent = true;
                AcademicRecords=DataAccess.GetAcademicRecordsByStudentId(SelectedStudentId);
                CountAcademicRecords = AcademicRecords.Count;
                HttpContext.Session.SetString("studentId", SelectedStudentId);
                HttpContext.Session.SetInt32("CountAcademicRecords", CountAcademicRecords);
            }
            else if (SelectedStudentId == "-1")
            {
                HasSelectedStudent = false;
            }

        }

        public void OnPostRegister()
        {
            AcademicRecords = new List<AcademicRecord>();


            if (CheckedCoursesCode.Count > 0)
            {
                notSelectedCourse = false;
                foreach (string courseCode in CheckedCoursesCode)
                {
                    AcademicRecord academicRecord = new AcademicRecord();
                    academicRecord.CourseCode = courseCode;
                    academicRecord.StudentId = SelectedStudentId;
                    DataAccess.AddAcademicRecord(academicRecord);
                    AcademicRecords.Add(academicRecord);
                }
            }else if(CheckedCoursesCode.Count == 0)
            {
                notSelectedCourse = true;
            }
            CountAcademicRecords = AcademicRecords.Count;
            HttpContext.Session.SetInt32("CountAcademicRecords", CountAcademicRecords);
            HttpContext.Session.SetString("studentId", SelectedStudentId);
        }


        public void OnPostSubmitGrades()
        {

            AcademicRecords = DataAccess.GetAcademicRecordsByStudentId(SelectedStudentId);

            if (Grade.Count != 0)
            {
                for (int i = 0; i < AcademicRecords.Count; i++)
                {
                    AcademicRecords[i].Grade = Grade[i];
                }
            }
            CountAcademicRecords = AcademicRecords.Count;
            HttpContext.Session.SetInt32("CountAcademicRecords", CountAcademicRecords);
            HttpContext.Session.SetString("studentId", SelectedStudentId);
        }

    }
}
