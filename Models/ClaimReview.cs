using System.ComponentModel.DataAnnotations.Schema;

namespace ContractMonthlyClaimsSystem.Models
{
    public class ClaimReview
    {
        public int ClaimReviewId { get; set; }

        [ForeignKey("Claim")]
        public int ClaimId { get; set; }
        public virtual Claim Claim { get; set; }

        [ForeignKey("Reviewer")]
        public int ReviewerId { get; set; }
        public virtual Reviewer Reviewer { get; set; }

        public DateTime ReviewDate { get; set; }
        public string Decision { get; set; }
        public string? Reason { get; set; }

    }
}
//did not work without annotations
//foreign keys: https://learn.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/creating-a-more-complex-data-model-for-an-asp-net-mvc-application