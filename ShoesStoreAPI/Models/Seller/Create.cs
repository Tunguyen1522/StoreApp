using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoesStoreAPI.Class;
using Microsoft.Data.SqlClient;

namespace ShoesStoreAPI.Models.Seller
{
    public class Create : PageModel
    {
        public Product product = new Product();
        public string errorMessage = string.Empty;
        public Create()
        {
            product = new Product();
        }
    }
}
