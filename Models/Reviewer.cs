namespace ContractMonthlyClaimsSystem.Models
{
    public class Reviewer
    {
        public int ReviewerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        //nav property
        public virtual ICollection<ClaimReview> ClaimReviewList { get; set; } = new List<ClaimReview>();
    }
}
