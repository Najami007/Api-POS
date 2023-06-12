namespace AccotuntsApi.Models
{
    public class City
    {
        public int CityID { get; set; }

        public string? CityName { get; set; }

        public string? Description { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int ModifiedBy { get; set;}

        public DateTime? DeletedOn { get; set; }

        public int DeletedBy { get; set; }   

        public bool IsDeleted { get; set; }


    }
}
