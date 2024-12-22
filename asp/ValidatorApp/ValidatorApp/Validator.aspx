<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Validator.aspx.cs" Inherits="ValidatorApp.Validator" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="name" runat="server">Name: </asp:Label>
            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator 
                ID="rfvName" 
                runat="server" 
                ControlToValidate="txtName" 
                ErrorMessage="Please enter your name" 
                ForeColor="Red">
            </asp:RequiredFieldValidator>
        </div>

        <div>
            <asp:Label ID="familyName" runat="server">Family Name: </asp:Label>
            <asp:TextBox ID="txtFamilyName" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator 
                ID="rfvFamilyName" 
                runat="server" 
                ControlToValidate="txtFamilyName" 
                ErrorMessage="Please enter your family name" 
                ForeColor="Red">
            </asp:RequiredFieldValidator>
        </div>

        <div>
            <asp:Label ID="address" runat="server">Address: </asp:Label>
            <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator 
                ID="rfvAddress"
                runat="server"
                ControlToValidate="txtAddress"
                ErrorMessage="Please enter your address"
                ForeColor="Red"
            />
        </div>

        <div>
            <asp:Label ID="city" runat="server">City: </asp:Label>
            <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator 
                ID="rfvCity"
                runat="server"
                ControlToValidate="txtCity"
                ErrorMessage="Please enter your city"
                ForeColor="Red"
            />
        </div>

        <div>
            <asp:Label ID="zipcode" runat="server">ZipCode: </asp:Label>
            <asp:TextBox ID="txtZipCode" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator 
                ID="rfvZipCode"
                runat="server"
                ControlToValidate="txtZipCode"
                ErrorMessage="Please enter your city"
                ForeColor="Red"
            />
        </div>

        <div>
            <asp:Label ID="phoneNumber" runat="server">Phone Number:</asp:Label>
            <asp:TextBox ID="txtPhoneNumber" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator 
                ID="rfvPhoneNumber"
                runat="server"
                ControlToValidate="txtPhoneNumber"
                ErrorMessage="Please enter your Phone Number"
                ForeColor="Red"
            />
            <asp:RegularExpressionValidator ID="revPhoneNumber" runat="server" 
                ControlToValidate="txtPhoneNumber" 
                ErrorMessage="Invalid phone number format" 
                ValidationExpression="^(?:\d{2}|\d{3})-\d{7}$">
            </asp:RegularExpressionValidator>
        </div>

        <div>
            <asp:Label ID="email" runat="server">Email: </asp:Label>
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator 
                ID="rfvEmail"
                runat="server"
                ControlToValidate="txtEmail"
                ErrorMessage="Please enter your email"
                ForeColor="Red"
            />
            <asp:RegularExpressionValidator 
                ID="revEmail" 
                runat="server" 
                ControlToValidate="txtEmail" 
                ErrorMessage="Invalid email format" 
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                >
            </asp:RegularExpressionValidator>
        </div>

        <asp:Button ID="btnSubmit" runat="server" Text="Submit" onClick="Page_Load"/>

        <asp:ValidationSummary ID="validationSummary" runat="server" HeaderText="Please correct the following error:" />
    </form>
</body>
</html>
