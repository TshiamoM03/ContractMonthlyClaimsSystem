using System.ComponentModel.DataAnnotations.Schema;

namespace ContractMonthlyClaimsSystem.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }

        [ForeignKey("Lecturer")]
        public int LecturerId { get; set; }
        public virtual Lecturer? Lecturer { get; set; }

        public DateTime SubmissionDate { get; set; }
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public double ClaimAmount { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; } = "Pending";


        //navigation properties
        public virtual ICollection<SupportingDocument> SupportingDocumentList { get; set; } = new List<SupportingDocument>();  //claim [1 - 1..*] document
        public virtual ClaimReview? ClaimReview { get; set; }

    }
}
