using AccotuntsApi.Context;
using AccotuntsApi.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace AccotuntsApi.Controllers
{

    [ApiController]
    [Route("api/sale")]
    public class SaleController : ControllerBase
    { 
         private readonly DapperContext _context;

          public SaleController(DapperContext context) => _context = context;


        [HttpGet("getSaleInvoice")]

        public IEnumerable<Invoice> getSaleInvoice()
        {
            var query = "SELECT dbo.Party.PartyName, dbo.Invoice.InvoiceNo, dbo.Invoice.BranchID, dbo.Invoice.InvoiceDate, dbo.Invoice.ReferenceInvoiceNo,"+
                " dbo.Invoice.ReferenceInvoiceDate, dbo.Invoice.LabourRefInvoiceNo, dbo.Invoice.CashReceived,"+
                "dbo.Invoice.Changed, dbo.Invoice.Discount, dbo.Invoice.Type, dbo.Invoice.SectionID, dbo.Invoice.Remarks, dbo.Invoice.SubType, "+
                "dbo.Invoice.PartyID, dbo.Invoice.CashReturn, dbo.Invoice.IsDeleted FROM     dbo.Invoice INNER JOIN "+
                " dbo.Party ON dbo.Invoice.PartyID = dbo.Party.PartyID WHERE  (dbo.Invoice.Type = 'S') AND (dbo.Invoice.IsDeleted = 0)";
             var invDetailQuery = "Select * from Invoice Detail where InvoiceNo = @invoiceNo and IsDeleted = 0";


            var data  = new List<Invoice>();

            using(var con = _context.CreateConnection())
            {
                var res = con.Query<Invoice>(query);




                


                return res.ToList();
            }
        }
    
        [HttpPost("insert")]

        public IActionResult insertSale(Invoice invoice)
        {
            var query = "Insert INTO Invoice(BranchID,InvoiceDate,PartyID," +
                "CashReturn,CashReceived,Changed,Discount,Type,SubType,SectionID,Remarks,BookerID,CreatedOn,CreatedBy,IsDeleted)" +
                "Values(@BranchID,@InvoiceDate,@PartyID,@CashReturn,@CashReceived,@Changed,"+
                "@Discount,@Type,@SubType,@SectionID,@Remarks,@BookerID,@CreatedOn,@CreatedBy,@IsDeleted)";


            var invParameters = new DynamicParameters();

            invParameters.Add("BranchID", invoice.BranchID);
            invParameters.Add("InvoiceDate", invoice.InvoiceDate);
            invParameters.Add("PartyID", invoice.PartyID);
            invParameters.Add("CashReturn", invoice.CashReturn);
            invParameters.Add("CashReceived", invoice.CashReceived);
            invParameters.Add("Changed", invoice.Changed);
            invParameters.Add("Discount", invoice.Discount);
            invParameters.Add("Type", invoice.Type);
            invParameters.Add("SubType", invoice.SubType);
            invParameters.Add("SectionID", invoice.SectionID);
            invParameters.Add("Remarks", invoice.Remarks);
            invParameters.Add("BookerID", invoice.BookerID);
            invParameters.Add("CreatedOn", DateTime.Now);
            invParameters.Add("CreatedBy", invoice.CreatedBy);
            invParameters.Add("IsDeleted", false);

            using (var con = _context.CreateConnection())
            {

                var invRes = con.Execute(query, invParameters);

                var lastSavedInvoice = "SELECT TOP 1 InvoiceNo FROM Invoice Where CreatedBy = "+ invoice.CreatedBy +" ORDER BY InvoiceNo DESC";

              
                    var res = con.QueryFirstOrDefault(lastSavedInvoice);


                   /* var invoiceDetail = new InvoiceDetail();*/
                    var invoiceDetailList = JsonConvert.DeserializeObject<List<InvoiceDetail>>(invoice.InoviceDetails);


                  
                        var invdetailQuery = "Insert INTO InvoiceDetail (InvoiceNo,ProductID,Quantity,Scheme,CostPrice,SalePrice," +
                            "COAID,Debit,Credit,DiscountInRupees,DiscountInPercentage,Gst,Et,LocationID,ProductQuantity,CreatedOn,"+
                            "CreatedBy,IsDeleted) Values(@InvoiceNo,@ProductID,@Quantity,@Scheme,@CostPrice,@SalePrice,@COAID,@Debit,@Credit,"+
                            "@DiscountInRupees,@DiscountInPercentage,@Gst,@Et,@LocationID,@ProductQuantity,@CreatedOn,@CreatedBy,@IsDeleted)";

                var invdetailDrquery = @"Insert INTO InvoiceDetail (InvoiceNo,COAID,Debit,CreatedOn,CreatedBy,IsDeleted)"+
                                       "Values(@invoiceNo,@COAID,@Debit,@createdOn,@createdBy,@IsDeleted)";

                var invDetailDiscountQuery = "Insert INTO InvoiceDetail (InvoiceNo,COAID,Debit,CreatedOn,CreatedBy,IsDeleted) " +
                                             "Values(@invoiceNo,@COAID,@Debit,@createdOn,@createdBy,@IsDeleted)";

                ////////////////////executed the ivoice Detail data///////////////////
                        foreach (var item in invoiceDetailList)

                        {
                            var invdetailParameters = new DynamicParameters();

                            invdetailParameters.Add("InvoiceNo", res.InvoiceNo);
                            invdetailParameters.Add("ProductID", item.ProductID);
                            invdetailParameters.Add("Quantity", item.Quantity);
                            invdetailParameters.Add("Scheme", item.Scheme);
                            invdetailParameters.Add("CostPrice", item.CostPrice);
                            invdetailParameters.Add("SalePrice", item.SalePrice);
                            invdetailParameters.Add("COAID", 1);
                            invdetailParameters.Add("Debit", 0);
                            invdetailParameters.Add("Credit", item.Quantity * item.SalePrice);
                            invdetailParameters.Add("DiscountInRupees", item.DiscountInRupees);
                            invdetailParameters.Add("DiscountInPercentage", item.DiscountInPercentage);
                            invdetailParameters.Add("Gst", item.Gst);
                            invdetailParameters.Add("Et", item.Et);
                            invdetailParameters.Add("LocationID", invoice.LocationID);
                            invdetailParameters.Add("ProductQuantity", item.ProductQuantity);
                            invdetailParameters.Add("CreatedOn", DateTime.Now);
                            invdetailParameters.Add("CreatedBy", invoice.CreatedBy);
                            invdetailParameters.Add("IsDeleted", false);


                             con.Execute(invdetailQuery, invdetailParameters);

                            

                    }

                        /////////////////Ececute the Debit side of the invoice detail/////////////////
                         con.Execute(invdetailDrquery, new
                             {
                                 invoiceNo = res.InvoiceNo,
                                COAID = 2,
                                Debit = invoice.CashReceived - invoice.Changed,
                                createdOn = DateTime.Now,
                                createdBy = invoice.CreatedBy,
                                IsDeleted = false,



                                 });
                if(invoice.Discount != 0)
                {
                    con.Execute(invDetailDiscountQuery, new
                    {
                        invoiceNo = res.InvoiceNo,
                        COAID = 2,
                        Debit = invoice.Discount,
                        createdOn = DateTime.Now,
                        createdBy = invoice.CreatedBy,
                        IsDeleted = false,
                    });
                }
                             return Ok("Invoice Saved Successfully");




            }
        }
    }
}
