using MiniMart.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;

namespace MiniMart.Controllers
{
    public class ProductController : Controller
    {
        // PRODUCT LIST + SEARCH

        public ActionResult Index(string search, string category)
        {
            string cs = ConfigurationManager.ConnectionStrings["MiniMartDBConnection"].ConnectionString;

            SqlConnection con = new SqlConnection(cs);

            string query = "SELECT * FROM Products WHERE 1=1";

            if (!string.IsNullOrEmpty(search))
            {
                query += " AND ProductName LIKE @search";
            }

            if (!string.IsNullOrEmpty(category))
            {
                query += " AND Category=@category";
            }

            SqlCommand cmd = new SqlCommand(query, con);

            if (!string.IsNullOrEmpty(search))
            {
                cmd.Parameters.AddWithValue("@search", "%" + search + "%");
            }

            if (!string.IsNullOrEmpty(category))
            {
                cmd.Parameters.AddWithValue("@category", category);
            }

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            List<Product> products = new List<Product>();

            while (reader.Read())
            {
                products.Add(new Product
                {
                    ProductId = Convert.ToInt32(reader["ProductId"]),
                    ProductName = reader["ProductName"].ToString(),
                    Category = reader["Category"].ToString(),
                    Price = Convert.ToDecimal(reader["Price"]),
                    Description = reader["Description"].ToString(),
                    ImagePath = reader["ImagePath"].ToString()
                });
            }

            con.Close();

            ViewBag.ProductCount = products.Count;

            return View(products);
        }
        public ActionResult Create()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            string fileName = Path.GetFileName(product.ImageFile.FileName);

            string path = Server.MapPath("~/Images/" + fileName);

            product.ImageFile.SaveAs(path);

            string cs = ConfigurationManager.ConnectionStrings["MiniMartDBConnection"].ConnectionString;

            SqlConnection con = new SqlConnection(cs);

            string query = "INSERT INTO Products VALUES(@ProductName,@Category,@Price,@Description,@ImagePath)";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
            cmd.Parameters.AddWithValue("@Category", product.Category);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@Description", product.Description);
            cmd.Parameters.AddWithValue("@ImagePath", "/Images/" + fileName);

            con.Open();

            cmd.ExecuteNonQuery();

            con.Close();

            return RedirectToAction("Index");
        }

        // PRODUCT DETAILS

        public ActionResult Details(int id)
        {
            string cs = ConfigurationManager.ConnectionStrings["MiniMartDBConnection"].ConnectionString;

            SqlConnection con = new SqlConnection(cs);

            string query = "SELECT * FROM Products WHERE ProductId=@ProductId";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@ProductId", id);

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            Product product = new Product();

            if (reader.Read())
            {
                product.ProductId = Convert.ToInt32(reader["ProductId"]);
                product.ProductName = reader["ProductName"].ToString();
                product.Category = reader["Category"].ToString();
                product.Price = Convert.ToDecimal(reader["Price"]);
                product.Description = reader["Description"].ToString();
                product.ImagePath = reader["ImagePath"].ToString();
            }

            con.Close();

            return View(product);
        }

        // EDIT PRODUCT

        public ActionResult Edit(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            string cs = ConfigurationManager.ConnectionStrings["MiniMartDBConnection"].ConnectionString;

            SqlConnection con = new SqlConnection(cs);

            string query = "SELECT * FROM Products WHERE ProductId=@ProductId";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@ProductId", id);

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            Product product = new Product();

            if (reader.Read())
            {
                product.ProductId = Convert.ToInt32(reader["ProductId"]);
                product.ProductName = reader["ProductName"].ToString();
                product.Category = reader["Category"].ToString();
                product.Price = Convert.ToDecimal(reader["Price"]);
                product.Description = reader["Description"].ToString();
                product.ImagePath = reader["ImagePath"].ToString();
            }

            con.Close();

            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            string cs = ConfigurationManager.ConnectionStrings["MiniMartDBConnection"].ConnectionString;

            SqlConnection con = new SqlConnection(cs);

            string query = "UPDATE Products SET ProductName=@ProductName, Category=@Category, Price=@Price, Description=@Description WHERE ProductId=@ProductId";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
            cmd.Parameters.AddWithValue("@Category", product.Category);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@Description", product.Description);
            cmd.Parameters.AddWithValue("@ProductId", product.ProductId);

            con.Open();

            cmd.ExecuteNonQuery();

            con.Close();

            return RedirectToAction("Index");
        }

        // DELETE PRODUCT

        public ActionResult Delete(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            string cs = ConfigurationManager.ConnectionStrings["MiniMartDBConnection"].ConnectionString;

            SqlConnection con = new SqlConnection(cs);

            string query = "DELETE FROM Products WHERE ProductId=@ProductId";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@ProductId", id);

            con.Open();

            cmd.ExecuteNonQuery();

            con.Close();

            return RedirectToAction("Index");
        }
    }
}