﻿using System.Collections.Generic;
using Topcat_Cat_Hotel.Models.Enums;

namespace Topcat_Cat_Hotel.ViewModels
{
    public class RegisterCatViewModel
    {
        public int NumOfCats { get; set; }
        public List<CatDetailsViewModel> Cats { get; set; } = new List<CatDetailsViewModel>();
        public string OwnersName { get; set; }
        public string OwnersAddress { get; set; }
        public string OwnersPostcode { get; set; }
        public string OwnersMobile { get; set; }
        public string GeneralHealthDetails { get; set; }
        public string VetDetails { get; set; }
    }

    public class CatDetailsViewModel
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public int? Age { get; set; }
        public Gender Sex { get; set; }
        public bool Microchipped { get; set; }
        public string MicrochipNumber { get; set; }
        public bool Insured { get; set; }
        public string InsuranceCompany { get; set; }
        public string InsurancePolicyNumber { get; set; }
    }
}
