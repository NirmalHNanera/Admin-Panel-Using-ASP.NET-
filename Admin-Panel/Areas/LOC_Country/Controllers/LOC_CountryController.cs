using AdminPanel.Areas.LOC_Country.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;


namespace AdminPanel.Areas.LOC_Country.Controllers
{

    [Area("LOC_Country")]
    [Route("LOC_Country/[controller]/[action]")]
    public class LOC_CountryController : Controller
    {

        private IConfiguration Configuration;

        public LOC_CountryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region SelectAll
        public IActionResult LOC_CountryList()
        {
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Country_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            connection.Close();
            return View(table);
        }
        #endregion

        #region SelectByID
        public IActionResult LOC_CountryListByID(int CountryID)
        {
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Country_SelectByPK";
            command.Parameters.AddWithValue("CountryID", CountryID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            connection.Close();
            return View(table);
        }
        #endregion

        #region Save
        public IActionResult LOC_CountrySave(LOC_CountryModel lOC_CountryModel, int CountryID = 0)
        {
            if (string.IsNullOrWhiteSpace(lOC_CountryModel.CountryName))
            {
                ModelState.AddModelError("CountryName", "Country Name is required.");
                return View("LOC_CountryAddEdit", lOC_CountryModel);
            }
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            if (CountryID == 0)
            {
                command.CommandText = "PR_Country_Insert";
                TempData["msg"] = "Record Inserted Succesfully";
                
            }
            else
            {
                command.CommandText = "PR_Country_UpdateByPK";
                command.Parameters.AddWithValue("@CountryID", CountryID);   
                TempData["msg"] = "Record Updated Succesfully";

            }
            command.Parameters.AddWithValue("@CountryName", lOC_CountryModel.CountryName);
            command.Parameters.AddWithValue("@CountryCode", lOC_CountryModel.CountryCode);


            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("LOC_CountryList");
        }
        #endregion

        #region Delete
        public IActionResult LOC_CountryDelete(int CountryID)
        {
            try
            {
                string connectionString = this.Configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Country_DeleteByPK";
                command.Parameters.AddWithValue("@CountryID", CountryID);
                command.ExecuteNonQuery();
                connection.Close();
                return RedirectToAction("LOC_CountryList");
            }
            catch (Exception Ex)
            {
                return RedirectToAction("LOC_CountryList");
            }
        }
        #endregion

        #region Add
        public IActionResult LOC_CountryAdd(int CountryID = 0)
        {
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Country_SelectByPK";
            command.Parameters.AddWithValue("@CountryID", CountryID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            LOC_CountryModel lOC_CountryModel = new LOC_CountryModel();
            foreach (DataRow dataRow in table.Rows)
            {
                lOC_CountryModel.CountryID = Convert.ToInt32(dataRow["CountryID"]);
                lOC_CountryModel.CountryName = dataRow["CountryName"].ToString();
                lOC_CountryModel.CountryCode = dataRow["CountryCode"].ToString();
            }
            return View("LOC_CountryAddEdit", lOC_CountryModel);
        }
        #endregion

        #region Search
        public IActionResult LOC_CountrySearch(string? searchKeyword)
        {

            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand command = conn.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Country_Search";
            command.Parameters.AddWithValue("@SearchKeyword", searchKeyword);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            conn.Close();
            //return View("LOC_CountryList", table);
            return View("LOC_CountryList", table);
        }

        #endregion

    }
}
