using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoesStoreAPI.Class;
using ShoesStoreAPI.Models;
using ShoesStoreAPI.Models.Seller;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Claims;

namespace ShoesStoreAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        private bool IsMobileRequest()
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            return userAgent.Contains("Mobile", StringComparison.OrdinalIgnoreCase);
        }

        // GET: HomeController
        public async Task<ActionResult> Home()
        {
            try
            {
                List<Product> productList = new List<Product>();
                ViewBag.ProductList = productList;
                try
                {
                    productList = new List<Product>();
                    productList = await Search(Request.Query["query"]);
                }
                catch(Exception)
                {
                    productList = new List<Product>();
                    productList = await GetListByBrand(100, Request.Query["Brand"]);
                }
                if (IsMobileRequest())
                {
                    return View("Mobile/Home", new Home(productList));
                }
                else return View("Home", new Home(productList));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<ActionResult> Detail()
        {
            ViewBag.UserID = GetUserID();
            ViewBag.Role = GetRoleID();
            try
            {
                Product product = await GetProduct(Request.Query["id"]);
                if (IsMobileRequest())
                {
                    return View("Mobile/Detail", new Detail(product));
                }
                else return View("Detail", new Detail(product));
            }
            catch (Exception ex)
            {
                return RedirectToAction("HomePage");
            }
        }
        public async Task<ActionResult> HomePage()
        {
            ViewBag.RoleID = GetRoleID();
            try
            {
                List<Product> listNew = await GetListByBrand(4, "%");
                List<Product> listAdidas = await GetListByBrand(4, "Adidas");
                List<Product> listNike = await GetListByBrand(4, "Nike");
                List<Product> listAirJordan = await GetListByBrand(4, "Air Jordan");
                List<List<Product>> productLists = new List<List<Product>>
                {
                    listNew,
                    listAdidas,
                    listNike,
                    listAirJordan
                };
                if (IsMobileRequest())
                {
                    return View("Mobile/HomePage", new HomePage(productLists));
                }
                else return View("HomePage", new HomePage(productLists));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<ActionResult> Cart()
        {
            if (GetUserID() == "")
            {
                return NoContent();
            }
            switch (GetRoleID())
            {
                case "1":
                    return NoContent();
                case "0":
                    if (IsMobileRequest())
                    {
                        return View("Mobile/Cart", new CartModel(GetUserID()));
                    }
                    else return View("Cart", new CartModel(GetUserID()));
            }
            return View(new CartModel(GetUserID()));
        }
        public IActionResult AddToCart()
        {
            AddToCart addToCart = new AddToCart(Request.Query["Id"], Request.Query["num"], GetUserID());
            return View(addToCart);
        }
        public async Task<ActionResult> Payment()
        {
            if (IsMobileRequest())
            {
                return View("Mobile/Payment", new Payment(GetUserID(), Request.Query["id"]));
            }
            else return View("Payment", new Payment(GetUserID(), Request.Query["id"]));
        }
        public ActionResult Pay()
        {
            return View(new Pay(GetUserID(), Request.Query["IDMH"], Request.Query["SoLuong"], Request.Query["ThanhTien"]));
        }
        public ActionResult DeleteFromCart()
        {
            DeleteFromCart delete = new DeleteFromCart(GetUserID(), Request.Query["id"]);
            return View();
        }
        public string GetUserID()
        {
            var ID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ID != null) { return ID; }
            else { return ""; }
        }
        public string GetRoleID()
        {
            string URL = ConnectionURL.User;
            SqlConnection connection = new SqlConnection(URL);
            connection.Open();
            string query = "Select RoleId from AspNetUserRoles where UserId = '" + GetUserID() + "'";
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            SqlDataReader reader = cmd.ExecuteReader();
            string result;
            if (reader.Read())
            {
                result = reader.GetString(0);
            }
            else result = "0";
            connection.Close();
            return result;
        }
        public ActionResult Orders()
        {
            return View(new Orders());
        }
        public ActionResult OrderDetail()
        {
            return View(new OrderDetail(int.Parse(Request.Query["id"])));
        }
        #region API Task
        private async Task<List<Product>> GetAll()
        {
            string apiUrl = "http://192.168.1.20:3001/api/Product";
            var aUrl = Url.Action("GetAll", "Product", new { }, Request.Scheme);
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(aUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        List<Product> productList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
                        return productList;
                    }
                    else
                    {
                        throw new Exception($"Failed to fetch product list. Status code: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new Exception($"HTTP request error: {ex.Message}");
                }
            }
        }
        private async Task<Product> GetProduct(string ID)
        {
            string apiUrl = "http://192.168.1.20:3001/api/Product/" + ID;
            var api = Url.Action("Get", "Product", new { Id = Request.Query["Id"] }, Request.Scheme);
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(api);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        Product product = JsonConvert.DeserializeObject<Product>(jsonContent);
                        return product;
                    }
                    else
                    {
                        throw new Exception($"Failed to fetch product list. Status code: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Handle the case where there's a problem with the HTTP request
                    throw new Exception($"HTTP request error: {ex.Message}");
                }
            }
        }
        private async Task<List<Product>> GetListByBrand(int Count, string Brand)
        {
            if (Brand == null)
                Brand = "%";
            string apiUrl = "https://192.168.1.20:3001/api/Product/" + Count.ToString() + "/" + Brand;
            var api = Url.Action("GetList", "Product", new { Count, Brand }, Request.Scheme);
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(api);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        List<Product> productList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
                        return productList;
                    }
                    else
                    {
                        throw new Exception($"Failed to fetch product list. Status code: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new Exception($"HTTP request error: {ex.Message}");
                }
            }
        }
        private async Task<List<Product>> Search(string Query)
        {
            string apiUrl = "https://192.168.1.20:3001/api/Product/" + Query;
            var api = Url.Action("Search", "Product", new { Query }, Request.Scheme);
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(api);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        List<Product> productList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
                        return productList;
                    }
                    else
                    {
                        throw new Exception($"Failed to fetch product list. Status code: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new Exception($"HTTP request error: {ex.Message}");
                }
            }
        }
        // GET: HomeController/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: HomeController/Create
        [HttpPost]
        public async Task<ActionResult> Create(Product Product)
        {
            string apiUrl = "https://192.168.1.20:3001/api/Product/";
            var api = Url.Action("Create", "Product", new { Product }, Request.Scheme);
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(api, Product);
                    if (response.IsSuccessStatusCode)
                    {
                        return Ok();
                    }
                    else
                    {
                        throw new Exception($"Can not create new product. Status code: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new Exception($"HTTP request error: {ex.Message}");
                }
            }
        }
        // GET: HomeController/Edit/5
        public async Task<ActionResult> Edit()
        {
            try
            {
                return View(new Edit(await GetProduct(Request.Query["Id"])));
            }
            catch (Exception) 
            {
                return BadRequest();
            }
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Product product)
        {
            var api = Url.Action("Update", "Product", new { product.Id, product }, Request.Scheme);
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PutAsJsonAsync(api, product);
                    if (response.IsSuccessStatusCode)
                    {
                        return Redirect("/Home/Detail?id=" + product.Id);
                    }
                    else
                    {
                        throw new Exception($"Failed to update product. Status code: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new Exception($"HTTP request error: {ex.Message}");
                }
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete()
        {
            try
            {
                Delete(int.Parse(Request.Query["Id"]));
                return RedirectToAction("HomePage");
            }
            catch (Exception ex)
            {
                return Redirect("/Home/Detail?id=" + Request.Query["Id"]);
            }
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int Id)
        {
            var api = Url.Action("Delete", "Product", new { Id }, Request.Scheme);
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.DeleteAsync(api);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("HomePage");
                    }
                    else
                    {
                        throw new Exception($"Failed to delete product. Status code: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new Exception($"HTTP request error: {ex.Message}");
                }
            }
        }
        #endregion
    }
}
