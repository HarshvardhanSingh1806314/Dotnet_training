using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ValidatorApp
{
    public partial class Validator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string familyName = txtFamilyName.Text;
            string address = txtAddress.Text;
            string city = txtCity.Text;
            string zipCode = txtZipCode.Text;
            string phoneNumber = txtPhoneNumber.Text;
            string email = txtEmail.Text;
            
            if(name != familyName && address.Length >= 2 && city.Length >= 2 && zipCode.Length == 5 && Page.IsValid)
            {
                Response.Write("Form Submitted Successfully");
            }
        }
    }
}