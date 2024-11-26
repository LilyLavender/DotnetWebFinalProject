using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class DiscountController : Controller
{
    // this controller depends on the NorthwindRepository
    private DataContext _dataContext;
    public DiscountController(DataContext db) => _dataContext = db;
    public IActionResult Index() => View(_dataContext.Discounts.OrderBy(d => d.Title));

    public IActionResult DeleteDiscount(int id)
    {
        _dataContext.DeleteDiscount(_dataContext.Discounts.FirstOrDefault(d => d.DiscountId == id));
        return RedirectToAction("Index");
    }

    private void AddErrorsFromResult(string error)
    {
        ModelState.AddModelError("", error);
    }
}
