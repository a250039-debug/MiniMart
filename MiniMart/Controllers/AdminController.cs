using MiniMart.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace MiniMart.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            string cs = ConfigurationManager.ConnectionStrings["MiniMartDBConnection"].ConnectionString;

            SqlConnection con = new SqlConnection(cs);

            string query = "SELECT * FROM Admins WHERE Email=@Email AND Password=@Password";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@Email", admin.Email);
            cmd.Parameters.AddWithValue("@Password", admin.Password);

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                Session["Admin"] = admin.Email;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Invalid Login";
            }

            con.Close();

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Login");
        }

    }
}