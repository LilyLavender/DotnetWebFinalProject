using Microsoft.AspNetCore.Mvc;

public class DiscountController : Controller
{
  // this controller depends on the NorthwindRepository
  private DataContext _dataContext;
  public DiscountController(DataContext db) => _dataContext = db;
  public IActionResult Index() => View(_dataContext.Discounts.OrderBy(d => d.Title));
}
