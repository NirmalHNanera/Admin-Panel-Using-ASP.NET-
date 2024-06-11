using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System;


namespace SessionLoginExample.Controllers
{

    public class AccountController : Controller
    {
        
        private readonly IConfiguration ConnectionString;

        public AccountController(IConfiguration _configuration)
        {
            ConnectionString = _configuration;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                

                using (SqlCommand command = new SqlCommand("PR_SEC_User_SelectByUserNamePassword", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserName", username);
                    command.Parameters.AddWithValue("@Password", password); 
                   
                    var result = command.ExecuteScalar();
                    if (result != null && Convert.ToBoolean(result))
                    {
                        HttpContext.Session.SetString("Username", username);
                        HttpContext.Session.SetString("Password", password);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid credentials. Please try again.";
                        return RedirectToAction("Login"); // Redirect to the login view
                    }
                }
            }

            //ViewBag.ErrorMessage = "Invalid credentials. Please try again.";
            //return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Login", "Account");
        }
    }
}
