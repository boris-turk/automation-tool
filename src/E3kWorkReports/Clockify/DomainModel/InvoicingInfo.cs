using System.Runtime.Serialization;

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class InvoicingInfo
    {
        [DataMember(Name = "invoiceId")]
        public string InvoiceId { get; set; }

        [DataMember(Name = "invoiceNumber")]
        public string InvoiceNumber { get; set; }

        [DataMember(Name = "manuallyInvoiced")]
        public bool ManuallyInvoiced { get; set; }
    }
}