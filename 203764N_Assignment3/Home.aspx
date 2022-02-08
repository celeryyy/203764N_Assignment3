<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="_203764N_Assignment3.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
             <asp:Label ID="lbl_msg" runat="server"></asp:Label>
            <br />
            <br />
           <asp:Button ID="btnLogout" runat="server" onClick="LogoutMe" Text="Logout" />
        </div>
    </form>
</body>
</html>

