using Dashboard.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Dashboard.Controllers
{
    public class LoginController : Controller
    {
        Executer _dbExecuter = new Executer();
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public class LoginModel
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        public JsonResult UserLogin([FromBody] LoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.UserId) || string.IsNullOrEmpty(loginModel.Password))
            {
                return Json(new { success = false, message = "User details are empty." });
            }

            string query = "SELECT * FROM USERMASTER WHERE User_ID = '"+ loginModel.UserId + "' AND Password = '"+ loginModel.Password + "' AND Status = 'Y'";
            DataTable dt = _dbExecuter.GetDataTable(query);

            if (dt.Rows.Count > 0)
            {
                return Json(new { success = true, message = "Login Successful." });
            }
            else
            {
                return Json(new { success = false, message = "Invalid Credentials." });
            }
        }

        [HttpPost]
        public JsonResult UserSignin([FromBody] LoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.UserId) || string.IsNullOrEmpty(loginModel.Password))
            {
                return Json(new { success = false, message = "User details are empty." });
            }

            string query = "SELECT * FROM UserMaster WHERE User_ID = '" + loginModel.UserId + "'";
            DataTable dt = _dbExecuter.GetDataTable(query);

            if (dt.Rows.Count > 0)
            {
                return Json(new { success = false, message = "This user already exists." });
            }
            else
            {
                string insertQuery = "INSERT INTO UserMaster (User_ID, User_Name, Password, Status) VALUES ('"+ loginModel.UserId + "', '"+ loginModel.UserName + "', '"+ loginModel.Password + "', 'Y')";
                bool isInserted = _dbExecuter.ExecuteNonQuery(insertQuery);

                if (isInserted)
                {
                    return Json(new { success = true, message = "User created successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to create user." });
                }
            }
        }

        [HttpPost]
        public JsonResult UserPasswordUpdate([FromBody] PasswordUpdateModel passwordUpdateModel)
        {
            if (passwordUpdateModel.NewPassword != passwordUpdateModel.ConfirmPassword)
            {
                return Json(new { success = false, message = "Password and confirm password do not match." });
            }

            string query = "SELECT * FROM UserMaster WHERE User_ID = '" + passwordUpdateModel.UserId + "'";
            DataTable dt = _dbExecuter.GetDataTable(query);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Status"].ToString() != "Y")
                {
                    return Json(new { success = false, message = "User state is inactive. Please contact admin." });
                }

                string updateQuery = "UPDATE UserMaster SET Password = '"+ passwordUpdateModel.NewPassword + "' WHERE User_ID = '"+ passwordUpdateModel.NewPassword + "'";
                bool isUpdated = _dbExecuter.ExecuteNonQuery(updateQuery);

                if (isUpdated)
                {
                    return Json(new { success = true, message = "Password updated successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Password update failed." });
                }
            }
            else
            {
                return Json(new { success = false, message = "Please enter a valid user." });
            }
        }

        public class PasswordUpdateModel
        {
            public string UserId { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmPassword { get; set; }
        }
    }
}
