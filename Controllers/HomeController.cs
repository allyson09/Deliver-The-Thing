using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeliverTheThing.Models;
using ShipEngine.ApiClient.Api;
using ShipEngine.ApiClient.Client;
using ShipEngine.ApiClient.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using LogReg.Connectors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DeliverTheThing.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if(Request.Cookies["userCookie"] != null)
            {
                return RedirectToAction("Home");
            }
            return View();
        }
        [HttpPost]
        [Route("Register")]
        public IActionResult Register(LogRegModels model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    List<Dictionary<string, object>> user = DbConnector.Query($"SELECT * FROM user WHERE email = '{model.Reg.Email}'");
                    if (user.Count > 0)
                    {
                        TempData["ValError"] = "This email already exists.";
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception e)
                {
                    Debug.Print("Exception when querying for user: " + e.Message );
                }
                PasswordHasher<Register> hasher = new PasswordHasher<Register>();
                string hashedPassword = hasher.HashPassword(model.Reg, model.Reg.Password);
                try
                {
                    DbConnector.Execute($"INSERT INTO user(Firstname, Lastname, Email, Password, createdAt, updatedAt) VALUES('{model.Reg.Firstname}', '{model.Reg.Lastname}', '{model.Reg.Email}', '{hashedPassword}', '{DateTime.Now.ToString()}', '{DateTime.Now.ToString()}')");
                    try
                    {
                        string id = DbConnector.Query($"SELECT iduser FROM user WHERE email = '{model.Reg.Email}'")[0]["iduser"].ToString();
                        CookieOptions option = new CookieOptions();
                        option.Expires = DateTime.Now.AddMinutes(60);
                        Response.Cookies.Append("userCookie", id, option);
                    }
                    catch (Exception e)
                    {
                        Debug.Print("Exception when baking cookie: " + e.Message );
                    }
                    HttpContext.Session.SetString("userEmail", model.Reg.Email);
                    return Redirect("Home");
                }
                catch (Exception e)
                {
                    Debug.Print("Exception when inserting new user: " + e.Message );
                }
            } 
            return View("Index");
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LogRegModels model)
        {
            if(ModelState.IsValid)
            {
                string FormPassword = model.Log.Password;
                string query = $"SELECT * FROM user WHERE email = '{model.Log.Email}'";
                List<Dictionary<string, object>> user = new List<Dictionary<string, object>>();
                try
                {
                    user = DbConnector.Query(query);
                    if(user.Count == 0)
                    {
                        TempData["ValError"] = "This email does not exist.";
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception e)
                {
                    Debug.Print("Exception when querying for user:" + e.Message);
                }
                PasswordHasher<List<Dictionary<string, object>>> Hasher = new PasswordHasher<List<Dictionary<string, object>>>();
                if(Hasher.VerifyHashedPassword(user, user[0]["password"].ToString(), model.Log.Password) != 0)
                {
                    try
                    {
                        object id = DbConnector.Query($"SELECT iduser FROM user WHERE email = '{model.Log.Email}'")[0]["iduser"];
                        string idString = id.ToString();
                        CookieOptions option = new CookieOptions();
                        option.Expires = DateTime.Now.AddMinutes(60);
                        Response.Cookies.Append("userCookie", idString, option);
                    }
                    catch (Exception e)
                    {
                        Debug.Print("Exception when baking cookie: " + e.Message );
                    }
                    HttpContext.Session.SetString("userEmail", model.Log.Email);
                    return RedirectToAction("Home");
                }
                else
                {
                    TempData["ValError"] = "Incorrect password.";
                    return RedirectToAction("Index");
                }
            }
            return View("Index");
        }
        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("userCookie");
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("Home")]
        public IActionResult Home() 
        {
            ViewBag.Logout = "Logout";
            ViewBag.User = new List<Dictionary<string, object>>();
            ViewBag.Rates = new List<Dictionary<string, object>>();
            if(Request.Cookies["userCookie"] != null)
            {
                try
                {
                    //Do inner join to get ship_to address for each rate
                    ViewBag.User = DbConnector.Query($"SELECT * FROM user WHERE iduser = {Request.Cookies["userCookie"]}");
                    ViewBag.Rates = DbConnector.Query($"SELECT * FROM rate WHERE userId = {Request.Cookies["userCookie"]}");
                }
                catch(Exception e)
                {
                    Debug.Print("Exception when querying for user: " + e.Message );
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        [Route("Rates")]
        public IActionResult Rates()
        {
            ViewBag.Logout = "Logout";
            return View();
        }
        [HttpPost]
        [Route("Rates/CalculateRates")]
        public IActionResult CalculateRates(Request model)
        {
            HttpContext.Session.Clear();
            if(ModelState.IsValid)
            {
                //create dimensions object
                Dimensions.UnitEnum dimensionsUnit = new Dimensions.UnitEnum();
                foreach (KeyValuePair<string, Dimensions.UnitEnum> item in DimensionUnitsConfig.DimensionsUnitsList)
                {
                    if(item.Key == model.shipment.package.dimensions.unit)
                    {
                        dimensionsUnit = item.Value;
                    }
                }
                Dimensions dimensions = new Dimensions
                {
                    Length = model.shipment.package.dimensions.length,
                    Width = model.shipment.package.dimensions.width,
                    Height = model.shipment.package.dimensions.height,
                    Unit = dimensionsUnit
                };
                //create weight object
                Weight.UnitEnum weightUnit = new Weight.UnitEnum();
                foreach (KeyValuePair<string, Weight.UnitEnum> item in WeightUnitsConfig.WeightUnitsList)
                {
                    if(item.Key == model.shipment.package.weight.unit)
                    {
                        weightUnit = item.Value;
                    }
                }
                Weight weight = new Weight
                {
                    Value = model.shipment.package.weight.value,
                    Unit = weightUnit
                };
                //create package list
                List<ShipmentPackage> packageList = new List<ShipmentPackage>();
                //create package and push to package list
                ShipmentPackage package = new ShipmentPackage
                {
                    Weight = weight,
                    Dimensions = dimensions
                };
                packageList.Add(package);
                // create ship to address
                AddressDTO ship_to = new AddressDTO
                {
                    Name = model.shipment.ship_to.name,
                    Phone = model.shipment.ship_to.phone,
                    PostalCode = model.shipment.ship_to.postal_code,
                    StateProvince = model.shipment.ship_to.state_province,
                    CityLocality = model.shipment.ship_to.city_locality,
                    AddressLine1 = model.shipment.ship_to.address_line1,
                    AddressLine2 = model.shipment.ship_to.address_line2,
                    CompanyName = model.shipment.ship_to.company_name,
                    CountryCode = model.shipment.ship_to.country_code
                };
                // create ship from address
                AddressDTO ship_from = new AddressDTO
                {
                    Name = model.shipment.ship_from.name,
                    Phone = model.shipment.ship_from.phone,
                    PostalCode = model.shipment.ship_from.postal_code,
                    StateProvince = model.shipment.ship_from.state_province,
                    CityLocality = model.shipment.ship_from.city_locality,
                    AddressLine1 = model.shipment.ship_from.address_line1,
                    AddressLine2 = model.shipment.ship_from.address_line2,
                    CompanyName = model.shipment.ship_from.company_name,
                    CountryCode = model.shipment.ship_from.country_code
                };
                // create shipment
                AddressValidatingShipment shipment = new AddressValidatingShipment
                {
                    Packages = packageList,
                    ShipFrom = ship_from,
                    ShipTo = ship_to
                };
                // create rateoptions
                var apiKey = "2zWOj6zfmwc98DLwos4B9U5Jg9wOxkAjlX20vgEYtDs";  
                var carrierApi = new CarriersApi();
                List<string> carrierIds = new List<string>();
                try
                {
                    List<Carrier> carrierList = carrierApi.CarriersList(apiKey).Carriers;
                    foreach(Carrier carrier in carrierList)
                    {
                        carrierIds.Add(carrier.CarrierId);
                    }
                }
                catch (Exception e)
                {
                    Debug.Print("Exception when calling CarriersApi.CarriersList: " + e.Message );
                }
                RateRequest rateOptions = new RateRequest
                {
                    CarrierIds = carrierIds
                };
                // create request
                RateShipmentRequest request = new RateShipmentRequest
                {
                    Shipment = shipment,
                    RateOptions = rateOptions
                };
                //send rate results to action through tempdata
                var rateApi = new RatesApi();
                try
                {
                    RateShipmentResponse result = rateApi.RatesRateShipment(request, apiKey);
                    RateResponse rates = result.RateResponse;
                    HttpContext.Session.SetString("rates", JsonConvert.SerializeObject(rates));
                    return RedirectToAction("RateResults");
                }
                catch (Exception e)
                {
                    Debug.Print("Exception when calling RatesApi.RatesRateShipment: " + e.Message );
                }

                
            }
            return RedirectToAction("Rates");
        }
        [HttpGet]
        [Route("Rates/Results")]
        public IActionResult RateResults()
        {
            ViewBag.Logout = "Logout";
            string serializedRates = HttpContext.Session.GetString("rates");
            RateResponse rateResults = JsonConvert.DeserializeObject<RateResponse>(serializedRates);
            ViewBag.Rates = from r in rateResults.Rates orderby r.DeliveryDays select r;
            return View();
        }
        [HttpPost]
        [Route("Rates/SaveRate")]
        public IActionResult SaveRate(Dictionary<string, string> data)
        {
            if(data != null)
            {
                string serializedRates = HttpContext.Session.GetString("rates");
                RateResponse rateResponse = JsonConvert.DeserializeObject<RateResponse>(serializedRates);
                foreach(Rate rate in rateResponse.Rates)
                {
                    if(rate.RateId == data["rateId"])
                    {
                        DbConnector.Execute($"INSERT INTO rate(rateRequestId, shipmentId, rateId, userId, carrierId, shippingAmount, deliveryDays, serviceType, createdAt, updatedAt) VALUES('{rateResponse.RateRequestId}', '{rateResponse.ShipmentId}', '{rate.RateId}', '{Request.Cookies["userCookie"]}', '{rate.CarrierId}', '{rate.ShippingAmount.Amount}', '{rate.DeliveryDays}', '{rate.ServiceType}', '{DateTime.Now.ToString()}', '{DateTime.Now.ToString()}')");
                        return RedirectToAction("RateResults");
                    }
                }
            }
            return RedirectToAction("RateResults");
        }
        [HttpPost]
        [Route("Rates/RemoveRate")]
        public IActionResult RemoveRate(Dictionary<string, string> data)
        {
            if(data != null)
            {
                try
                {
                    DbConnector.Execute($"DELETE FROM rate WHERE rateId = '{data["rateId"]}'");
                    if(data["page"] == "home")
                    {
                        return RedirectToAction("Home");
                    }
                    return RedirectToAction("RateResults");
                }
                catch(Exception e)
                {
                    Debug.Print("Exception when deleting rate:" + e.Message);
                };
            }
            return RedirectToAction("RateResults");
        }
        [HttpPost]
        [Route("Label")]
        public IActionResult Label(Dictionary<string, string> data)
        {
            var apiKey = "2zWOj6zfmwc98DLwos4B9U5Jg9wOxkAjlX20vgEYtDs";
            var labelApi = new LabelsApi();
            var labelId = "se-202887313";
            try
            {
                Label result = labelApi.LabelsGet(labelId, apiKey);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling LabelsApi.LabelsPurchaseLabelWithShipment: " + e.Message );
            }
            return RedirectToAction("Home");
        }
    }
}
