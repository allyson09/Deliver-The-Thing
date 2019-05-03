using System;
using ShipEngine.ApiClient.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeliverTheThing.Models
{
    public class Request
    {
        public Shipment shipment { get; set; }
        public RateOptions rate_options { get; set; }
    }
    public class Shipment
    {
        public string validate_address { get; set; }
        public Address ship_to { get; set; }
        public Address ship_from { get; set; }
        public List<PackageToShip> packages { get; set; }
        public PackageToShip package { get; set; }

    }
    public class RateOptions
    {
        public List<string> carrier_ids { get; set; }
    }
    public class Address
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string company_name { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string city_locality { get; set; }
        public string state_province { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }

    }
    public class PackageToShip
    {
        public PackageWeight weight { get; set; }
        public PackageDimensions dimensions { get; set; }
        
    }
    public class PackageWeight
    {
        public double value { get; set; }
        public string unit { get; set; }
    }
    public class PackageDimensions
    {
        public string unit { get; set; }
        public double length { get; set; }
        public double width { get; set; }
        public double height { get; set; }
    }
    public class DimensionUnitsConfig
    {
        public static Dictionary<string, Dimensions.UnitEnum> DimensionsUnitsList = new Dictionary<string, Dimensions.UnitEnum>();
        static DimensionUnitsConfig()
        {
            DimensionsUnitsList.Add("Inch", Dimensions.UnitEnum.Inch);
            DimensionsUnitsList.Add("Centimeter", Dimensions.UnitEnum.Centimeter);
            //return;
        }
    }
    public class WeightUnitsConfig
    {
        public static Dictionary<string, Weight.UnitEnum> WeightUnitsList = new Dictionary<string, Weight.UnitEnum>();
        static WeightUnitsConfig()
        {
            WeightUnitsList.Add("Pound", Weight.UnitEnum.Pound);
            WeightUnitsList.Add("Ounce", Weight.UnitEnum.Ounce);
            WeightUnitsList.Add("Gram", Weight.UnitEnum.Gram);
            WeightUnitsList.Add("Kilogram", Weight.UnitEnum.Kilogram);
            // return;
        }
    }
}