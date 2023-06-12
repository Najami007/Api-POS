using System.ComponentModel.DataAnnotations;

namespace AccotuntsApi.Models
{
    public class login
    {
        public string? LoginEmail { get; set; }

        public string? LoginPassword { get; set; }
    }

     public class userRes
    {
       
        public int UserID { get; set; }

        public string? UserName { get; set; }

        public string? UserEmail { get; set; }

        public string? UserPassword { get; set; }

        public int? SectionID { get; set; }

        public int? RoleID { get; set; } 

        public string? token { get; set; }




    }
}
