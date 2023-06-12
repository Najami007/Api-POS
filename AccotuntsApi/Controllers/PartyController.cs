using AccotuntsApi.Context;
using AccotuntsApi.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace AccotuntsApi.Controllers
{

    [ApiController]
    [Route("api/party")]
    public class PartyController : ControllerBase
    {

        private readonly DapperContext _context;

        public PartyController(DapperContext context) => _context = context;


        [HttpGet("getParty")]
        public IEnumerable<Party> getpartyData()
        {
            /*var query = "Select * from Party where IsDeleted = 0";*/

            var query = "SELECT dbo.Party.PartyID AS Expr1, dbo.Party.PartyName AS Expr2," +
                " dbo.Party.PartyAddress AS Expr3, dbo.Party.PartyCNIC AS Expr4," +
                " dbo.Party.Type AS Expr5, dbo.Party.PartyAlias AS Expr6, dbo.Party.CityID AS Expr7," +
                " \r\n dbo.Party.PhoneNo AS Expr8, dbo.Party.MobileNo AS Expr9, dbo.Party.PartyID," +
                " dbo.Party.PartyName, dbo.Party.PartyAddress, dbo.Party.PartyCNIC, dbo.Party.PartyAlias," +
                " dbo.Party.Type, dbo.Party.CityID, dbo.Party.PhoneNo, \r\n dbo.Party.MobileNo, " +
                "dbo.Party.Email, dbo.Party.Remarks, dbo.Party.DesignationsID, dbo.Party.CreatedOn," +
                " dbo.Party.CreatedBy, dbo.Party.ModifiedOn, dbo.Party.ModifiedBy, dbo.Party.IsDeleted," +
                " dbo.Party.ActiveStatus, \r\n  dbo.Party.DeletedBy, dbo.Party.DeletedOn, " +
                "dbo.City.CityName\r\nFROM     dbo.Party INNER JOIN\r\n dbo.City ON dbo.Party.CityID = dbo.City.CityID\r\nWHERE  (dbo.Party.IsDeleted = 0)";

            using ( var con = _context.CreateConnection())
            {

                var response = con.Query<Party>(query);

                return response.ToList();

            }

        }


        [HttpPost("insertparty")]
        public string insertParty(Party party)

        {

          
            var query = "INSERT INTO Party (PartyName,PartyAddress,PartyCNIC,Type,"+
                "CityID,PhoneNo,MobileNo,Remarks,CreatedOn,CreatedBy,IsDeleted,ActiveStatus)"+
                "VALUES(@PartyName,@PartyAddress,@PartyCNIC,@Type,@CityID,@PhoneNo,@MobileNo,@Remarks,"+
                "@CreatedOn,@CreatedBy,@IsDeleted,@ActiveStatus)";

            var chkquery = "Select * from Party where PartyCNIC = @PartyCNIC And IsDeleted = 0 ";

            var parameters = new DynamicParameters();

            parameters.Add("PartyName", party.PartyName.Trim());
            parameters.Add("PartyAddress",party.PartyAddress.Trim());
            parameters.Add("PartyCNIC",party.PartyCNIC.Trim());
            parameters.Add("Type",party.Type);
            parameters.Add("CityID", party.CityID);
            parameters.Add("PhoneNo",party.PhoneNo.Trim());
            parameters.Add("MobileNo",party.MobileNo.Trim());
            parameters.Add("Remarks",party.Remarks.Trim());
            parameters.Add("CreatedOn", DateTime.Now);
            parameters.Add("CreatedBy", party.CreatedBy);
            parameters.Add("IsDeleted", false);
            parameters.Add("ActiveStatus", true);
        

            using(var con = _context.CreateConnection())

            {

                /* var response = con.Execute(query, parameters);

                 if (response != null)
                 {
                     return "Data Added Successfully";
                 }
                 else
                 {
                     return "Error Occured";
                 }*/

                var res = con.QueryFirstOrDefault<Party>(chkquery, parameters);
                if (res == null)
                {
                    var response = con.Execute(query, parameters);

                    if (response != null)
                    {
                        return "Data Added Successfully";
                    }
                    else
                    {
                        return "Error Occured";
                    }
                }
                else
                {
                    return "Not a Valid CNIC No.";
                }
            }


        }



        [HttpPut("updateparty")]
        public string updateParty(int id,Party party) {


            var query = "UPDATE Party SET PartyName = @PartyName , PartyAddress = @PartyAddress ,"+
                " PartyCNIC = @PartyCNIC , Type = @Type , CityID = @CityID,"+
                " PhoneNo = @PhoneNo, MobileNo = @MobileNo , ModifiedOn = @ModifiedOn ,"+
                " ModifiedBy = @ModifiedBy , Remarks = @Remarks where PartyID = " + id;

            var chkquery = "Select * from Party Where PartyCNIC = @PartyCNIC and IsDeleted = 0 and PartyID != " + id;


            var parameters = new DynamicParameters();
            parameters.Add("PartyName", party.PartyName.Trim());
            parameters.Add("PartyAddress", party.PartyAddress.Trim());
            parameters.Add("PartyCNIC", party.PartyCNIC.Trim());
            parameters.Add("Type", party.Type.Trim());
            parameters.Add("CityID", party.CityID);
            parameters.Add("PhoneNo", party.PhoneNo.Trim());
            parameters.Add("Remarks", party.Remarks.Trim());
            parameters.Add("MobileNo", party.MobileNo.Trim());
            parameters.Add("ModifiedOn", DateTime.Now);
            parameters.Add("ModifiedBy", party.ModifiedBy);



            using (var con = _context.CreateConnection())
            {
                var res = con.QueryFirstOrDefault<Party>(chkquery, parameters);

                if(res == null)
                {
                    var response = con.Execute(query, parameters);

                    if (response != null)
                    {
                        return "Data Updated Succesfully";
                    }
                    else
                    {
                        return "Error Occured While Updateing Data";
                    }
                }
                else
                {
                    return "Not a Valid CNIC No.";
                }
            }

        
        }

        [HttpPut("deleteParty")]
        public string deleteParty(int id,Party party)
        {
            var query = "Update Party set IsDeleted = 1 ,DeletedOn = @DeletedOn, DeletedBy = @DeletedBy where PartyID = " + id;

            var parameters = new DynamicParameters();
            parameters.Add("DeletedOn", DateTime.Now);
            parameters.Add("DeletedBy", party.DeletedBy);

            using(var con = _context.CreateConnection())
            {
                var response = con.Execute(query,parameters);

                if(response == 1)
                {
                    return "Party Deleted Successfully";

                }else
                { return "Error Occured While Deleting the party"; }
            }
        }

    }
}
