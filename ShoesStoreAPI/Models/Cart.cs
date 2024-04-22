#nullable disable
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using ShoesStoreAPI.Class;
using System.Security.Claims;

namespace ShoesStoreAPI.Models
{
    public class CartModel : PageModel
    {
        public string ID { get; set; } = "";
        public List<Product> cart = new List<Product>();
        public string UserID { get; set; }
        public List<Product> GetCart(string UserID)
        {
            List<Product> cart = new List<Product>();
            try
            {
                string str = ConnectionURL.Cart;
                using (SqlConnection connection = new SqlConnection(str))
                {
                    connection.Open();
                    string sql = "select * from [" + UserID + "]";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        Console.WriteLine(cmd.CommandText.ToString());
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
                                product.HinhAnh = reader.GetString(7);
                                product.SoLuong = reader.GetInt32(8);
                                cart.Add(product);
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
            return cart;
        }
        public CartModel()
        {
            OnGet(ID);
        }
        public CartModel(string UserID)
        {
            OnGet(UserID);
            this.UserID = UserID;
        }
        void OnGet(string UserID)
        {
            cart = GetCart(UserID);
        }
    }
}
