namespace Mango.web.Utility
{
    public class SD
    {
        public static string CouponAPIBase { get; set; } = null!;
        public static string AuthAPIBase { get; set; } = null!;
        public static string ProductAPIBase { get; set; } = null!;
        public static string ShoppingCartAPIBase { get; set; } = null!;
        public const string RoleAdmin  = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        public const string TokenCookie  = "JwtToken";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE

        }
    }
}
