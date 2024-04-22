using System.Diagnostics;
using System.Data.SqlClient;

namespace ShoesStoreAPI.Class
{
    public class Product
    {
        public Product(int id) { Read(id); }
        public Product() { }
        public int Id { get; set; }
        public string Ten { get; set; }
        public string NhanHieu { get; set; }
        public int TonKho { get; set; }
        public string MoTa { get; set; }
        public decimal Gia { get; set; }
        public DateTime NgayThem { get; set; }
        public string HinhAnh { get; set; }
        public int SoLuong { get; set; }
        public void Create()
        {
            SqlConnection connection = new SqlConnection(ConnectionURL.Products);
            string query = "Insert into SanPham values(" +
                "@Ten, " +
                "@NhanHieu, " +
                "@TonKho, " +
                "@MoTa, " +
                "@Gia, " +
                "getdate(), " +
                "@HinhAnh" +
                ")";
            try
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("Id", Id);
                    cmd.Parameters.AddWithValue("Ten", Ten);
                    cmd.Parameters.AddWithValue("NhanHieu", NhanHieu);
                    cmd.Parameters.AddWithValue("TonKho", TonKho);
                    cmd.Parameters.AddWithValue("MoTa", MoTa);
                    cmd.Parameters.AddWithValue("Gia", Gia);
                    cmd.Parameters.AddWithValue("HinhAnh", HinhAnh);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Read(int id)
        {
            SqlConnection connection = new SqlConnection(ConnectionURL.Products);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "Select * from SanPham where IdSP=" + id;
            try
            {
                connection.Open();
                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Id = reader.GetInt32(0);
                        Ten = reader.GetString(1);
                        NhanHieu = reader.GetString(2);
                        TonKho = reader.GetInt32(3);
                        MoTa = reader.GetString(4);
                        Gia = reader.GetDecimal(5);
                        NgayThem = reader.GetDateTime(6).Date;
                        HinhAnh = reader.GetString(7);
                    }
                }
                catch (Exception)
                {
                    Debugger.Log(1, "Reader Exception", "Can not read data.");
                }
                connection.Close();
            }
            catch (Exception)
            {
                Debugger.Log(1, "Connection Exception", "Can not connect to database.");
            }
        }
        public void Update()
        {
            SqlConnection connection = new SqlConnection(ConnectionURL.Products);
            string query = "Update SanPham Set " +
                "TenSP=@Ten, " +
                "NhanHieu=@NhanHieu, " +
                "TonKho=@TonKho, " +
                "MoTa=@MoTa, " +
                "Gia=@Gia, " +
                "HinhAnh=@HinhAnh" +
                " where IdSP=@Id";
            try
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("Id", Id);
                    cmd.Parameters.AddWithValue("Ten", Ten);
                    cmd.Parameters.AddWithValue("NhanHieu", NhanHieu);
                    cmd.Parameters.AddWithValue("TonKho", TonKho);
                    cmd.Parameters.AddWithValue("MoTa", MoTa);
                    cmd.Parameters.AddWithValue("Gia", Gia);
                    cmd.Parameters.AddWithValue("HinhAnh", HinhAnh);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Delete()
        {
            SqlConnection connection = new SqlConnection(ConnectionURL.Products);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "Delete from SanPham where IdSP=" + Id;
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception)
            {
                Debugger.Log(1, "Connection Exception", "Can not connect to database.");
            }
        }
    }
}
