using Microsoft.AspNetCore.Mvc;
using ShoesStoreAPI.Class;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace ShoesStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }
        [HttpGet("{Count:int}/{Brand}")]
        public List<Product> GetList(int Count, string Brand)
        {
            List<Product> list = new List<Product>();
            try
            {
                string str = ConnectionURL.Products;
                using (SqlConnection connection = new SqlConnection(str))
                {
                    connection.Open();
                    string sql = "select top(" + Count.ToString() + ") * from SanPham " +
                        "where NhanHieu like '%" + Brand + "%' " +
                        "order by NgayThem desc";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product sanPham = new Product();
                                sanPham.Id = reader.GetInt32(0);
                                sanPham.Ten = reader.GetString(1);
                                sanPham.NhanHieu = reader.GetString(2);
                                sanPham.TonKho = reader.GetInt32(3);
                                sanPham.MoTa = reader.GetString(4);
                                sanPham.Gia = reader.GetDecimal(5);
                                sanPham.NgayThem = reader.GetDateTime(6).Date;
                                sanPham.HinhAnh = reader.GetString(7);
                                list.Add(sanPham);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return list;
        }
        [HttpGet("{Query}")]
        public List<Product> Search(string Query)
        {
            List<Product> list = SearchByBrand(Query).Concat(SearchByName(Query)).ToList();
            return list;
        }
        private List<Product> SearchByBrand(string Brand)
        {
            int Count = 1000;
            List<Product> list = new List<Product>();
            try
            {
                string str = ConnectionURL.Products;
                if (Brand != "%")
                {
                    using (SqlConnection connection = new SqlConnection(str))
                    {
                        connection.Open();
                        string sql = "select top(" + Count.ToString() + ") * from SanPham " +
                            "where NhanHieu like '%" + Brand + "%' " +
                            "order by NgayThem desc";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Product sanPham = new Product();
                                    sanPham.Id = reader.GetInt32(0);
                                    sanPham.Ten = reader.GetString(1);
                                    sanPham.NhanHieu = reader.GetString(2);
                                    sanPham.TonKho = reader.GetInt32(3);
                                    sanPham.MoTa = reader.GetString(4);
                                    sanPham.Gia = reader.GetDecimal(5);
                                    sanPham.NgayThem = reader.GetDateTime(6).Date;
                                    sanPham.HinhAnh = reader.GetString(7);
                                    list.Add(sanPham);
                                }
                            }
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return list;
        }
        private List<Product> SearchByName(string Name)
        {
            int Count = 1000;
            List<Product> list = new List<Product>();
            try
            {
                string str = ConnectionURL.Products;
                using (SqlConnection connection = new SqlConnection(str))
                {
                    connection.Open();
                    string sql = "select top(" + Count.ToString() + ") * from SanPham " +
                        "where TenSP like '%" + Name + "%' " +
                        "order by NgayThem desc";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product sanPham = new Product();
                                sanPham.Id = reader.GetInt32(0);
                                sanPham.Ten = reader.GetString(1);
                                sanPham.NhanHieu = reader.GetString(2);
                                sanPham.TonKho = reader.GetInt32(3);
                                sanPham.MoTa = reader.GetString(4);
                                sanPham.Gia = reader.GetDecimal(5);
                                sanPham.NgayThem = reader.GetDateTime(6).Date;
                                sanPham.HinhAnh = reader.GetString(7);
                                list.Add(sanPham);
                            }
                        }
                    }
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return list;
        }
        [HttpGet(Name = "GetAll")]
        public List<Product> GetAll()
        {
            List<Product> list = new List<Product>();
            try
            {
                string str = ConnectionURL.Products;
                using (SqlConnection connection = new SqlConnection(str))
                {
                    connection.Open();
                    string sql = "select * from SanPham";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product sanPham = new Product();
                                sanPham.Id = reader.GetInt32(0);
                                sanPham.Ten = reader.GetString(1);
                                sanPham.NhanHieu = reader.GetString(2);
                                sanPham.TonKho = reader.GetInt32(3);
                                sanPham.MoTa = reader.GetString(4);
                                sanPham.Gia = reader.GetDecimal(5);
                                sanPham.NgayThem = reader.GetDateTime(6).Date;
                                sanPham.HinhAnh = reader.GetString(7);
                                list.Add(sanPham);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return list;
        }
        [HttpGet("{Id:int}")]
        public ActionResult<Product> Get(int Id)
        {
            return new Product(Id);
        }
        [HttpDelete("{Id}")]
        public ActionResult Delete(int Id)
        {
            Product sanPham = new Product(Id);
            sanPham.Delete();
            return NoContent();
        }
        [HttpPost]
        public IActionResult Create([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            try
            {
                product.Create();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product product)
        {
            if (product == null || id != product.Id)
            {
                return BadRequest();
            }

            try
            {
                product.Update();
                return NoContent(); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

