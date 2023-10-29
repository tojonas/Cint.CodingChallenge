using System.ComponentModel.DataAnnotations;

namespace Cint.CodingChallenge.Model
{
    public class Survey
    {
        public Guid Id { get; set; }

        [StringLength(255, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string Description { get; set; } = string.Empty;

        public int LengthMinutes { get; set; } = 0;

        public double IncentiveEuros { get; set; } = 0.0;
    }
}

