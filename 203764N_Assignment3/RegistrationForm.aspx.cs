using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text.RegularExpressions;
using System.Drawing;

using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace _203764N_Assignment3
{
    public partial class RegistrationForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        Boolean Val = false;

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            // Fname
            lbl_fname.ForeColor = Color.Red;
            lbl_fname.Text = checkName(tb_fname.Text);

            // Lname
            lbl_lname.ForeColor = Color.Red;
            lbl_lname.Text = Checklname(tb_lname.Text);

            // Dob
            lbl_dob.ForeColor = Color.Red;
            lbl_dob.Text = CheckDOB(tb_DOB.Text);

            //// Email
            lbl_email.ForeColor = Color.Red;
            lbl_email.Text = Checkemail(tb_emailadd.Text);

            //// Credit Card
            lbl_creditcard.ForeColor = Color.Red;
            lbl_creditcard.Text = Checkcredit(tb_creditcard.Text);

            //// Photo

            //// Password
            //if (tb_fname.Text == "")
            ////{
            ////    lbl_fname.ForeColor = Color.Red;
            //    lbl_fname.Text = "Field is required!";
            //}
            if (tb_password.Text == "")
            {
                lbl_pwdchecker.ForeColor = Color.Red;
                lbl_pwdchecker.Text = "Field is required!";
            }
            else
            {
                lbl_pwdchecker.Text = " ";
            }
            int scores = checkPassword(tb_password.Text);
            string status = "";
            switch (scores)
            {
                case 1:
                    status = "Very Weak";
                    break;
                case 2:
                    status = "Weak";
                    break;
                case 3:
                    status = "Medium";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Excellent";
                    break;
                default:
                    break;
            }
            lbl_pwdchecker.Text = "Status : " + status;
            if (scores < 5)
            {
                lbl_pwdchecker.ForeColor = Color.Red;
                return;
            }
            lbl_pwdchecker.ForeColor = Color.Green;
            string pwd = tb_password.Text.ToString().Trim();
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];

            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);

            SHA512Managed hashing = new SHA512Managed();

            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

            finalHash = Convert.ToBase64String(hashWithSalt);

            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;

            if (lbl_fname.Text != "Field is required!" && lbl_lname.Text != "Field is required!" && lbl_email.Text != "Field is required!" && lbl_creditcard.Text != "Field is required!" && lbl_dob.Text != "Field is required!" && lbl_pwdchecker.Text != "Field is required!" )
            {
                if (lbl_email.Text != "This email address is already in use.")
                {

                    createAccount();

                }
                
            }
            
            







        }

        protected void createAccount()
        {
            try
            {
                RegisterAction();
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@FirstName,@LastName,@CreditCardInfo,@Email,@PasswordHash,@PasswordSalt,@DOB,@DateTimeRegistered,@IV,@Key,@Count,@AccountLockout,@DateTimeLockout)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FirstName", tb_fname.Text.Trim());
                            cmd.Parameters.AddWithValue("@LastName", tb_lname.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreditCardInfo", Convert.ToBase64String(encryptData(tb_creditcard.Text.Trim())));
                            cmd.Parameters.AddWithValue("@Email", tb_emailadd.Text.Trim());
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@DOB", tb_DOB.Text.Trim());
                            cmd.Parameters.AddWithValue("@DateTimeRegistered", DateTime.Now);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@Count", 0);
                            cmd.Parameters.AddWithValue("@AccountLockout", 0); // 0 is flse
                            cmd.Parameters.AddWithValue("@DateTimeLockout", 0);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();


                            Response.Redirect("Login.aspx", false);


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        protected void RegisterAction()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmdd = new SqlCommand("INSERT INTO AuditLog VALUES(@Emailadd,@Action,@DateTimeAction)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {

                            cmdd.CommandType = CommandType.Text;
                            cmdd.Parameters.AddWithValue("@Emailadd", tb_emailadd.Text.Trim());
                            cmdd.Parameters.AddWithValue("@Action", "Registered");
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
        //protected void btn_checkPassword_Click(object sender, EventArgs e)
        //{
        //    int scores = checkPassword(tb_password.Text);
        //    string status = "";
        //    switch (scores)
        //    {
        //        case 1:
        //            status = "Very Weak";
        //            break;
        //        case 2:
        //            status = "Weak";
        //            break;
        //        case 3:
        //            status = "Medium";
        //            break;
        //        case 4:
        //            status = "Strong";
        //            break;
        //        case 5:
        //            status = "Excellent";
        //            break;
        //        default:
        //            break;
        //    }
        //    lbl_pwdchecker.Text = "Status : " + status;
        //    if (scores < 4)
        //    {
        //        lbl_pwdchecker.ForeColor = Color.Red;
        //        return;
        //    }
        //    lbl_pwdchecker.ForeColor = Color.Green;

        //}

        private string checkName(string Fname)
        {
            if (String.IsNullOrEmpty(Fname))
            {
                return "Field is required!";
            }
            if (Regex.IsMatch(Fname, "[0-9]"))
            {
                return "Field must not contain Numbers";
            }
            if (Regex.IsMatch(Fname, "[^a-zA-Z0-9]"))
            {
                return "Field must not contain special characters";
            }
            return " ";

        }

        private string Checklname(string lname)
        {
            if (lname == "")
            {
                return "Field is required!";
            }
            if (Regex.IsMatch(lname, "[0-9]"))
            {
                return "Field must not contain Numbers";
            }
            if (Regex.IsMatch(lname, "[^a-zA-Z0-9]"))
            {
                return "Field must not contain special characters";
            }

            return " ";
        }
        private string getEmail()
        {

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select Email FROM Account WHERE Email = @Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", tb_emailadd.Text);

            string comment = "";
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Email"] != null)
                        {

                            if (reader["Email"].ToString() == tb_emailadd.Text)
                            {
                                comment = "This email address is already in use.";
                            }
                            else
                            {
                                comment = " ";
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

            return comment;


        }

        private string Checkemail(string email)
        {
            if(email == "")
            {
                return "Field is required!";
            }
            
            else
            {
                
                return getEmail();
            }
           
            
        }

        private string Checkcredit(string creditc)
        {
            if(creditc == "")
            {
                return "Field is required!";
            }
            if (Regex.IsMatch(creditc, "[A-Z]"))
            {
                return "Field should not contain letters";
            }
            if (Regex.IsMatch(creditc, "[a-z]"))
            {
                return "Field should not contain letters";
            }
            if (Regex.IsMatch(creditc, "[^a-zA-Z0-9]"))
            {
                return "Field should not contin special characters";
            }
            return " ";
        }

        private string CheckDOB(string dob)
        {
            if(dob =="")
            {
                return "Field is required!";
            }
            if(Regex.IsMatch(dob, "[^a-zA-Z0-9]"))
            {
                return "Field should not contin special characters";
            }
            if (Regex.IsMatch(dob, "[A-Z]"))
            {
                return "Field should not contain letters";
            }
            if (Regex.IsMatch(dob, "[a-z]"))
            {
                return "Field should not contain letters";
            }
            return " ";
        }
        private int checkPassword(string password)
        {
            int score = 0;

            // score 1 very weak (min password length of 12)
            if (password.Length < 12 && password.Length > 0)
            {
                return 1;
            }
            else
            {
                score = 1;
            }

            // score 2 is weak (contains lowercase letters)
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }

            // score 3 is medium (contains uppercase letters)
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }

            // score 4 is strong (contains numerals)
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }

            // scpre  is execellent (contains special characters)
            if (Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                score++;
            }
            return score;
        }

        






        protected byte[] encryptData(string data)
        {
            byte[] ciphertext = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                ciphertext = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return ciphertext;
        }
    }
}