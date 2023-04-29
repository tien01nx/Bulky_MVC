using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bulky.Models.ViewModels;
using System.Drawing;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Producdt.GetAll(includeProperties:"Category").ToList();
           /* List<Category> categories = _unitOfWork.Category.GetAll().ToList();*/
            return View(objProductList);
        }
        public IActionResult Upsert(int? id)
        {


            ProductVM productVm = new ProductVM()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if(id == null || id == 0)
            {
                // create 
                return View(productVm);
            }
            else
            {
                productVm.Product = _unitOfWork.Producdt.Get(u => u.Id == id);
                return View(productVm);
            }
           

        } 

           /* ViewData["CategoryList"] = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            ProductVM productVm = new ProductVM()
            {
                Product = new Product()
            };
            return View(productVm);*/
        /*}*/

     
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile ? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file!= null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    String productPath = Path.Combine(wwwRootPath, @"images\product");

                    // thuc hien xoa hinh anh hien tai
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        // delete the old image
                        var oldImagePath = Path.Combine(wwwRootPath,productVM.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Producdt.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Producdt.Update(productVM.Product);
                }
               
                _unitOfWork.Save();
                TempData["success"] = "Them thanh cong";
                return RedirectToAction("Index");
            }
            else
            {

                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
               
             
                return View(productVM);
              
            }
          
          
        }



      
/*
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromDb = _unitOfWork.Producdt.Get(u => u.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product? product = _unitOfWork.Producdt.Get(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _unitOfWork.Producdt.Remove(product);
            _unitOfWork.Save();
            TempData["success"] = "Delete thanh cong";
            return RedirectToAction("Index");
        }*/

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll() {
            List<Product> objProductList = _unitOfWork.Producdt.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = objProductList});
        }

        [HttpDelete]
        public IActionResult Delete(int? id) {
        var productToBeDelete = _unitOfWork.Producdt.Get(u=>u.Id == id);
            if(productToBeDelete == null) {

                return Json(new { success = false, message = "Error while deleting" });
            }
            // delete the old image
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDelete.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Producdt.Remove(productToBeDelete);
            _unitOfWork.Save();
          
            return Json(new { success = false, message = "Delete  success" });
        }
        #endregion
    }
}
