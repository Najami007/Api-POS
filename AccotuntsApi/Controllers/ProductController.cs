using AccotuntsApi.Context;
using AccotuntsApi.Models;
using Dapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace AccotuntsApi.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly DapperContext _context;

        public ProductController(DapperContext context) => _context = context;


        [HttpGet("getproduct")]
        public IEnumerable<Product> getProduct()
        {
            var query = "select ProductID,ProductName,CategoryID,SubCategoryID,PBarcode,PBarcode1,PBarcode2,PBarcode3,"+
                "CostPrice,CTCPrice,SalePrice,WholeSalePrice,MinLimit,MaxLimit,GST,UOM from Product where IsDeleted = 0";

            using(var con = _context.CreateConnection())
            {
                var res = con.Query<Product>(query);
                return res.ToList();
            }

        }

        [HttpPost("insertproduct")]

        public IActionResult insetProduct(Product prod)
        {
            var query = "INSERT INTO Product (CategoryID,SubCategoryID,ProductName,PBarcode,PBarcode1,PBarcode2," +
                "PBarcode3,CostPrice,SalePrice,CTCPrice,WholeSalePrice,MinLimit,MaxLimit,SectionID,Gst,UOM, CreatedOn," +
                "CreatedBy,IsDeleted) " +
                "\r\nVALUES (@CategoryID, @SubCategoryID, @ProductName," +
                " @PBarcode, @PBarcode1, @PBarcode2, " +
                "@PBarcode3, @CostPrice, @SalePrice, @CTCPrice, @WholeSalePrice, @MinLimit, @MaxLimit," +
                " @SectionID,@Gst,@UOM, @CreatedOn, @CreatedBy, @IsDeleted);";
            var lastProduct = "SELECT TOP 1 * FROM Product ORDER BY ProductID DESC";

            



            var parameters = new DynamicParameters();

            parameters.Add("CategoryID", prod.CategoryID);
            parameters.Add("SubCategoryID", prod.SubCategoryID);
            parameters.Add("ProductName", prod.ProductName.Trim());
            parameters.Add("PBarcode", prod.PBarcode?.Trim());
            parameters.Add("PBarcode1", prod.PBarcode1?.Trim());
            parameters.Add("PBarcode2",prod.PBarcode2?.Trim());
            parameters.Add("PBarcode3",prod.PBarcode3?.Trim());
            parameters.Add("CostPrice", prod.CostPrice);
            parameters.Add("SalePrice", prod.SalePrice);
            parameters.Add("CTCPrice", prod.CTCPrice);
            parameters.Add("WholeSalePrice", prod.WholeSalePrice);
            parameters.Add("MinLimit", prod.MinLimit);
            parameters.Add("MaxLimit", prod.MaxLimit);
            parameters.Add("SectionID", prod.SectionID);
            parameters.Add("Gst", prod.Gst);
            parameters.Add("UOM", prod.UOM.Trim());
            parameters.Add("CreatedOn", DateTime.Now);
            parameters.Add("CreatedBy", prod.CreatedBy);
            parameters.Add("IsDeleted", false);
            

            

            using(var con = _context.CreateConnection())
            {

                /*var cbpn = "Select * from Product where ProductName = " + prod.ProductName + "and IsDeleted = 0";*/
                /*var cbpn = "Select * from Product where IsDeleted = 0";
                var resCbpn = con.Query<Product>(cbpn);
                var byName = resCbpn.Where(x => x.ProductName == prod.ProductName).ToList();
                var byBarcode = resCbpn.Where(x => prod.PBarcode != null ? x.PBarcode == prod.PBarcode:false).ToList();
                var byBarcode1 = resCbpn.Where(x => prod.PBarcode1 != null ? x.PBarcode1 == prod.PBarcode1 : false).ToList();
                var byBarcode2 = resCbpn.Where(x => prod.PBarcode2 != null ? x.PBarcode2 == prod.PBarcode2:false).ToList();
                var byBarcode3 = resCbpn.Where(x => prod.PBarcode3 != null ? x.PBarcode3 == prod.PBarcode3:false).ToList();*/

                var cbpn = con.QueryFirstOrDefault<Product>("Select * from Product where IsDeleted = 0 and ProductName = @ProductName",parameters );
                var cbpb = con.QueryFirstOrDefault<Product>("Select * from Product where IsDeleted = 0 and PBarcode = @PBarcode",parameters);
                var cbpb1 = con.QueryFirstOrDefault<Product>("Select * from Product where IsDeleted = 0 and PBarcode1 = @PBarcode1", parameters);
                var cbpb2  = con.QueryFirstOrDefault<Product>("Select * from Product where IsDeleted = 0 and PBarcode2 = @PBarcode2", parameters);
                var cbpb3 = con.QueryFirstOrDefault<Product>("Select * from Product where IsDeleted = 0 and PBarcode3 =@PBarcode3", parameters);




                if ( cbpn != null)
                {
                    return Ok("Product Name Already Exists");

                }else if(cbpb != null)
                {
                    return Ok("Product Barcode Already Exists");
                }else if(cbpb1 != null)
                {
                    return Ok("Product Barcode 1 Already Exists");
                }else if(cbpb2 != null)
                {
                    return Ok("Product Barcode 2 Already Exists");
                }else if(cbpb != null)
                {
                    return Ok("Product Barcode 3 Already Exists");
                }
                else
                {

                    if (prod.PBarcode == "" || prod.PBarcode == null)
                    {

                        var res = con.Execute(query, parameters);

                        if (res != null)
                        {
                            var SavedProduct = con.QueryFirstOrDefault<Product>(lastProduct);

                            if (SavedProduct != null)
                            {
                                var updateProduct = "Update Product Set PBarcode = " + SavedProduct.ProductID + "where ProductID =" + SavedProduct.ProductID;
                                var updateNullBarcode1 = "Update Product set PBarcode1 = NULL where PBarcode1 = '' and ProductID = "+SavedProduct.ProductID;
                                var updateNullBarcode2 = "Update Product set PBarcode2 = NULL where PBarcode2 = '' and ProductID = " + SavedProduct.ProductID;
                                var updateNullBarcode3 = "Update Product set PBarcode3 = NULL where PBarcode3 = '' and ProductID = " + SavedProduct.ProductID;


                                var newres = con.Execute(updateProduct);
                                con.Execute(updateNullBarcode1);
                                con.Execute(updateNullBarcode2);
                                con.Execute(updateNullBarcode3);
                                return Ok("Data Inserted Successfully");
                            }

                            return Ok();

                        }
                        else
                        {
                            return Ok("Error Occured While Inserting Data");
                        }


                    }

                    else
                    {
                        var updateNullBarcode1 = "Update Product set PBarcode1 = NULL where PBarcode1 = '' ";
                        var updateNullBarcode2 = "Update Product set PBarcode2 = NULL where PBarcode2 = '' " ;
                        var updateNullBarcode3 = "Update Product set PBarcode3 = NULL where PBarcode3 = '' ";

                        var res = con.Execute(query, parameters);
                        con.Execute(updateNullBarcode1);
                        con.Execute(updateNullBarcode2);
                        con.Execute(updateNullBarcode3);

                        return Ok("Data Inserted Successfully");
                    }

                }

            }

        }


        [HttpPut("updateproduct")]

        public  String updateProduct(int prodid, Product prod)
        {
            var query = "Update Product set CategoryID = @CategoryID ,SubCategoryID = @SubCategoryID,ProductName = @ProductName,PBarcode = @PBarcode," +
                "PBarcode1 = @PBarcode1 , PBarcode2=@PBarcode2, PBarcode3 = @PBarcode3 ,CostPrice = @CostPrice,CTCPrice = @CTCPrice,SalePrice = @SalePrice,"+
                "WholeSalePrice = @WholeSalePrice ,MinLimit = @MinLimit,MaxLimit = @MaxLimit,Gst = @Gst,UOM = @UOM,ModifiedOn = @ModifiedOn,ModifiedBy = @ModifiedBy where ProductID = "+ prodid;

            var parameters = new DynamicParameters();

            parameters.Add("CategoryID", prod.CategoryID);
            parameters.Add("SubCategoryID", prod.SubCategoryID);
            parameters.Add("ProductName", prod.ProductName.Trim());
            parameters.Add("PBarcode", prod.PBarcode?.Trim());
            parameters.Add("PBarcode1",prod.PBarcode1?.Trim());
            parameters.Add("PBarcode2", prod.PBarcode2?.Trim());
            parameters.Add("PBarcode3", prod.PBarcode3?.Trim());
            parameters.Add("CostPrice", prod.CostPrice);
            parameters.Add("CTCPrice", prod.CTCPrice);
            parameters.Add("SalePrice", prod.SalePrice);
            parameters.Add("WholeSalePrice", prod.WholeSalePrice);
            parameters.Add("MinLimit", prod.MinLimit);
            parameters.Add("MaxLimit", prod.MaxLimit);
            parameters.Add("Gst", prod.Gst);
            parameters.Add("UOM", prod.UOM.Trim());
            parameters.Add("ModifiedOn", DateTime.Now);
            parameters.Add("ModifiedBy", prod.ModifiedBy);

            using(var con= _context.CreateConnection())
            {

                var cbpn = con.QueryFirstOrDefault<Product>("Select * from Product where IsDeleted = 0 and ProductName = @ProductName and ProductID != " + prodid, parameters);
                var cbpb = con.QueryFirstOrDefault<Product>("Select * from Product where IsDeleted = 0 and PBarcode = @PBarcode and ProductID != " + prodid, parameters);
                var cbpb1 = con.QueryFirstOrDefault<Product>("Select * from Product where IsDeleted = 0 and PBarcode1 = @PBarcode1 and ProductID != " + prodid, parameters);
                var cbpb2 = con.QueryFirstOrDefault<Product>("Select * from Product where IsDeleted = 0 and PBarcode2 = @PBarcode2 and ProductID != " + prodid, parameters);
                var cbpb3 = con.QueryFirstOrDefault<Product>("Select * from Product where IsDeleted = 0 and PBarcode3 =@PBarcode3 and ProductID != " + prodid, parameters);
                var updateNullBarcode1 = "Update Product set PBarcode1 = NULL where PBarcode1 = '' ";
                var updateNullBarcode2 = "Update Product set PBarcode2 = NULL where PBarcode2 = '' ";
                var updateNullBarcode3 = "Update Product set PBarcode3 = NULL where PBarcode3 = '' ";



                if (cbpn != null)
                {
                    return "Product Name Already Exists";

                }
                else if (cbpb != null)
                {
                    return "Product Barcode Already Exists";
                }
                else if (cbpb1 != null)
                {
                    return "Product Barcode 1 Already Exists";
                }
                else if (cbpb2 != null)
                { 
                    return "Product Barcode 2 Already Exists";
                }
                else if (cbpb != null)
                {
                    return "Product Barcode 3 Already Exists";
                }
                else
                {


                    var res = con.Execute(query, parameters);
                    con.Execute(updateNullBarcode1);
                    con.Execute(updateNullBarcode2);
                    con.Execute(updateNullBarcode3);

                    if(res != null) {
                        return "Product Updated Successfully";
                    }
                    else
                    {
                        return "Error Occured While Updating Data";
                    }

                }
            }

        }


        [HttpPut("deleteproduct")]

        public IActionResult deleteProduct(int id)
        {
            var query = "Update Product set IsDeleted = 1 where ProductID = " + id;

            using(var con = _context.CreateConnection())
            {
                var res = con.Execute(query);

                if(res == 1)
                {
                    return Ok("Product deleted");
                }
                else
                {
                    return Ok("Error Occured While Deleting Data");
                }
            }
        }


    }
}
