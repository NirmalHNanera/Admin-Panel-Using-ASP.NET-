using AdminPanel.Areas.MST_Student.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration; 
using AdminPanel.Areas.LOC_Country.Models;
using AdminPanel.Areas.LOC_State.Models;
using AdminPanel.Areas.LOC_City.Models;
using AdminPanel.Areas.MST_Branch.Models;
using AdminPanel.Areas.MST_Student.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.Data.SqlClient;

namespace AdminPanel.Areas.MST_Student.Controllers
{
    [Area("MST_Student")]
    [Route("MST_Student/[controller]/[action]")]
    public class MST_StudentController : Controller
    {
        private IConfiguration Configuration;

        public MST_StudentController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        #region SelectAll
        public IActionResult MST_StudentList()
        {
            Branch_DropDown();
            //State_DropDown();
            //City_DropDown();
            Country_DropDown();
            ViewBag.StateList = FillStateByCountry(null);
            ViewBag.CityList = FillCityByState(null);


            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Student_SelectAll"; // Replace with your stored procedure name
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            connection.Close();
            return View(table);
        }
        #endregion
        #region SelectByID
        public IActionResult MST_StudentListByID(int StudentID)
        {
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Student_SelectByPK";
            command.Parameters.AddWithValue("StudentID", StudentID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            connection.Close();
            return View(table);
        }
        #endregion
        #region Save
        public IActionResult MST_StudentSave(MST_StudentModel mst_StudentModel)
        {
            //Photo Upload

            if (mst_StudentModel.File != null)
            {
                string FilePath = "wwwroot\\Upload";
                string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileNameWithPath = Path.Combine(path, mst_StudentModel.File.FileName);
                mst_StudentModel.Photopath = FilePath.Replace("wwwroot\\", "/") + "/" + mst_StudentModel.File.FileName;
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    mst_StudentModel.File.CopyTo(stream);
                }
            }
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            if (mst_StudentModel.StudentID == null)
            {
                command.CommandText = "PR_Student_Insert";

            }
            else
            {
                command.CommandText = "PR_Student_UpdateByPK";
                command.Parameters.AddWithValue("@StudentID", mst_StudentModel.StudentID);

            }

            command.Parameters.AddWithValue("@StudentName", mst_StudentModel.StudentName);
            command.Parameters.AddWithValue("@BranchID", mst_StudentModel.BranchID);
            command.Parameters.AddWithValue("@CityID", mst_StudentModel.CityID);
            command.Parameters.AddWithValue("@MobileNoStudent", mst_StudentModel.MobileNoStudent);
            command.Parameters.AddWithValue("@Email", mst_StudentModel.Email);
            command.Parameters.AddWithValue("@MobileNoFather", mst_StudentModel.MobileNoFather);
            command.Parameters.AddWithValue("@Address", mst_StudentModel.Address);
            command.Parameters.AddWithValue("@BirthDate", mst_StudentModel.BirthDate);
            command.Parameters.AddWithValue("@Age", mst_StudentModel.Age);
            command.Parameters.AddWithValue("@IsActive", mst_StudentModel.IsActive);
            command.Parameters.AddWithValue("@Gender", mst_StudentModel.Gender);
            command.Parameters.AddWithValue("@Password", mst_StudentModel.Password);
            command.Parameters.AddWithValue("@PhotoPath", mst_StudentModel.Photopath);
            command.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("MST_StudentList");
        }
        #endregion

        #region Delete
        public IActionResult MST_StudentDelete(int StudentID)
        {
            try
            {
                string connectionString = this.Configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Student_DeleteByPK";
                command.Parameters.AddWithValue("@StudentID", StudentID);
                command.ExecuteNonQuery();
                connection.Close();
                return RedirectToAction("MST_StudentList");
            }
            catch (Exception Ex)
            {
                return RedirectToAction("MST_StudentList");
            }
        }
        #endregion

        #region Add
        public IActionResult MST_StudentAdd(int StudentID = 0)
        {

            Branch_DropDown();
            City_DropDown();
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");

            #region Branch ComboBox

            List<SelectListItem> genderList = new List<SelectListItem>
{
    new SelectListItem { Value = "Male", Text = "Male" },
    new SelectListItem { Value = "Female", Text = "Female" },
    new SelectListItem { Value = "Other", Text = "Other" }
};

            ViewBag.GenderList = genderList;

            #endregion
            #region Add
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Student_SelectByPK";
            command.Parameters.AddWithValue("@StudentID", StudentID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            MST_StudentModel mst_StudentModel = new MST_StudentModel();
            foreach (DataRow dataRow in table.Rows)
            {
                mst_StudentModel.StudentID = Convert.ToInt32(dataRow["StudentID"]);
                mst_StudentModel.StudentName = dataRow["StudentName"].ToString();
                mst_StudentModel.BranchID = Convert.ToInt32(dataRow["BranchID"]);
                mst_StudentModel.CityID = Convert.ToInt32(dataRow["CityID"]);
                mst_StudentModel.MobileNoStudent = dataRow["MobileNoStudent"].ToString();
                mst_StudentModel.Email = dataRow["Email"].ToString();
                mst_StudentModel.MobileNoFather = dataRow["MobileNoFather"].ToString();
                mst_StudentModel.Address = dataRow["Address"].ToString();
                mst_StudentModel.BirthDate = Convert.ToDateTime(dataRow["BirthDate"]);
                mst_StudentModel.Age = Convert.ToInt32(dataRow["Age"]);
                mst_StudentModel.IsActive = Convert.ToBoolean(dataRow["IsActive"]);
                mst_StudentModel.Gender = dataRow["Gender"].ToString();
                mst_StudentModel.Password = dataRow["Password"].ToString();
                mst_StudentModel.Photopath = dataRow["PhotoPath"].ToString();
            }
            return View("MST_StudentAddEdit", mst_StudentModel);
            #endregion
        }
        #endregion

        #region Search
        public IActionResult Search(string? StudentName, 
            string? Email, 
            int? Age, 
            DateTime? BirthDate,
            int? CityID, int?
            CountryID, 
            int? StateID, int? BranchID, 
            string? MobileNoStudent, string? Gender, bool? IsActive, DateTime? JoinEndDate, DateTime? JoinStartDate)
        {

            Branch_DropDown();
            Country_DropDown();
            ViewBag.StateList = FillStateByCountry(null);
            ViewBag.CityList = FillCityByState(null);
            string ConnectionString = Configuration.GetConnectionString("ConnectionString");
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Student_Search";
            objCmd.Parameters.AddWithValue("@StudentName", StudentName);
            objCmd.Parameters.AddWithValue("@Email", Email);
            objCmd.Parameters.AddWithValue("@Age", Age);
            objCmd.Parameters.AddWithValue("@BirthDate", BirthDate);
            objCmd.Parameters.AddWithValue("@CityID", CityID);
            objCmd.Parameters.AddWithValue("@CountryID", CountryID);
            objCmd.Parameters.AddWithValue("@StateID", StateID);
            objCmd.Parameters.AddWithValue("@BranchID", BranchID);
            objCmd.Parameters.AddWithValue("@MobileNoStudent", MobileNoStudent);
            objCmd.Parameters.AddWithValue("@Gender", Gender);
            objCmd.Parameters.AddWithValue("@IsActive", IsActive);
            objCmd.Parameters.AddWithValue("@JoinStartDate", JoinStartDate);
            objCmd.Parameters.AddWithValue("@JoinEndDate", JoinEndDate);
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            conn.Close();
            return View("MST_StudentList", dt);

        }
        #endregion

        public void City_DropDown()
        {
            #region City ComboBox
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = CommandType.StoredProcedure;
            command1.CommandText = "PR_City_ComboBox";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable table1 = new DataTable();
            table1.Load(reader1);
            connection1.Close();

            List<LOC_CityDropDownModel> list = new List<LOC_CityDropDownModel>();
            foreach (DataRow row in table1.Rows)
            {
                LOC_CityDropDownModel loc_CityDropDownModel = new LOC_CityDropDownModel();
                loc_CityDropDownModel.CityID = Convert.ToInt32(row["CityID"]);
                loc_CityDropDownModel.CityName = row["CityName"].ToString();
                list.Add(loc_CityDropDownModel);
            }
            ViewBag.CityList = list;
            #endregion
        }
        #region Branch-Dropdown
        public void Branch_DropDown()
        {
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");

            SqlConnection connection2 = new SqlConnection(connectionString);
            connection2.Open();
            SqlCommand command2 = connection2.CreateCommand();
            command2.CommandType = CommandType.StoredProcedure;
            command2.CommandText = "PR_Branch_ComboBox";
            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable table2 = new DataTable();
            table2.Load(reader2);
            connection2.Close();

            List<MST_BranchDropDownModel> list2 = new List<MST_BranchDropDownModel>();
            foreach (DataRow row in table2.Rows)
            {
                MST_BranchDropDownModel mst_BranchDropDownModel = new MST_BranchDropDownModel();
                mst_BranchDropDownModel.BranchID = Convert.ToInt32(row["BranchID"]);
                mst_BranchDropDownModel.BranchName = row["BranchName"].ToString();
                list2.Add(mst_BranchDropDownModel);
            }
            ViewBag.BranchList = list2;     
        }
        #endregion

        #region DropDownByCountry
        public IActionResult DropDownByCountry(int? CountryID)
        {
            var vModel = FillStateByCountry(CountryID);
            return Json(vModel);
        }
        #endregion

        #region FillStateByCountry
        public List<LOC_StateDropDownModel> FillStateByCountry(int? CountryID)
        {
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            DataTable table = new DataTable();
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_State_SelectComboBoxByCountryName";
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            table.Load(sqlDataReader);
            sqlConnection.Close();

            List<LOC_StateDropDownModel> list = new List<LOC_StateDropDownModel>();
            foreach (DataRow dataRow in table.Rows)
            {
                LOC_StateDropDownModel lOC_StateDropDownModel = new LOC_StateDropDownModel();
                lOC_StateDropDownModel.StateID = Convert.ToInt32(dataRow["StateID"]);
                lOC_StateDropDownModel.StateName = dataRow["StateName"].ToString();
                list.Add(lOC_StateDropDownModel);
            }

            return list;
        }
        #endregion
        #region DropDownByCountry
        public IActionResult DropDownByState(int? StateID)
        {
            var vModel = FillCityByState(StateID);
            return Json(vModel);
        }
        #endregion

        #region FillStateByCountry
        public List<LOC_CityDropDownModel> FillCityByState(int? StateID)
        {
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            DataTable table = new DataTable();
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_City_ComboBox";
            cmd.Parameters.AddWithValue("@StateID", StateID);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            table.Load(sqlDataReader);
            sqlConnection.Close();

            List<LOC_CityDropDownModel> list = new List<LOC_CityDropDownModel>();
            foreach (DataRow dataRow in table.Rows)
            {
                LOC_CityDropDownModel loc_CityDropDownModel = new LOC_CityDropDownModel();
                loc_CityDropDownModel.CityID = Convert.ToInt32(dataRow["CityID"]);
                loc_CityDropDownModel.CityName = dataRow["CityName"].ToString();
                list.Add(loc_CityDropDownModel);
            }

            return list;
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

        public IActionResult MST_StudentViewProfile(int? StudentID)
        {
            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Student_SelectByPK";
            command.Parameters.AddWithValue("StudentID", StudentID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            connection.Close();
            return View("MST_StudentViewProfile",table);
        }


        #region State_DropDown
        public void State_DropDown(int? StateID = null)
        {

            string connectionString = this.Configuration.GetConnectionString("ConnectionString");
            SqlConnection connection2 = new SqlConnection(connectionString);
            connection2.Open();
            SqlCommand command2 = connection2.CreateCommand();
            command2.CommandType = CommandType.StoredProcedure;
            command2.CommandText = "PR_State_ComboBox";
            command2.Parameters.AddWithValue("@StateID", StateID);

            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable table2 = new DataTable();
            table2.Load(reader2);

            List<LOC_StateDropDownModel> list2 = new List<LOC_StateDropDownModel>();
            foreach (DataRow row in table2.Rows)
            {
                LOC_StateDropDownModel model = new LOC_StateDropDownModel();
                model.StateID = Convert.ToInt32(row["StateID"]);
                model.StateName = row["StateName"].ToString();
                list2.Add(model);
            }
            ViewBag.StateList = list2;
        }
        #endregion
        

    }
}
