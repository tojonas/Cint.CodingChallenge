﻿using System.ComponentModel.DataAnnotations;

namespace Cint.CodingChallenge.Web.ViewModels
{
    public class SurveyCreateViewModel
    {
        [StringLength(255, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
        [StringLength(255)]
        public string Description { get; set; } = string.Empty;
        public int Number { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "The field must be a positive number.")]
        public double IncentiveEuros { get;  set; }
        [Range(0, int.MaxValue, ErrorMessage = "The field must be a positive number.")]
        public int LengthMinutes { get; set; }
    }
}
