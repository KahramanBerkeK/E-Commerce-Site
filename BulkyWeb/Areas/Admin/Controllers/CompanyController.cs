using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;



namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {

        private readonly IUnitofWork _unitofWork;
        
        public CompanyController(IUnitofWork unitofWork)
        {

            _unitofWork = unitofWork;
            
        }

        public IActionResult Index()
        {

            List<Company> objCompanyList = _unitofWork.Company.GetAll().ToList();

           

            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {
            
            
      
            if(id==null || id == 0)
            {
                //Create
                return View(new Company());

            }
            else
            {
                //update
                Company companyObj = _unitofWork.Company.Get(u => u.id == id);
                return View(companyObj); 
            }
            
        }
        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {

            if (ModelState.IsValid)
            {
                
                
                if(CompanyObj.id == 0)
                {
                    _unitofWork.Company.Add(CompanyObj);

                }
                else
                {
                    _unitofWork.Company.Update(CompanyObj);
                }
                
                _unitofWork.Save();
                TempData["success"] = "Company created successfully.";
                return RedirectToAction("Index");
            }
            else
            {
              
                return View(CompanyObj);

            }
            


        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {

            List<Company> objCompanyList = _unitofWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });


        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitofWork.Company.Get(u => u.id == id);
            List<Company> objCompanyList = _unitofWork.Company.GetAll(includeProperties: "Category").ToList();

            if(CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting." });

            }

           
            _unitofWork.Company.Remove(CompanyToBeDeleted);
            _unitofWork.Save();

            return Json(new { success = true, message = "Deleted Successfully." });

        }

        #endregion

    }
}
