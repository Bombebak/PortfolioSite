using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortfolioApp.Models.Db.LotteryDb;

namespace PortfolioApp.Models.ViewModels
{
    public class LotteryStoreVm
    {
        public List<PreSelectedCountries> SelectedList { get; set; }
        public List<CountryTable> CountryList { get; set; } 
        public bool HasPicked { get; set; }
        public string UserId { get; set; }
    }

    public class PreSelectedCountries
    {
        public int CountryId { get; set; }
        public string ImageUrl { get; set; }
        public string OwnerName { get; set; }
        public string CountryName { get; set; }
        public bool IsSelected { get; set; }
    }
}