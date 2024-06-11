using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AdminPanel.Areas.LOC_State.Models;
using System.Collections.Generic;
using AdminPanel.Areas.LOC_Country.Models;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace AdminPanel.Areas.LOC_State.Controllers
{
    [Area("LOC_State")]
    [Route("LOC_State/[controller]/[action]")]
    public class LOC_StateController : Controller
    {
        private IConfiguration Configuration;

        public LOC_StateController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        #region SelectAll
        public IActionResult LOC_StateList()
        {
            Country_DropDown();

            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_State_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            connection.Close();
            return View(table);
        }
        #endregion
        #region Delete
        public IActionResult LOC_StateDelete(int StateID)
        {
            try
            {
                string connectionString = this.Configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_State_DeleteByPK";
                command.Parameters.AddWithValue("@StateID", StateID);
                command.ExecuteNonQuery();
                connection.Close();
                return RedirectToAction("LOC_StateList");
                TempData["msg"] = "Record Deleted Succesfully";
            }
            catch (Exception Ex)
            {
                return RedirectToAction("LOC_StateList");

            }
        }
        #endregion
        #region SelectByID
        public IActionResult LOC_StateListByID(int StateID)
        {
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_State_SelectByPK";
            command.Parameters.AddWithValue("StateID", StateID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            connection.Close();
            return View(table);
        }
        #endregion

        #region Save
        public IActionResult LOC_StateSave(LOC_StateModel lOC_StateModel, int StateID = 0)
        {
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            if (StateID == 0)
            {
                command.CommandText = "PR_State_Insert";
                TempData["msg"] = "Record Inserted Succesfully";
            }
            else
            {
                command.CommandText = "PR_State_UpdateByPK";
                command.Parameters.AddWithValue("@StateID", StateID);
                TempData["msg"] = "Record Updated Succesfully";

            }
            command.Parameters.AddWithValue("@StateName", lOC_StateModel.StateName);
            command.Parameters.AddWithValue("@StateCode", lOC_StateModel.StateCode);
            command.Parameters.AddWithValue("@CountryID", lOC_StateModel.CountryID);
            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("LOC_StateList");
        }
        #endregion

        #region Add
        public IActionResult LOC_StateAdd(int StateID = 0)
        {

            Country_DropDown();
            #region Add
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_State_SelectByPK";
            command.Parameters.AddWithValue("@StateID", StateID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            LOC_StateModel lOC_StateModel = new LOC_StateModel();
            foreach (DataRow dataRow in table.Rows)
            {
                lOC_StateModel.StateID = Convert.ToInt32(dataRow["StateID"]);
                lOC_StateModel.StateName = dataRow["StateName"].ToString();
                lOC_StateModel.StateCode = dataRow["StateCode"].ToString();
                lOC_StateModel.CountryID = Convert.ToInt32(dataRow["CountryID"]);
            }
            return View("LOC_StateAddEdit", lOC_StateModel);
            #endregion
        }
        #endregion

        #region Search
        public IActionResult LOC_StateSearch(string? StateName, int? CountryID)
        {
            Country_DropDown();
           

            ViewBag.StateName = StateName;
            ViewBag.CountryID = CountryID;
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand command = conn.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_State_Search";
            command.Parameters.AddWithValue("@StateName", StateName);
            command.Parameters.AddWithValue("@CountryID", CountryID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            conn.Close();
            return View("LOC_StateList", table);




        }

        #endregion

        #region StateSearchByCountryName
        public IActionResult LOC_StateSearchByCountry(string? searchKeyword)
        {
            ViewBag.SearchKeyword = searchKeyword;
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand command = conn.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_State_SearchByCountry";
            command.Parameters.AddWithValue("@SearchKeyword", searchKeyword);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            conn.Close();
            return PartialView("LOC_StateList", table);
        }
        #endregion

        #region country-ComboBox
        public void Country_DropDown()
        {

            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection connection2 = new SqlConnection(connectionString);
            connection2.Open();
            SqlCommand command2 = connection2.CreateCommand();
            command2.CommandType = CommandType.StoredProcedure;
            command2.CommandText = "PR_Country_ComboBox";
            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable table2 = new DataTable();
            table2.Load(reader2);

            List<LOC_CountryDropDownModel> list2 = new List<LOC_CountryDropDownModel>();
            foreach (DataRow row in table2.Rows)
            {
                LOC_CountryDropDownModel lOC_CountryDropDownModel = new LOC_CountryDropDownModel();
                lOC_CountryDropDownModel.CountryID = Convert.ToInt32(row["CountryID"]);
                lOC_CountryDropDownModel.CountryName = row["CountryName"].ToString();
                list2.Add(lOC_CountryDropDownModel);
            }
            ViewBag.CountryList = list2;
        }
        #endregion
    }
}
