using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bulky.Models.ViewModels;
using System.Drawing;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
  /*  [Authorize(Roles = SD.Role_Admin)]*/
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
     
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
          
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            /* List<Category> categories = _unitOfWork.Category.GetAll().ToList();*/
            return View(objCompanyList);
        }
        public IActionResult Upsert(int? id)
        {



            if (id == null || id == 0)
            {
                // create 
                return View(new Company());
            }
            else
            {
                Company company = _unitOfWork.Company.Get(u => u.Id == id);
                return View(company);
            }


        }

        /* ViewData["CategoryList"] = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
         {
             Text = u.Name,
             Value = u.Id.ToString()
         });

         CompanyVM CompanyVm = new CompanyVM()
         {
             Company = new Company()
         };
         return View(CompanyVm);*/
        /*}*/


        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            if (ModelState.IsValid)
            {
                if (companyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(companyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(companyObj);
                }
                _unitOfWork.Save();
                TempData["success"] = "Them thanh cong";
                return RedirectToAction("Index");

            }
            else
            {
                return View(companyObj);
            }
           

           
        }
    



      
/*
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Company? CompanyFromDb = _unitOfWork.Producdt.Get(u => u.Id == id);
            if (CompanyFromDb == null)
            {
                return NotFound();
            }
            return View(CompanyFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Company? Company = _unitOfWork.Producdt.Get(u => u.Id == id);
            if (Company == null)
            {
                return NotFound();
            }
            _unitOfWork.Producdt.Remove(Company);
            _unitOfWork.Save();
            TempData["success"] = "Delete thanh cong";
            return RedirectToAction("Index");
        }*/

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll() {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new {data = objCompanyList});
        }

        [HttpDelete]
        public IActionResult Delete(int? id) {
        var CompanyToBeDelete = _unitOfWork.Company.Get(u=>u.Id == id);
            if(CompanyToBeDelete == null) {

                return Json(new { success = false, message = "Error while deleting" });
            }
          
            _unitOfWork.Company.Remove(CompanyToBeDelete);
            _unitOfWork.Save();
          
            return Json(new { success = false, message = "Delete  success" });
        }
        #endregion
    }

}
