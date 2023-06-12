using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AccotuntsApi.Models
{
    public class User
    {
        [Key]
        public int UserID {get;set;}

        public string? UserName { get; set; }

        public string? UserEmail { get; set; }

        public string? UserPassword { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public int ModifiedBy { get; set; }

        public Boolean IsDeleted { get; set; }
        
        public string? token { get; set; }

        public int SectionID { get; set; }

        public int RoleID { get; set; }
    }

    public class deleteUser
    {
        public int UserID { get; set; }
    }
}
