namespace ContractMonthlyClaimsSystem.Models
{
    public class Lecturer
    {
        public int LecturerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        //nav property
        public virtual ICollection<Claim> ClaimList { get; set; } //lecturer [1 - 0..*] claim
    }
}
