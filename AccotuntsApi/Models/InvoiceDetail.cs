namespace AccotuntsApi.Models
{
    public class InvoiceDetail
    {

        public int InvoiceDetailID { get; set; }

        public int InvoiceNo { get; set; }

        public int ProductID { get; set; }

        public float Quantity { get; set; }

        public float Scheme { get; set; }

        public float CostPrice { get; set; }

        public float SalePrice { get; set; }

        public int COAID { get; set; }

        public float Debit { get; set; }

        public float Credit { get; set; }

        public float DiscountInRupees { get; set; }

        public float DiscountInPercentage { get; set; }

        public float Gst { get;set; }

        public float Et { get; set; }

        public string? CreditCardNo { get; set; }

        public DateTime ExpiryDate { get; set; }    

        public int BatchNo { get; set; }

        public string? BatchStatus { get; set; } 

        public int LocationID { get; set; }

        public string? SchemeStatus { get; set; }

        public string? SchemeType { get; set; }

         public float ProductQuantity { get; set; }

        public float SchemeLimit { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public int ModifiedBy { get; set;}

        public bool IsDeleted { get; set; }

        public string? MeasurementUnit { get; set; }

        public int UomID { get; set; }

        public  float Packing { get; set; }



    }
}
