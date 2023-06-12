namespace AccotuntsApi.Models
{
    public class ProductSubCategory
    {
        public int SubCategoryID { get; set; }

        public int CategoryID { set; get; }

        public string? CategoryName { set; get; }

        public string? SubCategoryName { get; set; }

        public string? Alias { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public int DeletedBy { get; set; }
    }
}
