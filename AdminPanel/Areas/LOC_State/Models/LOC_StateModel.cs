﻿using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Areas.LOC_State.Models
{
    public class LOC_StateModel
    {
        public int? StateID { get; set; }
        [Required]
        public string? StateName { get; set; }
        [Required]
        public string? StateCode { get; set; }
        public string? CountryName { get; set; }
        [Required]
        public int? CountryId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

    }

    public class LOC_StateDropDownModel
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
    }

    public class LOC_StateFilterModel
    {
        public int? CountryID { get; set; }
        public string? StateName { get; set; }
        public string? StateCode { get; set; }
    }
}
