using System.ComponentModel.DataAnnotations;

namespace AccotuntsApi.Models
{
    public class Party
    {
       
        public int PartyID { get; set; }

        public string? PartyName { get; set; }

        public string? PartyAddress { get; set; }

        public string? PartyCNIC { get; set; }

        public string? PartyAlias { get; set; }  

        public string? Type { get; set; }

        public int CityID { get; set; }

        public string? PhoneNo { get; set; }

        public string? MobileNo { get; set; }

        public string? Remarks { get; set; } 

        public int CreatedBy { get; set; }

        public int ModifiedBy { get; set; }

        public int DeletedBy { get; set; }

        public string? CityName { get; set; }


       


    }
}
