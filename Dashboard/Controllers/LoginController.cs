using Dashboard.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

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

        //[HttpPost]
        //public JsonResult UserLogin([FromBody] LoginModel loginModel)
        //{
        //    if (string.IsNullOrEmpty(loginModel.UserId) || string.IsNullOrEmpty(loginModel.Password))
        //    {
        //        return Json(new { success = false, message = "User details are empty." });
        //    }

        //    string query = "SELECT * FROM USERMASTER WHERE User_ID = '"+ loginModel.UserId + "' AND Password = '"+ loginModel.Password + "' AND Status = 'Y'";
        //    DataTable dt = _dbExecuter.GetDataTable(query);

        //    if (dt.Rows.Count > 0)
        //    {
        //        return Json(new { success = true, message = "Login Successful." });
        //    }
        //    else
        //    {
        //        return Json(new { success = false, message = "Invalid Credentials." });
        //    }
        //}

        [HttpPost]
        public JsonResult UserLogin([FromBody] LoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.UserId) || string.IsNullOrEmpty(loginModel.Password))
            {
                return Json(new { success = false, message = "User details are empty." });
            }

            string query = "SELECT User_ID, User_Name FROM UserMaster WHERE User_ID = @UserId AND Password = @Password AND Status = 'Y'";
            DataTable dt = _dbExecuter.GetDataTable(query,
                new SqlParameter("@UserId", loginModel.UserId),
                new SqlParameter("@Password", loginModel.Password));

            if (dt.Rows.Count > 0)
            {
                // Fetch user details
                var user = new
                {
                    UserId = dt.Rows[0]["User_ID"].ToString(),
                    UserName = dt.Rows[0]["User_Name"].ToString()
                };
                HttpContext.Session.SetString("userName", user.UserName);
                HttpContext.Session.SetString("userId", user.UserId);

                return Json(new { success = true, message = "Login Successful.", user });
            }
            else
            {
                return Json(new { success = false, message = "Invalid Credentials." });
            }
        }


        //[HttpPost]
        //public JsonResult UserSignin([FromBody] LoginModel loginModel)
        //{
        //    if (string.IsNullOrEmpty(loginModel.UserId) || string.IsNullOrEmpty(loginModel.Password))
        //    {
        //        return Json(new { success = false, message = "User details are empty." });
        //    }

        //    string query = "SELECT * FROM UserMaster WHERE User_ID = '" + loginModel.UserId + "'";
        //    DataTable dt = _dbExecuter.GetDataTable(query);

        //    if (dt.Rows.Count > 0)
        //    {
        //        return Json(new { success = false, message = "This user already exists." });
        //    }
        //    else
        //    {
        //        string insertQuery = "INSERT INTO UserMaster (User_ID, User_Name, Password, Status) VALUES ('"+ loginModel.UserId + "', '"+ loginModel.UserName + "', '"+ loginModel.Password + "', 'Y')";
        //        bool isInserted = _dbExecuter.ExecuteNonQuery(insertQuery);

        //        if (isInserted)
        //        {
        //            return Json(new { success = true, message = "User created successfully." });
        //        }
        //        else
        //        {
        //            return Json(new { success = false, message = "Failed to create user." });
        //        }
        //    }
        //}


        [HttpPost]
        public JsonResult UserSignin([FromBody] LoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.UserId) || string.IsNullOrEmpty(loginModel.Password))
            {
                return Json(new { success = false, message = "User details are empty." });
            }

            // Check if user already exists
            string checkQuery = "SELECT COUNT(*) FROM UserMaster WHERE User_ID = @UserId";
            object result = _dbExecuter.ExecuteScalar(checkQuery, new SqlParameter("@UserId", loginModel.UserId));

            if (Convert.ToInt32(result) > 0)
            {
                return Json(new { success = false, message = "This user already exists." });
            }

            // Insert new user
            string insertQuery = "INSERT INTO UserMaster (User_ID, User_Name, Password, Status) VALUES (@UserId, @UserName, @Password, 'Y')";
            bool isInserted = _dbExecuter.ExecuteNonQuery(insertQuery,
                new SqlParameter("@UserId", loginModel.UserId),
                new SqlParameter("@UserName", loginModel.UserName),
                new SqlParameter("@Password", loginModel.Password));

            if (isInserted)
            {
                return Json(new { success = true, message = "User created successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to create user." });
            }
        }




        //[HttpPost]
        //public JsonResult UserPasswordUpdate([FromBody] PasswordUpdateModel passwordUpdateModel)
        //{
        //    if (passwordUpdateModel.NewPassword != passwordUpdateModel.ConfirmPassword)
        //    {
        //        return Json(new { success = false, message = "Password and confirm password do not match." });
        //    }

        //    string query = "SELECT * FROM UserMaster WHERE User_ID = '" + passwordUpdateModel.UserId + "'";
        //    DataTable dt = _dbExecuter.GetDataTable(query);

        //    if (dt.Rows.Count > 0)
        //    {
        //        if (dt.Rows[0]["Status"].ToString() != "Y")
        //        {
        //            return Json(new { success = false, message = "User state is inactive. Please contact admin." });
        //        }

        //        string updateQuery = "UPDATE UserMaster SET Password = '"+ passwordUpdateModel.NewPassword + "' WHERE User_ID = '"+ passwordUpdateModel.NewPassword + "'";
        //        bool isUpdated = _dbExecuter.ExecuteNonQuery(updateQuery);

        //        if (isUpdated)
        //        {
        //            return Json(new { success = true, message = "Password updated successfully." });
        //        }
        //        else
        //        {
        //            return Json(new { success = false, message = "Password update failed." });
        //        }
        //    }
        //    else
        //    {
        //        return Json(new { success = false, message = "Please enter a valid user." });
        //    }
        //}

        [HttpPost]
        public JsonResult UserPasswordUpdate([FromBody] PasswordUpdateModel passwordUpdateModel)
        {
            if (passwordUpdateModel.NewPassword != passwordUpdateModel.ConfirmPassword)
            {
                return Json(new { success = false, message = "Password and confirm password do not match." });
            }

            string query = "SELECT Status FROM UserMaster WHERE User_ID = @UserId";
            DataTable dt = _dbExecuter.GetDataTable(query, new SqlParameter("@UserId", passwordUpdateModel.UserId));

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Status"].ToString() != "Y")
                {
                    return Json(new { success = false, message = "User state is inactive. Please contact admin." });
                }

                string updateQuery = "UPDATE UserMaster SET Password = @NewPassword WHERE User_ID = @UserId";
                bool isUpdated = _dbExecuter.ExecuteNonQuery(updateQuery,
                    new SqlParameter("@NewPassword", passwordUpdateModel.NewPassword),
                    new SqlParameter("@UserId", passwordUpdateModel.UserId));

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
