namespace AdminPanel.DAL
{
    public class DALHelper
    {
        #region Connection String
        public static string ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("myConnectionString");
        #endregion
    }
}
