<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistrationForm.aspx.cs" Inherits="_203764N_Assignment3.RegistrationForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Registration</title>

    <script type="text/javascript">
        function validatepwd() {
            var str = document.getElementById('<%=tb_password.ClientID %>').value;
            if (str.length == 0) {
                document.getElementById("lbl_pwdchecker").innerHTML = "This field is required";
                document.getElementById("lbl_pwdchecker").style.color = "Red";

                return ("empty");
            }
            else if (str.length < 12 && str.length > 0) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password Length Must Be At Least 12 Characters";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("too_short");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 number";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_number");
            }
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 uppercase";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_uppercase");
            }
            if (str.search(/[a-z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 lowercase";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_lowercase");
            }
            else if (str.search(/[^a-zA-Z0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 special characters";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_specialcharacters");
            }
            else {
                document.getElementById("lbl_pwdchecker").innerHTML = " ";
            }
            
        }
        function validatefname() {
            var firstn = document.getElementById('<%=tb_fname.ClientID %>').value;
            if (firstn.length == 0) {
                document.getElementById('lbl_fname').innerHTML = "This field is required!";
                document.getElementById('lbl_fname').style.color = "Red";
                return ("empty");
            }
            if (str.search(/[0-9]/) == 1) {
                document.getElementById("lbl_fname").innerHTML = "This field does not require numbers!";
                document.getElementById("lbl_fname").style.color = "Red";
                return ("no_number");
            if (str.search(/[^a-zA-Z0-9]/) == 1) {
                document.getElementById("lbl_fname").innerHTML = "This field does not require special characters";
                document.getElementById("lbl_fname").style.color = "Red";
                return ("no_specialcharacters");
            
        }

        function validatelname() {
            var lname = document.getElementById('<%=tb_lname.ClientID %>').value;
            if (lname.length == 0) {
                document.getElementById('lbl_lname').innerHTML = "This field is required!";
                document.getElementById('lbl_lname').style.color = "Red";
                return ("empty");
            }
            if (str.search(/[0-9]/) == 1) {
                document.getElementById("lbl_lname").innerHTML = "This field does not require numbers!";
                document.getElementById("lbl_lname").style.color = "Red";
                return ("no_number");
            if (str.search(/[^a-zA-Z0-9]/) == 1) {
                document.getElementById("lbl_lname").innerHTML = "This field does not require special characters";
                document.getElementById("lbl_lname").style.color = "Red";
                return ("no_specialcharacters");
            }
            
        }

        function validatecredit() {
            var credit = document.getElementById('<%=tb_creditcard.ClientID %>').value;
            if (credit.length == 0) {
                document.getElementById('lbl_creditcard').innerHTML = "This field is required!";
                document.getElementById('lbl_creditcard').style.color = "Red";
                return ("empty");
            }
            else if (credit.length > 16) {
                document.getElementById('lbl_creditcard').innerHTML = "Please enter in correct format!";
                document.getElementById('lbl_creditcard').style.color = "Red";
                return ("empty");

            }
            if (dob.search(/[^a-zA-Z0-9]/) == 1) {
                document.getElementById("lbl_creditcard").innerHTML = "This field does not require special characters";
                document.getElementById("lbl_creditcard").style.color = "Red";
                return ("no_specialcharacters");

            }
            if (str.search(/[A-Z]/) == 1) {
                document.getElementById("lbl_creditcard").innerHTML = "This field does not require uppercase";
                document.getElementById("lbl_creditcard").style.color = "Red";
                return ("no_uppercase");
            }
            if (str.search(/[a-z]/) == 1) {
                document.getElementById("lbl_creditcard").innerHTML = "This field does not require lowercase";
                document.getElementById("lbl_creditcard").style.color = "Red";
                return ("no_lowercase");
            }
            
            
        }

        function validateDOB() {
            var dob = document.getElementById('<%=tb_DOB.ClientID %>').value;
            if (dob.length == 0) {
                document.getElementById('lbl_dob').innerHTML = "This field is required!";
                document.getElementById('lbl_dob').style.color = "Red";
                return ("empty");
            }
            else if (dob.length > 6) {
                document.getElementById('lbl_dob').innerHTML = "Please enter in the correct format: DDMMYY";
                document.getElementById('lbl_dob').style.color = "Red";
            }
            if (dob.search(/[^a-zA-Z0-9]/) == 1) {
                document.getElementById("lbl_dob").innerHTML = "This field does not require special characters";
                document.getElementById("lbl_dob").style.color = "Red";
                return ("no_specialcharacters");

            }
            if (str.search(/[A-Z]/) == 1) {
                document.getElementById("lbl_dob").innerHTML = "This field does not require uppercase";
                document.getElementById("lbl_dob").style.color = "Red";
                return ("no_uppercase");
            }
            if (str.search(/[a-z]/) == 1) {
                document.getElementById("lbl_dob").innerHTML = "This field does not require lowercase";
                document.getElementById("lbl_dob").style.color = "Red";
                return ("no_lowercase");
            }
        }

        function validateemail() {
            var emaila = document.getElementById('<%=tb_emailadd.ClientID %>').value;
            if (emaila.length == 0) {
                document.getElementById('lbl_email').innerHTML = "This field is required!";
                document.getElementById('lbl_email').style.color = "Red";
                return ("empty");
            }
            else if (emaila.search(/[a-zA-Z0-9]+@[a-zA-Z]/) == -1) {
                document.getElementById('lbl_email').innerHTML = "Please reenter your email!";
                document.getElementById('lbl_email').style.color = "Red";
                return ("empty");
            }
            else {
                document.getElementById('lbl_email').innerHTML = "";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Registration Form</h2>
        <div>
           <table class="style1">
             
            <tr>
                <td class="style3">
                    <asp:Label ID="lb_fname" runat="server" Text="First Name"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_fname" runat="server" Width="240px" Height="30px" placeholder="Enter your first name"></asp:TextBox>
                    <asp:Label ID="lbl_fname" runat="server"></asp:Label>
<%--                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="This field is required" ControlToValidate="tb_fname" ForeColor="#cc0000"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
               <tr>
                <td class="style3">
                    <asp:Label ID="lb_lname" runat="server" Text="Last Name"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_lname" runat="server" Width="240px" Height="30px" placeholder="Enter your last name"></asp:TextBox>
   <%--                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="This field is required" ControlToValidate="tb_lname" ForeColor="#cc0000"></asp:RequiredFieldValidator>--%>
                 <asp:Label ID="lbl_lname" runat="server"></asp:Label>
                </td>
            </tr>
               <tr>
                <td class="style3">
                    <asp:Label ID="lb_creditcardinfo" runat="server" Text="Credit Card Information"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_creditcard" runat="server" Width="240px" Height="30px" placeholder="Enter your credit card information" TextMode="Number"></asp:TextBox>
                    <asp:Label ID="lbl_creditcard" runat="server"></asp:Label>
<%--                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="This field is required" ControlToValidate="tb_creditcard" ForeColor="#cc0000"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
               <tr>
                <td class="style3">
                    <asp:Label ID="lb_emailadd" runat="server" Text="Email Address"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_emailadd" runat="server" Width="240px" Height="30px" placeholder="Enter your email address"></asp:TextBox>
<%--                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="This field is required" ControlToValidate="tb_emailadd" ForeColor="#cc0000"></asp:RequiredFieldValidator>--%>
                    <asp:Label ID="lbl_email" runat="server"></asp:Label>
                </td>
            </tr>
               <tr>
                <td class="style3">
                    <asp:Label ID="lb_password" runat="server" Text="Password"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_password" runat="server" Width="240px" Height="30px" placeholder="Enter your password"></asp:TextBox>
                    <asp:Label ID="lbl_pwdchecker" runat="server"></asp:Label>
                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="This field is required" ControlToValidate="tb_password" ForeColor="#cc0000"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
               <tr>
                <td class="style3">
                    <asp:Label ID="lb_DOB" runat="server" Text="Date Of Birth (DDMMYYYY)"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_DOB" runat="server" Width="240px" Height="30px" placeholder="Enter your date of birth"></asp:TextBox>
<%--                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="This field is required" ControlToValidate="tb_DOB" ForeColor="#cc0000"></asp:RequiredFieldValidator>--%>
                    <asp:Label ID="lbl_dob" runat="server"></asp:Label>
                </td>
            </tr>
               <tr>
                <td class="style3">
                    <asp:Label ID="lb_img" runat="server" Text="Photo"></asp:Label>
                </td>
                <td class="style2">
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                </td>
            </tr>
            </table>
        </div>
       <asp:Button ID ="Submit" Text="Submit" runat="server" OnClick="btn_Submit_Click"/>
        

    </form>
</body>
</html>
