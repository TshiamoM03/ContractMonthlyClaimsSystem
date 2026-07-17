using System.ComponentModel.DataAnnotations.Schema;

namespace ContractMonthlyClaimsSystem.Models
{
    public class SupportingDocument
    {
        public int SupportingDocumentId { get; set; }

        [ForeignKey("Claim")]
        public int ClaimId { get; set; }
        public virtual Claim? Claim { get; set; }

        public string FileName { get; set; }
        public byte[] FileData { get; set; }
    }
}
