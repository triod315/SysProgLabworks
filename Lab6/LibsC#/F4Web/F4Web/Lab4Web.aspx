<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Lab4Web.aspx.cs" Inherits="F4Web.Lab4Web" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="TextBox1" runat="server" style="display:block;margin-left:auto;margin-right:auto" Height="23px" Width="149px"></asp:TextBox>
           
            <asp:TextBox ID="TextBox2" runat="server" style="display:block;margin-left:auto;margin-right:auto" Height="25px" Width="150px"></asp:TextBox>

            <asp:Button ID="Button1" runat="server" Text="Do It" style="display:block;margin-left:auto;margin-right:auto" OnClick="Button1_Click"/>
            <hr />
            <asp:Label ID="Label1" runat="server" Text="" style="display:block;margin-left:auto;margin-right:auto; text-align:center"></asp:Label>
        </div>
    </form>
</body>
</html>
