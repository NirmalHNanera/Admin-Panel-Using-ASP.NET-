using AdminPanel.Areas.LOC_Country.Models;
using AdminPanel.Areas.MST_Branch.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;

namespace AdminPanel.Areas.MST_Branch.Controllers
{

    [Area("MST_Branch")]
    [Route("MST_Branch/[controller]/[action]")]
    public class MST_BranchController : Controller
    {

        private IConfiguration Configuration;

        public MST_BranchController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region SelectAll
        public IActionResult MST_BranchList()
        {
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Branch_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            connection.Close();
            return View(table);
        }
        #endregion

        #region SelectByID
        public IActionResult LOC_BranchListByID(int BranchID)
        {
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Branch_SelectByPK";
            command.Parameters.AddWithValue("BranchID", BranchID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            connection.Close();
            return View(table);
        }
        #endregion

        #region Save
        public IActionResult MST_BranchSave(MST_BranchModel mst_BranchModel, int BranchID = 0)
        {
            //if (string.IsNullOrWhiteSpace(mst_BranchModel.BranchName))
            //{
            //    ModelState.AddModelError("BranchName", "BranchName is required.");
            //    return View("MST_BranchAddEdit", mst_BranchModel);
            //}

            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            if (BranchID == 0)
            {
                command.CommandText = "PR_Branch_Insert";
            }
            else
            {
                command.CommandText = "PR_Branch_UpdateByPK";
                command.Parameters.AddWithValue("@BranchID", BranchID);
            }
            command.Parameters.AddWithValue("@BranchName", mst_BranchModel.BranchName);
            command.Parameters.AddWithValue("@BranchCode", mst_BranchModel.BranchCode);
            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("MST_BranchList");
        }
        #endregion

        #region Delete
        public IActionResult MST_BranchDelete(int BranchID)
        {
            try
            {
                string connectionString = this.Configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Branch_DeleteByPK";
                command.Parameters.AddWithValue("@BranchID", BranchID);
                command.ExecuteNonQuery();
                connection.Close();
                return RedirectToAction("MST_BranchList");
            }
            catch (Exception Ex)
            {
                return RedirectToAction("MST_BranchList");
            }
        }
        #endregion

        #region Add
        public IActionResult MST_BranchAdd(int BranchID = 0)
        {
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Branch_SelectByPK";
            command.Parameters.AddWithValue("@BranchID", BranchID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            MST_BranchModel mst_BranchModel = new MST_BranchModel();
            foreach (DataRow dataRow in table.Rows)
            {
                mst_BranchModel.BranchID = Convert.ToInt32(dataRow["BranchID"]);
                mst_BranchModel.BranchName = dataRow["BranchName"].ToString();
                mst_BranchModel.BranchCode = dataRow["BranchCode"].ToString();
            }
            return View("MST_BranchAddEdit", mst_BranchModel);
        }
        #endregion

        #region Search
        public IActionResult MST_BranchSearch(string? searchKeyword)
        {
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand command = conn.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Branch_Search";
            command.Parameters.AddWithValue("@SearchKeyword", searchKeyword);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            conn.Close();
            return View("MST_BranchList", table);
        }

        #endregion
    }
}
