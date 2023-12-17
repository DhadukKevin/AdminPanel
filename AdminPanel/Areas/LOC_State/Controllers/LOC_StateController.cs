using AdminPanel.Areas.LOC_Country.Models;
using AdminPanel.Areas.LOC_State.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AdminPanel.BAL;

namespace AdminPanel.Areas.LOC_State.Controllers
{
    [CheckAccess]
    [Area("LOC_State")]
    [Route("LOC_State/[controller]/[action]")]
    public class LOC_StateController : Controller
    {
        #region CONFIGURATION
        private readonly IConfiguration Configuration;
        public LOC_StateController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        #endregion

        #region INDEX
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region SELECTALL
        public IActionResult LOC_StateList()
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");

            #region Country DropDown

            SqlConnection connection1 = new SqlConnection(connectionStr);
            connection1.Open();
            SqlCommand objCmd1 = connection1.CreateCommand();
            objCmd1.CommandType = CommandType.StoredProcedure;
            objCmd1.CommandText = "PR_Country_ComboBox";
            SqlDataReader reader1 = objCmd1.ExecuteReader();
            DataTable dt1 = new DataTable();
            dt1.Load(reader1);
            connection1.Close();

            List<LOC_CountryModel> list = new List<LOC_CountryModel>();

            foreach (DataRow dr in dt1.Rows)
            {
                LOC_CountryModel countryModel = new LOC_CountryModel();
                countryModel.CountryID = Convert.ToInt32(dr["CountryID"]);
                countryModel.CountryName = dr["CountryName"].ToString();
                list.Add(countryModel);
            }
            ViewBag.CountryList = list;

            #endregion

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_State_SelectAll";
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            return View("LOC_StateList", dt);
        }
        #endregion

        #region DELETE
        public IActionResult Delete(int StateID)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_State_DeleteByPK";
            objCmd.Parameters.AddWithValue("@StateID", StateID);
            objCmd.ExecuteNonQuery();
            return RedirectToAction("LOC_StateList");
        }
        #endregion

        #region State Add / Update

        public IActionResult Save(LOC_StateModel model)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;

            if (model.StateID == null)
            {
                objCmd.CommandText = "PR_State_Insert";
                TempData["key"] = "Record Inserted Successfully";
            }
            else
            {
                objCmd.CommandText = "PR_State_UpdateByPK";
                objCmd.Parameters.AddWithValue("@StateID", model.StateID);
                TempData["key"] = "Record Updated Successfully";
            }

            objCmd.Parameters.AddWithValue("@StateName", model.StateName);
            objCmd.Parameters.AddWithValue("@StateCode", model.StateCode);
            objCmd.Parameters.AddWithValue("@CountryId", model.CountryId);
            objCmd.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("LOC_StateList");
        }

        public IActionResult LOC_StateAdd(int? StateID)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");

            #region Country DropDown

            SqlConnection connection1 = new SqlConnection(connectionStr);
            connection1.Open();
            SqlCommand objCmd1 = connection1.CreateCommand();
            objCmd1.CommandType = CommandType.StoredProcedure;
            objCmd1.CommandText = "PR_Country_ComboBox";
            SqlDataReader reader1 = objCmd1.ExecuteReader();
            DataTable dt1 = new DataTable();
            dt1.Load(reader1);
            connection1.Close();

            List<LOC_CountryModel> list = new List<LOC_CountryModel>();

            foreach (DataRow dr in dt1.Rows)
            {
                LOC_CountryModel countryModel = new LOC_CountryModel();
                countryModel.CountryID = Convert.ToInt32(dr["CountryID"]);
                countryModel.CountryName = dr["CountryName"].ToString();
                list.Add(countryModel);
            }
            ViewBag.CountryList = list;

            #endregion

            if (StateID != null)
            {
                TempData["name"] = "State Edit Page";
                TempData["button"] = "Update";
                SqlConnection connection = new SqlConnection(connectionStr);
                connection.Open();
                SqlCommand objCmd = connection.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_State_SelectByPk";
                objCmd.Parameters.AddWithValue("@StateID", StateID);
                DataTable dt = new DataTable();
                SqlDataReader objSDR = objCmd.ExecuteReader();

                dt.Load(objSDR);
                LOC_StateModel model = new LOC_StateModel();
                foreach (DataRow dr in dt.Rows)
                {
                    model.StateID = Convert.ToInt32(dr["StateID"]);
                    model.StateName = (string)dr["StateName"];
                    model.StateCode = (string)dr["StateCode"];
                    model.CountryId = Convert.ToInt32(dr["CountryId"]);
                    model.CountryName = (string)dr["CountryName"];
                }
                return View("LOC_StateAdd", model);
            }
            else
            {
                TempData["button"] = "Add";
                TempData["name"] = "State Add Page";
                return View("LOC_StateAdd");
            }
        }

        #endregion


        #region SEARCH
        public IActionResult LOC_StateSearchByName(string? StateName)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_State_SelectByStateName";
            objCmd.Parameters.AddWithValue("@StateName", StateName);
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            return View("LOC_StateList", dt);
        }
        #endregion

        #region FILTER
        public IActionResult LOC_StateFilter(LOC_StateFilterModel FilterModel)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");

            #region Country DropDown

            SqlConnection connection1 = new SqlConnection(connectionStr);
            connection1.Open();
            SqlCommand objCmd1 = connection1.CreateCommand();
            objCmd1.CommandType = CommandType.StoredProcedure;
            objCmd1.CommandText = "PR_Country_ComboBox";
            SqlDataReader reader1 = objCmd1.ExecuteReader();
            DataTable dt1 = new DataTable();
            dt1.Load(reader1);
            connection1.Close();

            List<LOC_CountryModel> list = new List<LOC_CountryModel>();

            foreach (DataRow dr in dt1.Rows)
            {
                LOC_CountryModel countryModel = new LOC_CountryModel();
                countryModel.CountryID = Convert.ToInt32(dr["CountryID"]);
                countryModel.CountryName = dr["CountryName"].ToString();
                list.Add(countryModel);
            }
            ViewBag.CountryList = list;

            #endregion

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_State_filter";
            objCmd.Parameters.AddWithValue("@CountryID", FilterModel.CountryID);
            objCmd.Parameters.AddWithValue("@StateName", FilterModel.StateName);
            objCmd.Parameters.AddWithValue("@StateCode", FilterModel.StateCode);
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);

            ModelState.Clear();
            return View("LOC_StateList", dt);
        }
        #endregion
    }
}
