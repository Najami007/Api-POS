namespace AccotuntsApi.Models
{
    public class Invoice
    {

        public int InvoiceNo { get; set; }

        public int BranchID { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public int ReferenceInvoiceNo { get; set; }

        public DateTime? ReferenceInvoiceDate { get; set; }

        public int LabourRefInvoiceNo { get; set; } 

        public int PartyID { get; set; }

        public string? PartyName { get; set; }

        public float CashReturn { get; set; }

        public float CashReceived { get; set; }

        public float Changed { get; set; }

        public float Discount { get; set; }

        public string? Type { get; set; }

        public string? SubType { get; set; }

        public int SectionID { get; set; }

        public string? Remarks { get; set; }

        public int BookerID { get; set; }

        public string? Relation { get; set; }

        public string? PatientName { get; set; }

        public DateTime CreatedOn { get; set; }


        public int CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public int ModifiedBy { get; set;}

        public bool IsDeleted { get; set; }

        public DateTime DeletedOn { get; set; }

        public int DeletedBy { get; set;}

        public string? HdnRemarks { get; set; }

        public  string? SaleRateType { get; set; }

        public int ApprovedBy { get; set; }

        public DateTime ApprovedOn { get; set; }

        public bool ApprovedStatus { get; set; }

        public string InoviceDetails { get; set; }

        public int LocationID { get; set; }








    }
}
