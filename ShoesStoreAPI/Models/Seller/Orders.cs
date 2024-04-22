using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using ShoesStoreAPI.Class;

namespace ShoesStoreAPI.Models.Seller
{
    public class Orders : PageModel
    {
        public List<Customer> CustomerList = new List<Customer>();
        public Orders() 
        {
            OnGet();
        }
        public void GetCustomerInfo(Customer customer)
        {
            Customer c = new Customer();
            try
            {
                string str = ConnectionURL.User;
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();
                    string sql = "select [Email], [UserName], [PhoneNumber] " +
                        "from [AspNetUsers] " +
                        "where [Id] = '" + customer.Id + "'";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                c.Email = reader.GetString(0);
                                c.UserName = reader.GetString(1);
                                c.PhoneNumber = reader.GetString(2);
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
            customer.PhoneNumber = c.PhoneNumber;
            customer.Email = c.Email;
            customer.UserName = c.UserName;
        }

        public void OnGet() 
        {
            try
            {
                string str = ConnectionURL.Products;
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();
                    string sql = "select DISTINCT IDKH from DonHang";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Customer customer = new Customer();
                                customer.Id = reader.GetString(0);
                                GetCustomerInfo(customer);
                                customer.CustomerOrders = GetCustomerOrders(customer.Id);
                                CustomerList.Add(customer);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public List<string> GetCustomerOrders(string UserID)
        {
            List<string> orders = new List<string>();
            string URL = ConnectionURL.Products;
            SqlConnection sqlConnection = new SqlConnection(URL);
            SqlCommand command = sqlConnection.CreateCommand();
            command.CommandText = "select ID from DonHang where IDKH = '" + UserID + "'";
            try
            {
                sqlConnection.Open();
                using (SqlDataReader r = command.ExecuteReader())
                {
                    while (r.Read())
                    {
                        orders.Add("" + r.GetInt32(0));
                    }    
                }
                sqlConnection.Close();

            }
            catch (Exception) {
                throw;
            }
            return orders;
        }
    }
    public class Customer
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List <string> CustomerOrders { get; set;}
    }
}
