using Mango.web.Models;
using Mango.web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mango.web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService; 
        public ProductController(IProductService productService)
        {
            _productService=productService;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List <ProductDto> list= new();
            ResponseDto? response = await _productService.GetAllProductsAsync();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }
        public IActionResult ProductCreate()
        { 
          return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid) {
                ResponseDto? response = await _productService.CreateProductAsync(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(model);
        }
        public async Task<IActionResult> ProductEdit(int productId)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(productId);
            if (response != null && response.IsSuccess)
            {
                var productdto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(productdto);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto productDto)
        {
            ResponseDto? response = await _productService.UpdateProductAsync(productDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product updated successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(productDto);
        }
        public async Task<IActionResult> ProductDelete(int productId)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(productId);
            if (response != null && response.IsSuccess)
            {
                var productdto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(productdto);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
            ResponseDto? response = await _productService.DeleteProductAsync(productDto.ProductId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(productDto);
        }
    }
}
