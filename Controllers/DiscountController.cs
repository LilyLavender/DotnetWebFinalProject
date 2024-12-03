using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

public class DiscountController : Controller
{
    // this controller depends on the NorthwindRepository
    private DataContext _dataContext;
    public DiscountController(DataContext db) => _dataContext = db;
    public IActionResult Index() => View(_dataContext.Discounts.OrderBy(d => d.Title));

    [Authorize(Roles = "northwind-employee")]
    public IActionResult DeleteDiscount(int id)
    {
        _dataContext.DeleteDiscount(_dataContext.Discounts.FirstOrDefault(d => d.DiscountId == id));
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "northwind-employee")]
    public IActionResult AddDiscount(int id)
    {
        ViewBag.Products = _dataContext.Products.Where(p => !p.Discontinued).ToList();
        return View(new Discount());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "northwind-employee")]
    public IActionResult AddDiscount(Discount discount)
    {
        Random rnd = new Random();
        discount.Code = rnd.Next(1000, 10000);
        discount.DiscountPercent = discount.DiscountPercent / 100;
        if (ModelState.IsValid)
        {
            _dataContext.AddDiscount(discount);
            return RedirectToAction("Index");
        }
        return View();
    }

    [Authorize(Roles = "northwind-employee"), HttpGet]
    public async Task<IActionResult> EditDiscount(int id)
    {
        var discount = await _dataContext.Discounts.FirstOrDefaultAsync(d => d.DiscountId == id);
        ViewBag.Products = _dataContext.Products.Where(p => !p.Discontinued).ToList();
        discount.DiscountPercent = discount.DiscountPercent * 100;
        if (discount != null)
        {
            return View(discount);
        }
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "northwind-employee"), HttpPost]
    public async Task<IActionResult> EditDiscount(Discount discount)
    {
        if (ModelState.IsValid)
        {
            try
            {
                discount.DiscountPercent = discount.DiscountPercent / 100;
                var result = await _dataContext.EditDiscountAsync(discount);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult("Failed to update discount");
                }
            }
            catch (Exception ex)
            {
                AddErrorsFromResult($"Error updating discount: {ex.Message}");
            }
        }
        return View();
    }

    private void AddErrorsFromResult(string error)
    {
        ModelState.AddModelError("", error);
    }
}

