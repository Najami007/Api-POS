using System.ComponentModel.DataAnnotations;

namespace AccotuntsApi.Models
{
    public class Product
    {
        [Key]
        public int ProductID {get; set;}

        public int CategoryID { get; set;}  

        public int SubCategoryID { get; set;}

        public string? ProductName { get; set;}

        public string? PBarcode { get; set;}

        public string? PBarcode1 { get; set;}

        public string? PBarcode2 { get; set;}

        public string? PBarcode3 { get; set;}


        public float CostPrice { get; set;}

        public float SalePrice { get; set;}

        public float CTCPrice { get; set;}

        public float WholeSalePrice { get; set;}

        public int MinLimit { get; set;}

        public int MaxLimit { get; set;}    

        public int SectionID { get; set;}

        public int Gst { get; set;} 

        public string? UOM { get; set;} 

        public DateTime CreatedOn { get; set;}

        public int CreatedBy { get; set;}

        public DateTime ModifiedOn { get; set;}

        public int ModifiedBy { get; set;}

        public bool IsDeleted { get; set;}

        public DateTime DeletedOn { get; set;}

        public int DeletedBy { get; set;}


    }
}
