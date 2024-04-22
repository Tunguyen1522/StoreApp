namespace ShoesStoreAPI.Class
{
    public class ConnectionURL
    {
        static string DataSource = "LAPTOP-RG8KF39B\\SQLEXPRESS";
        static string URL(string Catalog)
        {
            return "Data Source=" + DataSource + ";Initial Catalog=" + Catalog + ";User ID=sa;Password=12345;Integrated Security=True;Pooling=False;TrustServerCertificate=True";
        }
        public static readonly string Cart = URL("GioHang");
        public static readonly string User = URL("User");
        public static readonly string Products = URL("SellerDB");
    }
}
