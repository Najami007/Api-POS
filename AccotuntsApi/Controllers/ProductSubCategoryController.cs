using AccotuntsApi.Context;
using AccotuntsApi.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace AccotuntsApi.Controllers
{

    [ApiController]
    [Route("api/productSubCategory")]
    public class ProductSubCategoryController : ControllerBase
    {

        private readonly DapperContext _context;

        public ProductSubCategoryController(DapperContext context) => _context = context;

        [HttpGet("getsubcategory")]

        public IActionResult GetSubCategory()
        {

            /*var query = "Select SubCategoryID,SubCategoryName,CategoryID From ProductSubCategory where IsDeleted = 0 ";*/

            var query = "SELECT dbo.ProductCategory.CategoryName, dbo.ProductSubCategory.SubCategoryID," +
                " dbo.ProductSubCategory.CategoryID, dbo.ProductSubCategory.SubCategoryName\r\nFROM " +
                "dbo.ProductCategory INNER JOIN\r\n  dbo.ProductSubCategory ON dbo.ProductCategory.CategoryID = dbo.ProductSubCategory.CategoryID";

            using (var con = _context.CreateConnection())
            {
                var res = con.Query<ProductSubCategory>(query);

                return Ok(res);
            }



        }


        [HttpPost("insertsubcategory")]

        public IActionResult insertSubCategory(ProductSubCategory subCategory)
        {
            var query = "INSERT INTO ProductSubCategory (CategoryID,SubCategoryName,CreatedOn,CreatedBy,IsDeleted)" +
                "Values(@CategoryID,@SubCategoryName,@CreatedOn,@CreatedBy,@IsDeleted)";


            var chkquery = "Select * from ProductSubCategory where SubCategoryName = @SubCategoryName and IsDeleted = 0";

            var parameters = new DynamicParameters();
            parameters.Add("CategoryID", subCategory.CategoryID);
            parameters.Add("SubCategoryName", subCategory.SubCategoryName.Trim());
            parameters.Add("CreatedOn", DateTime.Now);
            parameters.Add("CreatedBy", subCategory.CreatedBy);
            parameters.Add("IsDeleted", false);

            using(var con = _context.CreateConnection())
            {
                var condres = con.QueryFirstOrDefault<ProductSubCategory>(chkquery,parameters);

                if (condres == null)
                {
                    var insertres = con.Execute(query, parameters);
                    return Ok("Sub Category Inserted Successfully");
                }
                else
                {
                    return Ok("Sub Category Already Exists");
                }
            }


        }

        [HttpPut("updatesubcategory")]

        public IActionResult updateSubCategory(int id, ProductSubCategory subCategory)
        {
            var query = "Update ProductSubCategory set CategoryID = @CategoryID , SubCategoryName = @SubCategoryName," +
                "ModifiedOn = @ModifiedOn,ModifiedBy = @ModifiedBy where SubCategoryID ="+id;

            var chkquery = "Select * from ProductSubCategory where SubCategoryName = @SubCategoryName and IsDeleted = 0 and SubCategoryID !=" + id;


            var parameters = new DynamicParameters();

            parameters.Add("CategoryID", subCategory.CategoryID);
            parameters.Add("SubCategoryName", subCategory.SubCategoryName.Trim());
            parameters.Add("ModifiedOn", DateTime.Now);
            parameters.Add("ModifiedBy", subCategory.ModifiedBy);

            using (var con= _context.CreateConnection())
            {
                var condres = con.QueryFirstOrDefault(chkquery,parameters);
                if(condres == null)
                {
                    var updateres = con.Execute(query, parameters);

                    return Ok("Sub Category Updated Succesfully");
                }
                else
                {
                    return Ok("Sub Category Already Exists");
                }
            }
        }



        [HttpGet("getbycatgoryID")]

        public IActionResult getSubcategoryByID(int Id)
        {
            var query = "Select SubCategoryID,SubCategoryName from ProductSubCategory where isDeleted = 0 and CategoryID = " + Id;

            using (var con = _context.CreateConnection())
            {
                var res = con.Query<ProductSubCategory>(query);

                return Ok(res);
            }

        }

    }
}
