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
    [Route("api/user")]
    
   


    public class UserController : ControllerBase
    {

        private readonly DapperContext _context;

        public UserController(DapperContext context)=> _context = context;


        /*[HttpGet("acc/GetUserData")]*/

        [HttpGet("getUserData")]
        public  IEnumerable<User> getUserData()
        {
            var query = "Select * from UserProfile where IsDeleted = 0";


            using(var connection = _context.CreateConnection())
            {
                
                var Users =  connection.Query<User>(query);
            return Users.ToList();

                
            }
            
        }


        /* Getting data of user by id   */
        [HttpGet("getUserData{id}",Name = "getUserById")]
        public  User getuser(string id)
        {
            var query = "Select * from UserProfile where UserID = @id";
            using( var con = _context.CreateConnection())
            {
                var userData = con.QuerySingleOrDefault<User>(query,new {id});
                return userData;
            }
;        }

        /* to Insert data in userprofile table   */
        [HttpPost("insertUserData")]
        public async Task<String> insertUser(User user)
        {
            var query = "INSERT INTO UserProfile (UserName,UserEmail,UserPassword,SectionID,RoleID,CreatedOn,CreatedBy,IsDeleted)" +
                "VALUES(@userName,@userEmail,@userPassword,@SectionID,@RoleID,@createdOn,@createdBy,@isDeleted)";

            var chkquery = "Select UserName from UserProfile where  UserEmail = @userEmail AND IsDeleted = 0" ;

            var parameters = new DynamicParameters();

            parameters.Add("userName", user.UserName.Trim());
            parameters.Add("userEmail", user.UserEmail.Trim());
            parameters.Add("userPassword", user.UserPassword.Trim());
            parameters.Add("SectionID", user.SectionID);
            parameters.Add("RoleID", user.RoleID);
            parameters.Add("createdOn", DateTime.Now);
            parameters.Add("CreatedBy", user.UserID);
            parameters.Add("isDeleted", false);
       

            using (var con= _context.CreateConnection())
            {

                ///// query checks whether the data already exists or not by username or userEmail
                var cond =  con.QueryFirstOrDefault(chkquery,parameters);

                
                    if (cond == null)

                    {
                        await con.ExecuteAsync(query, parameters);
                        return "User Added Succesfully";

                    }
                    else
                    {
                        return "User Already Exists";
                    }
               
               
            }



        }



        /*Login Authantication function path =  api/user/auth   */
        [HttpPost]
        [Route("auth")]
        public async Task<userRes> LoginUser(login user)
        {
            Guid token = Guid.NewGuid();
            var query = "Select UserID,UserName,UserEmail,SectionID,RoleID from UserProfile where UserEmail = '" +
                user.LoginEmail + "' AND  UserPassword = '" +user.LoginPassword+"' AND IsDeleted = 0";


            /*var parameters = new DynamicParameters();

            parameters.Add("UserEmail", user.LoginEmail.Trim(),DbType.String);
            parameters.Add("UserPassword", user.LoginPassword.Trim(),DbType.String);
*/
            using (var con= _context.CreateConnection())
            {
               var response =await con.QueryFirstOrDefaultAsync(query);
                var data = new userRes();

               


                if (response != null)
                {
                    data.token = token.ToString();
                    data.UserID = response.UserID;
                    data.UserEmail = response.UserEmail;
                    data.UserName = response.UserName;
                    data.UserPassword = response.UserPassword;
                    data.SectionID = response.SectionID;
                    data.RoleID = response.RoleID;

                    return data;
                }
                else

                return null;


            }

        }


        [HttpPut("updateUserData")]
        public async Task<String> updateUser(int id, User user)

        {
            ////////// query executes the updata data command
            var query = "UPDATE UserProfile SET UserName = @UserName , UserEmail = @UserEmail "+
                ", UserPassword = @UserPassword,SectionID =@SectionID,RoleID=@RoleID, ModifiedOn = @ModifiedOn , ModifiedBy = @ModifiedBy  WHERE UserID = @UserID";

            ///////// chkquery verifies whether the updated email already possessed by some other user or not
            var chkquery = "Select * from UserProfile where  UserEmail = @UserEmail And UserID != @UserID AND IsDeleted = 0";

            var parameters = new DynamicParameters();

            parameters.Add("UserID", id);
            parameters.Add("UserName", user.UserName.Trim());
            parameters.Add("UserEmail",user.UserEmail.Trim());
            parameters.Add("SectionID", user.SectionID);
            parameters.Add("RoleID", user.RoleID);
            parameters.Add("ModifiedOn", DateTime.Now);
            parameters.Add("ModifiedBy", user.UserID);
            parameters.Add("UserPassword", user.UserPassword.Trim());
            


            using(var con= _context.CreateConnection())
            {
               /* var cond = await con.QueryFirstOrDefaultAsync<User>(chkquery, parameters);
                var res = await con.ExecuteAsync(query, parameters);

                if (res == null)
                {
                    return "Error Occured";
                }
                else
                {
                    return "User Data Updated Succesfully";
                }*/

                var cond = await con.QueryFirstOrDefaultAsync<User>(chkquery, parameters);
                if (cond == null)
                {
                    var response = await con.ExecuteAsync(query, parameters);
                    if(response != null)
                    {
                        return "User Data Updated Succesfully";
                    }
                    else
                    {
                        return "Unable to Update Data";
                    }

                }
                else
                {
                    return "Email Must be Different from Other Users!";
                }
            }



        }




        /*   Delete func for the userdata */

        [HttpPut("DeleteUser")]
        public async Task<int> deleteUser(int id, deleteUser user)
        {
      
            
            
            var query = "UPDATE UserProfile SET DeletedOn =@DeletedOn, DeletedBy = @DeletedBy, IsDeleted = @IsDeleted WHERE UserID = @UserID";

            var parameters = new DynamicParameters();

            parameters.Add("UserID",id,DbType.Int32);
            parameters.Add("DeletedOn", DateTime.Now);
            parameters.Add("DeletedBy", user.UserID);
            parameters.Add("IsDeleted", true, DbType.Boolean);

            using( var con = _context.CreateConnection())
            {
                
              var value = await  con.ExecuteAsync(query, parameters);
                return value;
                
            }



        }
    }
}
