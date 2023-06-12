using AccotuntsApi.Context;
using AccotuntsApi.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace AccotuntsApi.Controllers
{

    [ApiController]
    [Route("api/productCategory")]
    public class ProductCategoryController : ControllerBase
    {
        private readonly DapperContext _context;

        public ProductCategoryController(DapperContext context) => _context = context;


        [HttpGet("getCategory")]
        public IActionResult getCategory()
        {
            var query = "Select CategoryID,CategoryName from ProductCategory where IsDeleted = 0";

            using(var con= _context.CreateConnection())
            {
                var res = con.Query<ProductCategory>(query);

                return Ok(res);
            }
        }

        [HttpPost("insertcategory")]


            public IActionResult InsertCategory(ProductCategory category) 
        {

            var query = "Insert INTO ProductCategory  (CategoryName,CreatedOn,CreatedBy,IsDeleted)" +
                "Values(@CategoryName,@CreatedOn,@CreatedBy,@IsDeleted)";

            var chkquery = "Select * from ProductCategory where CategoryName = @CategoryName and IsDeleted = 0";

            var parameters = new DynamicParameters();

            parameters.Add("CategoryName", category.CategoryName.Trim());
            parameters.Add("CreatedOn", DateTime.Now);
            parameters.Add("CreatedBy", category.CreatedBy);
            parameters.Add("IsDeleted", false);

            using (var con = _context.CreateConnection())
            {
                

                var cond = con.QueryFirstOrDefault<ProductCategory>(chkquery,parameters);

                if(cond != null)
                {
                    return Ok("Category Already Exists");
                }
                else
                {
                    con.Execute(query, parameters);
                    return Ok("Category Inserted Succesfully");
                }
            
            }
        
        
        
        }



        [HttpPut("updatecategory")]

        public IActionResult updateCategory(int id,ProductCategory category)
        {
            var query = "Update ProductCategory set CategoryName = @CategoryName,ModifiedOn= @ModifiedOn," +
                "ModifiedBy = @ModifiedBy where CategoryID = "+id;


           /* var delquery = "Update ProductCategory set CategoryName = @CategoryName,ModifiedOn = @ModifiedOn,"+
                "ModifiedBy= @ModifiedBy  where CategoryID = " + id;*/

            var chkquery = "Select * from ProductCategory where CategoryName = @CategoryName and IsDeleted = 0 and  CategoryID !=" + id;

            /*var insertquery = "set Identity_insert ProductCategory On;" +
                "Insert Into ProductCategory(CategoryID,CategoryName,ModifiedOn,ModifiedBy,IsDeleted)" +
                "values(@CategoryID,@CategoryName,@ModifiedOn,@ModifiedBy,@IsDeleted);"+
                "set Identity_insert ProductCategory off;";
*/
            var parameters = new DynamicParameters();

            parameters.Add("CategoryID", id);
            parameters.Add("CategoryName", category.CategoryName.Trim());
            parameters.Add("ModifiedOn", DateTime.Now);
            parameters.Add("ModifiedBy", category.ModifiedBy);
         

            using(var con = _context.CreateConnection())
            {
                var condres = con.QueryFirstOrDefault<ProductCategory>(chkquery,parameters);
                if(condres == null )
                {
                    var updateres = con.Execute(query,parameters);

                    if (updateres == 1)
                    {
                        return Ok("Category Updated Succesfully");
                    }
                    else
                    {
                        return Ok("Unable to Update Data");
                    }

                }
                else
                {
                    return Ok("Category Already Exists");
                }
            }

           
            
        }


    }
}
