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

using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace _203764N_Assignment3
{
    public partial class Login : System.Web.UI.Page
    {
        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }

        public bool ValidateCaptcha()
        {
            bool result = true;

            string captchaResponse = Request.Form["g-recaptcha-response"];

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret= &response=" + captchaResponse);

            try
            {
                using (WebResponse wResponse = req.GetResponse()) {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();

                        lbl_gScore.Text = jsonResponse.ToString();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        result = Convert.ToBoolean(jsonObject.success);
                    }

                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("X-Frame-Options", "DENY");
        }

        protected void btn_submit_click(object sender, EventArgs e)
        {

            string pwd = tb_password.Text.ToString().Trim();
            string emailadd = tb_emailadd.Text.ToString().Trim();
            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(emailadd);
            string dbSalt = getDBSalt(emailadd);

            string j = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select * FROM Account WHERE Email = @Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", emailadd);
            if (ValidateCaptcha())
            {
                getDate();
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        try
                        {
                            if (getAccountLockout() == 0 || updateAccount() == 0)
                            {
                                
                                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                                    {
                                        string pwdWithSalt = pwd + dbSalt;
                                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                                        string userHash = Convert.ToBase64String(hashWithSalt);
                                        if (userHash.Equals(dbHash))
                                        {
                                            LoginAction();
                                            //lblMessage.Text = "YAY";
                                            Session["LoggedIn"] = tb_emailadd.Text.Trim();

                                            // create a new GUID and save into session
                                            string guid = Guid.NewGuid().ToString();
                                            Session["AuthToken"] = guid;

                                            // Create a new cookie with this guid value
                                            Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                                            Response.Redirect("Home.aspx", false);
                                        }
                                        else
                                        {
                                            CountIncrement();
                                            //lblMessage.Text = CountIncrement().ToString();
                                            //Response.Redirect("Login.aspx");
                                            // cll count increment - if count > 3 updt lockout to true else {}
                                            if (getCount() > 2)
                                            {
                                                lblMessage.Text = "You would not be able to login for 1 minute. Please try again later";
                                                setDateTimeLockout();
                                                updateAccount();
                                                

                                            }
                                            else
                                            {
                                                lblMessage.Text = "Email address or password is invalid. Please try again!";
                                            }

                                        }
                                    
                                }
                                
                            }
                            else
                            {
                                
                                lblMessage.Text = "Your Account is locked, please try again later!";
                            }
                        }



                        catch (Exception ex)
                        {
                            throw new Exception(ex.ToString());
                        }
                        finally { }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally { }
            }


        }
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        byte[] Key;
        byte[] IV;

        protected int getCount()
        {
            int x = 0;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select Count FROM Account WHERE Email = @Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", tb_emailadd.Text);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Count"] != null)
                        {
                            x = Convert.ToInt32(reader["Count"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally
            {
                connection.Close();
            }

            return x;


        }
        protected int updateCount()
        {
            SqlConnection con = new SqlConnection(MYDBConnectionString);
            string sql = "UPDATE Account SET Count=@Count WHERE Email = @Email";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Email", tb_emailadd.Text);

            if (getDate() == 0)
            {
                cmd.Parameters.AddWithValue("@Count", 0);

            }

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            return getCount();
        }

        private string Checkemail(string email)
        {
            if (email == "")
            {
                return "Field is required!";
            }

            else
            {

                return "";
            }


        }

        protected int getAccountLockout()
        {
            int y = 0;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select AccountLockout FROM Account WHERE Email = @Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", tb_emailadd.Text);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["AccountLockout"] != null)
                        {
                            y = Convert.ToInt32(reader["AccountLockout"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally
            {
                connection.Close();
            }

            return y;


        }
        
        
        protected void LoginAction()
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
                            cmdd.Parameters.AddWithValue("@Emailadd", tb_emailadd.Text.Trim());
                            cmdd.Parameters.AddWithValue("@Action", "Login");
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
        // set new time
        protected DateTime setDateTimeLockout()
        {
            DateTime x = DateTime.Now.AddMinutes(1);
            try
            {
                using (SqlConnection connect = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand comd = new SqlCommand("UPDATE Account SET DateTimeLockout=@DateTimeLockout where Email=@Email"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            comd.CommandType = CommandType.Text;
                            comd.Parameters.AddWithValue("@DateTimeLockout", DateTime.Now.AddMinutes(1));
                            comd.Parameters.AddWithValue("@Email", tb_emailadd.Text);

                            comd.Connection = connect;
                            connect.Open();
                            comd.ExecuteNonQuery();
                            connect.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }


            return x;
        }
        // check time
        protected int getDate()
        {
            SqlConnection conn = new SqlConnection(MYDBConnectionString);
            string sql = "SELECT DateTimeLockout FROM Account where Email = @Email";
            SqlCommand cmdd = new SqlCommand(sql, conn);
            cmdd.Parameters.AddWithValue("@Email", tb_emailadd.Text);

            int lockout = 0;
            try
            {
                conn.Open();
                using (SqlDataReader reader = cmdd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["DateTimeLockout"] != null)
                        {
                            // bring Account lockout to false
                            if (Convert.ToDateTime(reader["DateTimeLockout"]) <= DateTime.Now)
                            {
                                lockout = 0;
                            }
                            else
                            {
                                lockout = 1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return lockout;
        }

        protected int updateAccount()
        {
            //int x = getAccountLockout();
            SqlConnection con = new SqlConnection(MYDBConnectionString);
            string sql = "UPDATE Account SET AccountLockout=@AccountLockout WHERE Email = @Email";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Email", tb_emailadd.Text);

            if (getDate() == 0)
            {
                updateCount();
                cmd.Parameters.AddWithValue("@AccountLockout", 0);
            }
            else
            {
                //lbl_test.Text = "TESTTTT";
                cmd.Parameters.AddWithValue("@AccountLockout", getAccountLockout() + 1);
            }
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            return getAccountLockout();
        }
       
        protected int CountIncrement()
        {
            int x = getCount();

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE Account SET Count=@Count WHERE Email = @Email"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@Count", getCount() + 1);
                            cmd.Parameters.AddWithValue("@Email", tb_emailadd.Text);

                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return x;
        }

        
        
        protected string getDBHash(string emailadd) // count increment
        {
            string h = null;

            // int x = getCount();

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE Email = @Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", emailadd);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally
            {
                connection.Close();
            }

            return h;

        }

        protected string getDBSalt(string emailadd)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PASSWORDSALT FROM ACCOUNT WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", emailadd);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }

        //protected string getCount(string count)
        //{
        //    SqlConnection connection = new SqlConnection(MYDBConnectionString);
        //    string sql = "select COUNT FROM ACCOUNT WHERE Email=@Email";

        //    return " ";
        //}


    }

}
