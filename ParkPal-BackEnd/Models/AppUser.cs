using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ParkPal_BackEnd.Models.DAL;

namespace ParkPal_BackEnd.Models
{
    public class AppUser : DataBaseObject
    {
        //--------------------------------------------------------------------------------------------------
        // Fields
        //--------------------------------------------------------------------------------------------------

        int id;
        string userName, email, password, firstName, lastName;
        List<ParkingArrangement> parkingSchedule;

        //--------------------------------------------------------------------------------------------------
        // Props
        //--------------------------------------------------------------------------------------------------

        public int Id { get => id; set => id = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public List<ParkingArrangement> ParkingSchedule { get => parkingSchedule; set => parkingSchedule = value; }

        //--------------------------------------------------------------------------------------------------
        // Constructors
        //--------------------------------------------------------------------------------------------------

        // Full constructor
        public AppUser(int id, string userName, string email, string password, string firstName, string lastName, List<ParkingArrangement> parkingSchedule) : this(id, userName, email, password, firstName, lastName)
        {
            ParkingSchedule = parkingSchedule;
        }

        // For Get
        public AppUser(int id, string userName, string email, string password, string firstName, string lastName) : this(userName, email, password, firstName, lastName)
        {
            Id = id;
        }

        // For Get - Login (no password returned)
        public AppUser(int id, string userName, string email, string firstName, string lastName) : this(userName, email, null, firstName, lastName)
        {
            Id = id;
        }

        // For Post
        public AppUser(string userName, string email, string password, string firstName, string lastName)
        {
            UserName = userName;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
        }

        // For Auction demo
        public AppUser(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }


        public AppUser() {}

        //--------------------------------------------------------------------------------------------------
        // Methods 
        //--------------------------------------------------------------------------------------------------

        // Returns the user, given email and password.
        public static AppUser Get(string login, DataServices.LoginType type, string password = null)
        {
            return DataServices.GetUser(login, type, password);
        }

    }
}