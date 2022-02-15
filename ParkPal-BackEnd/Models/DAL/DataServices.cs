using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;

namespace ParkPal_BackEnd.Models.DAL
{
    public class DataServices
    {
        //--------------------------------------------------------------------------------------------------
        // Custom Data types.
        //--------------------------------------------------------------------------------------------------
        public enum runType { INSERT, UPDATE, DELETE }
        public enum LoginType { Email, Username, Password }
        public enum Period { Past, Future }      

        //--------------------------------------------------------------------------------------------------
        // Create a connection to the database according to the connectionString name in the web.config.
        //--------------------------------------------------------------------------------------------------
        public static SqlConnection Connect(string conString)
        {
            // read the connection string from the configuration file
            string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        //--------------------------------------------------------------------------------------------------
        // Create the SqlCommand.
        //--------------------------------------------------------------------------------------------------
        private static SqlCommand CreateCommand(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand(); // create the command object
            cmd.Connection = con;              // assign the connection to the command object
            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
            cmd.CommandType = CommandType.Text; // the type of the command, can also be stored procedure
            return cmd;
        }

        //--------------------------------------------------------------------------------------------------
        // Append the Insert command text with values of given object to given command object.
        //--------------------------------------------------------------------------------------------------   
        private static void BuildInsertCommand(SqlCommand cmd, object o)
        {
            string commandText = "INSERT INTO ";

            if (o is AppUser)
            {
                AppUser u = (AppUser)o;
                commandText += "ParkPal_Users values(@email, @username, @password, @first_name, @last_name) ";
                cmd.CommandText = commandText;

                cmd.Parameters.AddWithValue("@email", u.Email);
                cmd.Parameters.AddWithValue("@username", u.UserName);
                cmd.Parameters.AddWithValue("@password", u.Password);
                cmd.Parameters.AddWithValue("@first_name", u.FirstName);
                cmd.Parameters.AddWithValue("@last_name", u.LastName);
            }

            else if (o is ParkingLot)
            {
                ParkingLot pl = (ParkingLot)o;
                commandText += "ParkPal_Parking_Lots values(@name, @address, @hourly_tariff, @num_of_spaces, @longitude, @latitude) ";
                cmd.CommandText = commandText;

                cmd.Parameters.AddWithValue("@name", pl.Name);
                cmd.Parameters.AddWithValue("@address", pl.Address);

                cmd.Parameters.Add("@hourly_tariff", SqlDbType.Int);
                cmd.Parameters["@hourly_tariff"].Value = pl.HourlyTariff;

                cmd.Parameters.Add("@num_of_spaces", SqlDbType.Int);
                cmd.Parameters["@num_of_spaces"].Value = pl.NumOfSpaces;

                cmd.Parameters.Add("@longitude", SqlDbType.Float);
                cmd.Parameters["@longitude"].Value = pl.Longitude;

                cmd.Parameters.Add("@latitude", SqlDbType.Float);
                cmd.Parameters["@latitude"].Value = pl.Latitude;
            }

            else if (o is ParkingArrangement)
            {
                ParkingArrangement pa = (ParkingArrangement)o;
                commandText += "ParkPal_Parking_Arrangements values(@user_id, @parking_lot_id, @start_time, @end_time) ";
                cmd.CommandText = commandText;

                cmd.Parameters.Add("@user_id", SqlDbType.Int);
                cmd.Parameters["@user_id"].Value = pa.Buyer.Id;

                cmd.Parameters.Add("@parking_lot_id", SqlDbType.Int);
                cmd.Parameters["@parking_lot_id"].Value = pa.ReservedAt.Id;

                cmd.Parameters.Add("@start_time", SqlDbType.SmallDateTime);
                cmd.Parameters["@start_time"].Value = pa.StartTime;

                cmd.Parameters.Add("@end_time", SqlDbType.SmallDateTime);
                cmd.Parameters["@end_time"].Value = pa.EndTime;
            }

        }

        //--------------------------------------------------------------------------------------------------
        // Append the Update command text with values of given object to given command object.
        //--------------------------------------------------------------------------------------------------
        private static void BuildUpdateCommand(SqlCommand cmd, object o)
        {

            string commandText = "UPDATE ";
            if (o is AppUser)
            {
                AppUser u = (AppUser)o;
                commandText += "ParkPal_Users " +
                             "SET username=@new_usr, email=@new_email, password=@new_pwd, first_name=@new_fname, last_name=@new_lname " +
                             "WHERE id=@id";
                cmd.CommandText = commandText;

                cmd.Parameters.AddWithValue("@new_email", u.Email);
                cmd.Parameters.AddWithValue("@new_usr", u.UserName);
                cmd.Parameters.AddWithValue("@new_pwd", u.Password);
                cmd.Parameters.AddWithValue("@new_fname", u.FirstName);
                cmd.Parameters.AddWithValue("@new_lname", u.LastName);
            }
        }

        //--------------------------------------------------------------------------------------------------
        // Run an SQL querry.
        //--------------------------------------------------------------------------------------------------       
        public static int ExecuteSQL(object o, runType rtype)
        {
            SqlConnection con;
            SqlCommand cmd;
            try
            {
                con = Connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                throw ex;// write to log
            }
            cmd = CreateCommand(con); // create the command
            switch (rtype)
            {
                case runType.INSERT:
                    BuildInsertCommand(cmd, o); // Append fields to insert command.
                    break;
                case runType.UPDATE:
                    BuildUpdateCommand(cmd, o);
                    break;
                case runType.DELETE: break;
                default: break;
            }
            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (SqlException sx)
            {
                throw sx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                    con.Close();// close the db connection
            }
        }

        //--------------------------------------------------------------------------------------------------
        // Insert an object to the database.
        //--------------------------------------------------------------------------------------------------
        public static int Insert(object o)
        {
            return ExecuteSQL(o, runType.INSERT);
        }

        //--------------------------------------------------------------------------------------------------
        // Insert an object to the database.
        //--------------------------------------------------------------------------------------------------
        public static int Update(object o)
        {
            return ExecuteSQL(o, runType.UPDATE);
        }

        //--------------------------------------------------------------------------------------------------
        // Get user credentials.
        //-------------------------------------------------------------------------------------------------- 
        public static AppUser GetUser(string login, LoginType type, string password = null)
        {
            string selectSTR = "SELECT * FROM ParkPal_Users as u WHERE";
            switch (type)
            {
                case LoginType.Email: selectSTR += " u.email = '" + login + "'"; break;
                case LoginType.Username: selectSTR += " u.username = '" + login + "'"; break;
                case LoginType.Password:
                default: selectSTR += " u.email = '" + login + "' or u.username = '" + login + "' and u.password = '" + password + "'"; break;
            }
            SqlConnection con = Connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            try
            {
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                if (dr.HasRows == false) // no record returned.
                    return null; // if no user found.
                dr.Read();
                return new AppUser((int)dr["id"], (string)dr["username"], (string)dr["email"], (string)dr["first_name"], (string)dr["last_name"]);
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------
        // Get list of parking lots corresponding to requested loaction.
        //--------------------------------------------------------------------------------------------------
        public static List<ParkingLot> GetParkingLots(DateTime startTime, DateTime endTime)
        {
            string selectSTR = "SELECT * " +
                               "FROM ParkPal_Parking_Lots as pl " +
                               "WHERE EXISTS (SELECT pa.parking_lot_id " +
                                            "FROM ParkPal_Parking_Arrangements as pa " +
                                            "WHERE pl.id = pa.parking_lot_id " +
                                                "and pa.start_time > " + endTime + " or pa.end_time > " + startTime +
                                            " GROUP BY pa.parking_lot_id " +
                                            "HAVING COUNT(pa.id) < pl.num_of_spaces);";

            SqlConnection con = Connect("DBConnectionString");
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            try
            {
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr.HasRows == false)
                    return null;
                List<ParkingLot> parkingLots = new List<ParkingLot>();
                while (dr.Read())
                    parkingLots.Add(new ParkingLot((int)dr["parking_lot_id"], (string)dr["name"], (string)dr["address"], (int)dr["hourly_tariff"], (int)dr["num_of_spaces"], (double)dr["longitude"], (double)dr["latitude"]));
                return parkingLots;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        //--------------------------------------------------------------------------------------------------
        // Get list of parking arrangements for a user corresponding to requested time period.
        //--------------------------------------------------------------------------------------------------
        public static List<ParkingArrangement> GetParkingArrangements(int user_id, Period datePeriod)
        {
            string selectSTR = "SELECT * FROM ParkPal_Parking_Arrangements as pa WHERE pa.user_id = '" + user_id + "' and GetDate()";
            selectSTR += (datePeriod == Period.Past) ? " > end_time" : " < start_time ";

            SqlConnection con = Connect("DBConnectionString");
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            try
            {
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr.HasRows == false)
                    return null;
                List<ParkingArrangement> parkingArrangements = new List<ParkingArrangement>();
                List<ParkingLot> parkingLots = new List<ParkingLot>();
                while (dr.Read())
                {
                    parkingArrangements.Add(new ParkingArrangement(new ParkingLot((int)dr["parking_lot_id"], (string)dr["@name"], (string)dr["@address"]), (DateTime)dr["start_time"], (DateTime)dr["end_time"]));
                }
                return parkingArrangements;

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }

        //--------------------------------------------------------------------------------------------------
        // Update an object in the database. - Update users's given parking arrangement.
        //--------------------------------------------------------------------------------------------------


        //--------------------------------------------------------------------------------------------------
        // Delete an object from the database. - Delete user's given parking arrangement.
        //--------------------------------------------------------------------------------------------------


    }
}