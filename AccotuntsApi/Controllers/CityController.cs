using AccotuntsApi.Context;
using AccotuntsApi.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace AccotuntsApi.Controllers
{
    [ApiController]
    [Route("api/city")]
    public class CityController : ControllerBase
    {

        private readonly DapperContext _context;

        public CityController(DapperContext context) => _context = context;


        [HttpGet("getCity")]
        public IEnumerable<City> getCity()
        {

            var query = "Select CityID,CityName from City Where isDeleted = 0 ";

            var data = new City();
            using(var con= _context.CreateConnection())
            {
                var response = con.Query<City>(query);



                return response.ToList();
            }
        }

        [HttpPost("insertcity")]
        public String insetCity(City city) 
        {
            var query = "INSERT INTO City (CityName,CreatedOn,CreatedBy,IsDeleted)" +
                "VAlUES(@CityName,@CreatedOn,@CreatedBy,@IsDeleted) ";

            var chkquery = "Select * from City where CityName = @CityName and IsDeleted = 0";

            var parameter = new DynamicParameters();

            parameter.Add("CityName", city.CityName.Trim());
            parameter.Add("CreatedOn", DateTime.Now);
            parameter.Add("CreatedBy", city.CreatedBy);
            parameter.Add("isDeleted", false);

            using(var con = _context.CreateConnection())
            {
                var cond = con.QueryFirstOrDefault<City>(chkquery,parameter);
                if(cond != null)
                {
                    return "City Name Already Exists";
                }
                else
                {

                    var res = con.Execute(query, parameter);

                    if (res == 1)
                    {
                        return "City Added Successfully";
                    }
                    else
                    {
                        return "Error Occurred While Adding Data";
                    }
                }
            }
        }


        [HttpPut("updatecity")]

        public string updateCity(int id,City city)
        {
            var query = "Update City Set CityName = @CityName ,ModifiedOn = @ModifiedOn,ModifiedBy = @ModifiedBy where CityID = "+id;

            var chkquery = "Select * from City where CityName = @CityName and IsDeleted = 0 and CityID != " + id;
            var parameter = new DynamicParameters();
            parameter.Add("CityName", city.CityName.Trim());
            parameter.Add("ModifiedOn", DateTime.Now);
            parameter.Add("ModifiedBy", city.ModifiedBy);
            

            using(var con =_context.CreateConnection())
            {

                var cond = con.QueryFirstOrDefault<City>(chkquery, parameter);
                if(cond == null) {
                    var res = con.Execute(query, parameter);

                    if (res == 1)
                    {
                        return "Data Updated Successfully";

                    }
                    else
                    {
                        return "Error Occured While Updating Data";
                    }
                }
                else
                {
                    return "City Name Already Exists";
                }

            }


        }





        [HttpPut("deletecity")]
        public String deleteCity(City city,int id)
        {
            var query = "UPDATE City Set IsDeleted = 1 ,DeletedOn = @DeletedOn, DeletedBy = @DeletedBy where CityID = " + id;

                var parameter = new DynamicParameters();

            parameter.Add("DeletedOn", DateTime.Now);
            parameter.Add("DeletedBy", city.DeletedBy);

            using (var con = _context.CreateConnection())
            {

                var res = con.Execute(query, parameter);
                if(res == 0)
                {
                    return "Unable To Delete";
                }
                else
                {
                    return "City Deleted Successfully";
                }


            }





        }
        
    }
}
