namespace AccotuntsApi.Models
{
    public class ProductCategory
    {
        public int CategoryID { get; set; }

        public string? CategoryName { get; set; }

        public string? Alias { get; set;}

        public DateTime? CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; } 

        public int DeletedBy { get; set;}


    }
}
