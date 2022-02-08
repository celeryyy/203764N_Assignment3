<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="_203764N_Assignment3.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://www.google.com/recaptcha/api.js?render=6Lc9alkeAAAAAM9sFtk7a-P1j3TyjHiqhU7hQYos"></script>
</head>
<body>
    <h2>Login Form</h2>

    <form id="form1" runat="server">
        <div>
            <table class="style1">
                 <tr>
                <td class="style3">
                    <asp:Label ID="lb_loginemailadd" runat="server" Text="Email Address"></asp:Label>
                </td>
                <td class="style2">
                        <asp:TextBox ID="tb_emailadd" runat="server" Width="240px" Height="30px" placeholder="Enter your email address"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="This field is required" ControlToValidate="tb_emailadd" ForeColor="#cc0000"></asp:RequiredFieldValidator>
                        
                </td>
            </tr>
                 <tr>
                <td class="style3">
                    <asp:Label ID="lb_loginpassword" runat="server" Text="Password"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_password" runat="server" Width="240px" Height="30px" placeholder="Enter your password" TextMode="Password"></asp:TextBox>
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="This field is required" ControlToValidate="tb_password" ForeColor="#cc0000"></asp:RequiredFieldValidator>
                    
                </td>
            </tr><tr>
                <td class="style3">
                   
                </td>
                <td class="style2">
                     <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
                </td>
            </tr>
               
            </table>
        </div>
        <asp:Button ID ="loginbtn" Text="Login" runat="server" OnClick="btn_submit_click"/>
        <br />
        <br />
        <asp:Label ID="lblMessage" runat="server" Text="Error message here (lblMessage)"></asp:Label>
        <br />

       
        <asp:Label ID="lbl_gScore" runat="server" Text="Json Message: " EnableViewState="False"></asp:Label>
        <br />
        <asp:Label ID="lbl_test" runat="server"></asp:Label>
       
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6Lc9alkeAAAAAM9sFtk7a-P1j3TyjHiqhU7hQYos', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
    
    
</body>
</html>
