using System;

namespace ProductsWebApp
{
    public partial class Products : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void DdlProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            // handling image change based on selected product
            int selectedProductId = int.Parse(ddlProducts.SelectedValue);

            switch (selectedProductId)
            {
                case 1:
                    imgProduct.ImageUrl = "~/Images/shoes.jpeg"; // Replace with actual image path
                    break;
                case 2:
                    imgProduct.ImageUrl = "~/Images/laptop.jpg"; // Replace with actual image path
                    break;
                case 3:
                    imgProduct.ImageUrl = "~/Images/headphones.jpg"; // Replace with actual image path
                    break;
                case 4:
                    imgProduct.ImageUrl = "~/Images/mobilePhone.jpg"; // Replace with actual image path
                    break;
                default:
                    imgProduct.ImageUrl = ""; // No image if no product selected
                    break;
            }
        }

        protected void BtnGetPrice_Click(object sender, EventArgs e)
        {
            // Display price based on selected product
            int selectedProductId = int.Parse(ddlProducts.SelectedValue);

            switch (selectedProductId)
            {
                case 1:
                    lblPrice.Text = "100";
                    break;
                case 2:
                    lblPrice.Text = "150";
                    break;
                case 3:
                    lblPrice.Text = "215";
                    break;
                case 4:
                    lblPrice.Text = "255";
                    break;
                default:
                    lblPrice.Text = "Please select a valid product.";
                    break;
            }
        }
    }
}