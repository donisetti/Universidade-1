﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universidade.Contoso.Dominio.DAL;
using PagedList;

namespace Universidade.Contoso.Web.Controllers
{
    public class AlunoController : Controller
    {
        private IStudentRepository studentRepository;

        public AlunoController()
        {
            this.studentRepository = new StudentRepository(new SchoolContext());
        }


        public AlunoController(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }




        //// GET: Aluno
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: Aluno
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.tema = "_Cyborg";
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var students = from s in studentRepository.GetStudents()
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                                       || s.FirstMidName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:  // Name ascending 
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(students.ToPagedList(pageNumber, pageSize));
        }
    }
}