using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[Authorize(Roles = Roles.Admin)]
public class CategoryController : ApiBaseController
{
      private List<string>  _categories= new List<string>()
    {
        "Fruits and Vegetables",
        "Dairy",
        "Bakery, Cereals and Spices",
        "Beverages",
        "Snacks and Branded Foods",
        "Cleaning and Household",
        "Personal Care",
        "Baby Care",
        "Pet Care",
        "Other Essentials"
    };
    
    public CategoryController()
    {
    }
    
    [HttpGet]
    public List<string> GetCategories()
    {
        return _categories;
    }
    
    [HttpPost("add")]
    public void AddCategory(string category)
    {
        _categories.Add(category);
    }
    
    [HttpDelete("delete")]
    public void DeleteCategory(string category)
    {
        _categories.Remove(category);
    }
}