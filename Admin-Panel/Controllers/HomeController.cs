using Admin_Panel.BAL;
using Admin_Panel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace Admin_Panel.Controllers

{
    [CheckAccess]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        //public IActionResult Index()
        //{
        //    //string username = HttpContext.Session.GetString("Username");
        //    //TempData["Username"] = username;
        //    //return View();

        //}
        public IActionResult Index()
        {
            var connectionString = _configuration.GetConnectionString("ConnectionString"); 
            var model = new DashboardViewModel();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("GetTotalCounts", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    model.TotalCountries = (int)reader["TotalCountries"];
                    model.TotalStates = (int)reader["TotalStates"];
                    model.TotalCities = (int)reader["TotalCities"];
                    model.TotalBranches = (int)reader["TotalBranches"];
                    model.TotalStudents = (int)reader["TotalStudents"];
                }

                connection.Close();
            }

            return View(model);
        }
        [Route("/Error")]
        [Route("/Error/{id?}")]
        public IActionResult Error(int statuscode)
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult FAQ()
        {
            return View();
        }
        public IActionResult My_Profile()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login1()
        {
            return View();
        }
        public IActionResult Account_Setting()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}