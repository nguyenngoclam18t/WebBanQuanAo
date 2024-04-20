using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using BTL_LTWeb.Models;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using BTL_LTWeb.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace BTL_LTWeb.Controllers
{
    public class Admin_ProductApiController : ApiController
    {
        string constring = ConfigurationManager.ConnectionStrings["ConnectionStringDB"].ConnectionString;
        SqlConnection con;


        [HttpPost]
        public string addProduct(Admin_Product p)
        {
            p.Img = Path.GetFileName(p.Img);
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string imagesFolderPath = Path.Combine(desktopPath, "images");
            string fullPath = Path.Combine(imagesFolderPath, p.Img);

            string cloudinaryFileName = Path.GetFileNameWithoutExtension(p.Img);
            string cloudinaryFileExtension = Path.GetExtension(p.Img).ToLower();
            string cloudinaryPublicId = cloudinaryFileName;
            string imageSaveToDB = cloudinaryFileName + cloudinaryFileExtension;

            if ( p.Img == "" || p.Name == "" || p.Price == 0||p.Descript=="")
            {
                return "Error add product ";
            }
            try
            {
                var cloudinary = new Cloudinary(new CloudinaryDotNet.Account(
                    "dfapum2fd",
                    "821542417418447",
                    "RzD7B0cuTe5EQ06UuK4zQ4yB0kM"
                 ));

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fullPath),
                    PublicId = cloudinaryPublicId,
                    Overwrite = true
                };

                var uploadResult = cloudinary.Upload(uploadParams);
                string imageUrl = uploadResult.SecureUrl.ToString();

                con = new SqlConnection(constring);
                con.Open();
                string query = "INSERT INTO Product (product_name, description, price, image_url) VALUES (@ProductName, @ProductDescription, @ProductPrice, @ProductImage) ";
                SqlCommand sqlcmd = new SqlCommand(query, con);
                sqlcmd.Parameters.AddWithValue("@ProductName",p.Name);
                sqlcmd.Parameters.AddWithValue("@ProductDescription",p.Descript);
                sqlcmd.Parameters.AddWithValue("@ProductPrice",p.Price);
                sqlcmd.Parameters.AddWithValue("@ProductImage", imageSaveToDB);
                sqlcmd.ExecuteNonQuery();
                con.Close();
                return "Success add product ";
            }
            catch (Exception ex)
            {
                return "Error add product ";
            }
           
        }
        [HttpGet]
        public Admin_Product GetProductById(int id)
        {
            BaseService QL = new BaseService();
            Product P = QL.db.Products.FirstOrDefault(t => t.product_id == id);
            Admin_Product product = new Admin_Product();
            product.id = P.product_id;
            product.Name = P.product_name;
            product.Descript = P.description;
            product.Price =  (P.price??0) ;
            product.Img = P.image_url;
            
            return product;
        }
        [HttpPut]
        public string Update(Admin_Product p)
        {
            p.Img = Path.GetFileName(p.Img);
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string imagesFolderPath = Path.Combine(desktopPath, "images");
            string fullPath = Path.Combine(imagesFolderPath, p.Img);

            string cloudinaryFileName = Path.GetFileNameWithoutExtension(p.Img);
            string cloudinaryFileExtension = Path.GetExtension(p.Img).ToLower();

            string cloudinaryPublicId = cloudinaryFileName;

            string imageSaveToDB = cloudinaryFileName + cloudinaryFileExtension;


            p.Img = Path.GetFileName(p.Img);
            if (p.Img == "" || p.Name == "" || p.Price == 0 || p.Descript == "")
            {
                return "Error add product ";
            }
            try
            {
                var cloudinary = new Cloudinary(new CloudinaryDotNet.Account(
                    "dfapum2fd",
                    "821542417418447",
                    "RzD7B0cuTe5EQ06UuK4zQ4yB0kM"
                 ));

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fullPath),
                    PublicId = cloudinaryPublicId,
                    Overwrite = true
                };

                var uploadResult = cloudinary.Upload(uploadParams);
                string imageUrl = uploadResult.SecureUrl.ToString();

                BaseService QL = new BaseService();
                Product Product = QL.db.Products.FirstOrDefault(t => t.product_id == p.id);
                if(Product == null)
                {
                    return "Error Update product, never seen product before";
                }
                Product.product_name = p.Name;
                Product.description = p.Descript;
                Product.price = p.Price;
                Product.image_url = imageSaveToDB;
                QL.db.SaveChanges();
                return "Success Update product ";
            }
            catch (Exception)
            {
                return "Error Update product ";
            }
        }

        [Route("api/Admin_ProductApi/GetInvoiceDetails")]
        [HttpGet]
        public List<Admin_InvoiceDetails> GetInvoiceDetails(string invoiceId)
        {
            BaseService QL = new BaseService();
            var query = from InvoiceDetail in QL.db.InvoiceDetails
                        join Product in QL.db.Products on InvoiceDetail.product_id equals Product.product_id
                        where InvoiceDetail.invoice_id == invoiceId
                        select new
                        {
                            ProductName = Product.product_name,
                            Quantity = InvoiceDetail.quantity,
                            Img = Product.image_url,
                            Price = Product.price,
                            total= Product.price* InvoiceDetail.quantity
                        };
            var data = query.ToList();
            List<Admin_InvoiceDetails> list = new List<Admin_InvoiceDetails>();
            foreach (var item in data)
            {
                Admin_InvoiceDetails temp = new Admin_InvoiceDetails();
                temp.Name = item.ProductName;
                temp.Quantity = (item.Quantity??0);
                temp.Img = item.Img;
                temp.Price = (item.Price??0);
                temp.total = (item.total??0);
                list.Add(temp);
            }
            return list;
        }
    }
}
