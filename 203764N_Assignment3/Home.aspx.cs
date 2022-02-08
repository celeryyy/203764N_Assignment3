using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace _203764N_Assignment3
{
    public partial class Home : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }

                else
                {
                    lbl_msg.Text = "Congratulations! You are logged in!";
                    lbl_msg.ForeColor = System.Drawing.Color.Green;
                    btnLogout.Visible = true;
                }
            }

            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("X-Frame-Options", "DENY");
        }

        //protected string getEmail()
        //{
        //    string x = "";
        //    SqlConnection connection = new SqlConnection(MYDBConnectionString);
        //    string sql = "select Email FROM Account WHERE Email = @Email";
        //    SqlCommand command = new SqlCommand(sql, connection);
        //    command.Parameters.AddWithValue("@Email", tb_emailadd.Text);

        //    try
        //    {
        //        connection.Open();
        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                if (reader["AccountLockout"] != null)
        //                {
        //                    y = Convert.ToInt32(reader["AccountLockout"]);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.ToString());
        //    }
        //    return x;
        //}

        protected void LogoutAction()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmdd = new SqlCommand("UPDATE AuditLog SET Action=@Action WHERE Emailadd = @Emailadd"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {

                            cmdd.CommandType = CommandType.Text;
                            cmdd.Parameters.AddWithValue("@Emailadd", Session["LoggedIn"]);
                            cmdd.Parameters.AddWithValue("@Action", "Logout");
                            cmdd.Parameters.AddWithValue("@DateTimeAction", DateTime.Now);
                            cmdd.Connection = conn;
                            conn.Open();
                            cmdd.ExecuteNonQuery();
                            conn.Close();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        protected void LogoutMe(object sender, EventArgs e)
        {
            LogoutAction();
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("Login.aspx", false);
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = String.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }
    }
}