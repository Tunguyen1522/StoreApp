using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using ShoesStoreAPI.Class;
using ShoesStoreAPI.Controllers;
using System.Collections.Generic;
using System.Collections;

namespace ShoesStoreAPI.Models
{
    public class HomePage : PageModel
    {
        public List<Product> listNew = new List<Product>();
        public List<Product> listAdidas = new List<Product>();
        public List<Product> listNike = new List<Product>();
        public List<Product> listAirJordan = new List<Product>();
        public List<Product> cart = new List<Product>();
        public int colCount = 5;
        public int rowCount = 1;
        public int lastColIndex = 0;
        public HomePage(List<List<Product>> productlists)
        {
            listNew = productlists[0];
            listAdidas = productlists[1];
            listNike = productlists[2];
            listAirJordan = productlists[3];
        }
        /*
        public void OnGet()
        {
            listNew = getShoes("");
            listAdidas = getShoes("Adidas");
            listNike = getShoes("Nike");
            listAirJordan = getShoes("Ari Jordan");
        }
        public List<Product> getShoes(string NhanHieu)
        {
            List<Product> list = new List<Product>();
            try
            {
                string str = ConnectionURL.Products;
                using (SqlConnection connection = new SqlConnection(str))
                {
                    connection.Open();
                    string sql = "select top(4) * from SanPham where NhanHieu like '%" + NhanHieu + "' order by NgayThem desc";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product product = new Product();
                                product.Id = reader.GetInt32(0);
                                product.Ten = reader.GetString(1);
                                product.NhanHieu = reader.GetString(2);
                                product.TonKho = reader.GetInt32(3);
                                product.MoTa = reader.GetString(4);
                                product.Gia = reader.GetDecimal(5);
                                product.NgayThem = reader.GetDateTime(6).Date;
                                product.HinhAnh = "" + reader.GetString(7);
                                list.Add(product);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            return list;
        }
        */
    }
}
