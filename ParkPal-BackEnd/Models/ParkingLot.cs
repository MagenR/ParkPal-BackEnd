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
        private const double R = 6371e3; // metres
        private const int desired_radius = 3;

        int id, hourlyTariff, numOfSpaces;
        double longitude, latitude;
        string name, address;
        List<ParkingArrangement> parkingArrangements;

        //--------------------------------------------------------------------------------------------------
        // Props
        //--------------------------------------------------------------------------------------------------

        public int Id { get => id; set => id = value; }
        public int HourlyTariff { get => hourlyTariff; set => hourlyTariff = value; }
        public int NumOfSpaces { get => numOfSpaces; set => numOfSpaces = value; }
        public double Longitude { get => longitude; set => longitude = value; }
        public double Latitude { get => latitude; set => latitude = value; }
        public string Name { get => name; set => name = value; }
        public string Address { get => address; set => address = value; }
        public List<ParkingArrangement> ParkingArrangements { get => parkingArrangements; set => parkingArrangements = value; }

        //--------------------------------------------------------------------------------------------------
        // Constructors
        //--------------------------------------------------------------------------------------------------

        // Full constructor
        public ParkingLot(int id, string name, string address, int hourlyTariff, int numOfSpaces, double longitude, double latitude, List<ParkingArrangement> parkingArrangements) : this(id, name, address, hourlyTariff, numOfSpaces, longitude, latitude)
        {
            ParkingArrangements = parkingArrangements;
        }

        // For Get
        public ParkingLot(int id, string name, string address, int hourlyTariff, int numOfSpaces, double longitude, double latitude) : this(name, address, hourlyTariff, numOfSpaces, longitude, latitude)
        {
            Id = id;
        }

        // For Post
        public ParkingLot(string name, string address, int hourlyTariff, int numOfSpaces, double longitude, double latitude)
        {
            HourlyTariff = hourlyTariff;
            NumOfSpaces = numOfSpaces;
            Longitude = longitude;
            Latitude = latitude;
            Name = name;
            Address = address;
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
        
        // Checks wether given parking lot is in desired distance from a point on map.
        private static bool isInRadius(ParkingLot pl, double longitude, double latitude)
        {
            double φ1 = latitude * Math.PI / 180; // φ, λ in radians
            double φ2 = pl.Latitude * Math.PI / 180;
            double Δφ = (pl.Latitude - latitude) * Math.PI / 180;
            double Δλ = (pl.Longitude - longitude) * Math.PI / 180;

            double a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                       Math.Cos(φ1) * Math.Cos(φ2) *
                       Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double d = (R * c)/1000; // in metres

            return  (d <= desired_radius)? true: false;
        }

        // Returns the list of parking lots corresponding to requested loaction.
        public static List<ParkingLot> Get(double longitude, double latitude, DateTime startTime, DateTime endTime)
        {
            List<ParkingLot> VacantSlot = DataServices.GetParkingLots( startTime, endTime);
            List<ParkingLot> VacantAndInRadius = new List<ParkingLot>();

            foreach(ParkingLot pl in VacantSlot)
            {
                if(isInRadius(pl, longitude, latitude))
                    VacantAndInRadius.Add(pl);
            }
            return VacantAndInRadius;
        }

    }
}