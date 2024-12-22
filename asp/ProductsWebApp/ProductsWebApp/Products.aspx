<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="ProductsWebApp.Products" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Selector</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Select a Product</h2>

            <asp:DropDownList ID="ddlProducts" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlProducts_SelectedIndexChanged">
                <asp:ListItem Text="Select a product" Value="0" />
                <asp:ListItem Text="shoes" Value="1" />
                <asp:ListItem Text="laptop" Value="2" />
                <asp:ListItem Text="headphones" Value="3" />
                <asp:ListItem Text="mobilePhone" Value="4" />
            </asp:DropDownList>
            
            <br /><br />
            <asp:Image ID="imgProduct" runat="server" Height="200px" Width="200px" />
            
            <br /><br />
            <asp:Button ID="btnGetPrice" runat="server" Text="Get Price" OnClick="BtnGetPrice_Click" />
            
            <br /><br />
            <asp:Label ID="lblPrice" runat="server" Text="Price will appear here" Font-Bold="true" Font-Size="Large" ForeColor="Green" />
        </div>
    </form>
</body>
</html>

