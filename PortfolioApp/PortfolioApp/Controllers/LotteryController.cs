using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortfolioApp.Models.Db.LotteryDb;
using PortfolioApp.Models.ViewModels;

namespace PortfolioApp.Controllers
{
    public class LotteryController : Controller
    {
        // GET: Lottery
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Country(Guid countryId)
        {
            LotteryStoreVm storeVm = new LotteryStoreVm();
            
            using (var _db = new LotteryDbEntities())
            {
                try
                {
                    var countries = _db.CountryTables.ToList();
                    var selectedCountries = _db.CountryUserSelectedTables.ToList();
                    storeVm.SelectedList = new List<PreSelectedCountries>();
                    foreach (var c in countries)
                    {
                        bool isSelected = false;
                        string ownerName = "None";
                        var t = selectedCountries.SingleOrDefault(x => x.CountryId == c.CountryId);
                        if (t != null)
                        {
                            isSelected = true;
                            ownerName = _db.CountryUserTables.SingleOrDefault(x => x.UserId == t.UserId).Name;
                        }
                        var pre = new PreSelectedCountries()
                        {
                            CountryId = c.CountryId,
                            IsSelected = isSelected,
                            OwnerName = ownerName,
                            CountryName = c.Name,
                            ImageUrl = "../../Images/Lottery/"+c.Name+".png"
                        };
                        storeVm.SelectedList.Add(pre);
                    }
                    var id = countryId.ToString();
                    storeVm.UserId = id;
                    var user = _db.CountryUserTables.FirstOrDefault(x => x.RegId == id);
                    if (user == null)
                    {
                        //User doesnt exist
                        return null;
                    }
                    var hasSelected = _db.CountryUserSelectedTables.SingleOrDefault(x => x.UserId == user.UserId);
                    if (hasSelected != null)
                    {
                        //User has already picked a country
                        storeVm.HasPicked = true;
                    }
                }
                catch (Exception ex)
                {
                        
                    throw;
                }
                
            }
            
            return View("~/Views/Lottery/Country.cshtml", storeVm);
        }

        [HttpPost]
        public ActionResult PickRandomCountry(string userId)
        {
            LotteryStoreVm storeVm = new LotteryStoreVm();
            using (var _db = new LotteryDbEntities())
            {
                try
                {
                    var user = _db.CountryUserTables.SingleOrDefault(x => x.RegId == userId);
                    if (user == null) return null;
                    var countries = _db.CountryTables.ToList();
                    var selectedCountries = _db.CountryUserSelectedTables.ToList();
                    storeVm.SelectedList = new List<PreSelectedCountries>();
                    foreach (var c in countries)
                    {
                        bool isSelected = false;
                        string ownerName = "None";
                        var t = selectedCountries.SingleOrDefault(x => x.CountryId == c.CountryId);
                        if (t != null)
                        {
                            isSelected = true;
                            ownerName = _db.CountryUserTables.SingleOrDefault(x => x.UserId == t.UserId).Name;
                        }
                        var pre = new PreSelectedCountries()
                        {
                            CountryId = c.CountryId,
                            IsSelected = isSelected,
                            OwnerName = ownerName,
                            CountryName = c.Name,
                            ImageUrl = "../../Images/Lottery/" + c.Name + ".png"
                        };
                        storeVm.SelectedList.Add(pre);
                        
                    }
                    int index = RandomCountryId(storeVm.SelectedList);
                    
                    _db.CountryUserSelectedTables.Add(new CountryUserSelectedTable()
                    {
                        CountryId = index,
                        UserId = user.UserId
                    });
                    _db.SaveChanges();

                }
                catch (Exception ex)
                {

                    throw;
                }

            }
            return RedirectToAction("Country",new { countryId = userId });
            
        }

        private int RandomCountryId(List<PreSelectedCountries> countries)
        {
            Random r = new Random();
            int index = r.Next(1, 4);
            var res = from c in countries
                      where c.CountryId == index && c.IsSelected
                      select c;
            if (res.Any())
            {
                RandomCountryId(countries);
            }
            return index;
        }
    }
}