using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ParkPal_BackEnd.Models.DAL;

namespace ParkPal_BackEnd.Models
{
    public class ParkingLot : DataBaseObject
    {
        //--------------------------------------------------------------------------------------------------
        // Fields
        //--------------------------------------------------------------------------------------------------
        public const int searchRadius = 3000; // search within this radius.

        int id, hourlyTariff, numOfSpaces;
        // int numOfOccupiedSlots
        // list<int> occupiedSlots;
        double distanceFromPosition, latitude, longitude;
        string name, address, type;
        List<ParkingSpot> spots;

        //--------------------------------------------------------------------------------------------------
        // Props
        //--------------------------------------------------------------------------------------------------

        public int Id { get => id; set => id = value; }
        public int HourlyTariff { get => hourlyTariff; set => hourlyTariff = value; }
        public int NumOfSpaces { get => numOfSpaces; set => numOfSpaces = value; }
        public double DistanceFromPosition { get => distanceFromPosition; set => distanceFromPosition = value; }
        public string Name { get => name; set => name = value; }
        public string Address { get => address; set => address = value; }
        public List<ParkingSpot> Spots { get => spots; set => spots = value; }
        public double Latitude { get => latitude; set => latitude = value; }
        public double Longitude { get => longitude; set => longitude = value; }
        public string Type { get => type; set => type = value; }

        //--------------------------------------------------------------------------------------------------
        // Constructors
        //--------------------------------------------------------------------------------------------------

        // Full constructor
        public ParkingLot(int id, string name, string address, int hourlyTariff, int numOfSpaces, double latitude, double longitude, List<ParkingSpot> spots, string type) : this(id, name, address, hourlyTariff, numOfSpaces, latitude, longitude, type)
        {
            Spots = spots;
        }

        // For Get
        public ParkingLot(int id, string name, string address, int hourlyTariff, int numOfSpaces, double latitude, double longitude, string type) : this(name, address, hourlyTariff, numOfSpaces, latitude, longitude, type)
        {
            Id = id;
        }

        // For Post
        public ParkingLot(string name, string address, int hourlyTariff, int numOfSpaces, double latitude, double longitude, string type)
        {
            HourlyTariff = hourlyTariff;
            NumOfSpaces = numOfSpaces;
            Name = name;
            Address = address;
            Latitude = latitude;
            Longitude = longitude;
            Type = type;
        }

        // For parking arrangements
        public ParkingLot(int id, string name, string address)
        {
            Id = id;
            Name = name;
            Address = address;
        }

        //--------------------------------------------------------------------------------------------------
        // Methods 
        //--------------------------------------------------------------------------------------------------

        // Returns a list of parking lots with atleast one vacant spot by a given time period.
        public static List<ParkingLot> Get(double latitude, double longitude, DateTime startTime, DateTime endTime)
        {
            List<ParkingLot> pls = DataServices.GetParkingLots(latitude, longitude, startTime, endTime);
            return pls;

        }

    }
}
