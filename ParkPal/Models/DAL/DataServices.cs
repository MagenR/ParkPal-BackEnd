using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Types;
using System.Text;
using Antlr.Runtime.Misc;

namespace ParkPal_BackEnd.Models.DAL
{
    public class DataServices
    {
        //--------------------------------------------------------------------------------------------------
        // Custom Data types.
        //--------------------------------------------------------------------------------------------------
        public enum queryType { INSERT, INSERTwReturn, UPDATE, DELETE }
        public enum LoginType { Email, Username, Password }
        public enum Period { Past, Future }
        public enum IdSearchType { User, ParkingLot }

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

            if (o is Bidder)
            {
                Bidder b = (Bidder)o;
                commandText += "ParkPal_Auction_Bids values(@bidder_id, @parking_lot_id, @bid_time, @bid_limit, @for_start_time, @for_end_time)";

                cmd.CommandText = commandText;

                cmd.Parameters.Add("@bidder_id", SqlDbType.Int);
                cmd.Parameters["@bidder_id"].Value = b.Id;

                cmd.Parameters.Add("@parking_lot_id", SqlDbType.Int);
                cmd.Parameters["@parking_lot_id"].Value = b.BiddedLot.Id;

                cmd.Parameters.Add("@bid_time", SqlDbType.SmallDateTime);
                cmd.Parameters["@bid_time"].Value = b.BidTime;

                cmd.Parameters.Add("@bid_limit", SqlDbType.Int);
                cmd.Parameters["@bid_limit"].Value = b.BidLimit;

                cmd.Parameters.Add("@for_start_time", SqlDbType.SmallDateTime);
                cmd.Parameters["@for_start_time"].Value = b.ForStartTime;

                cmd.Parameters.Add("@for_end_time", SqlDbType.SmallDateTime);
                cmd.Parameters["@for_end_time"].Value = b.ForEndTime;
            }

            else if (o is AppUser)
            {
                AppUser u = (AppUser)o;
                commandText += "ParkPal_Users values(@username, @email, @password, @first_name, @last_name) ";
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
                commandText += "ParkPal_Parking_Lots values(@name, @address, @hourly_tariff, @num_of_spaces, @geo_location) ";
                cmd.CommandText = commandText;

                cmd.Parameters.AddWithValue("@name", pl.Name);
                cmd.Parameters.AddWithValue("@address", pl.Address);

                cmd.Parameters.Add("@hourly_tariff", SqlDbType.Int);
                cmd.Parameters["@hourly_tariff"].Value = pl.HourlyTariff;

                cmd.Parameters.Add("@num_of_spaces", SqlDbType.Int);
                cmd.Parameters["@num_of_spaces"].Value = pl.NumOfSpaces;

                cmd.Parameters.AddWithValue("@geo_location", SqlGeography.Point(pl.Latitude, pl.Longitude, ParkingLot.SRID));
            }

            else if (o is ParkingArrangement)
            {
                ParkingArrangement pa = (ParkingArrangement)o;
                commandText += "ParkPal_Parking_Arrangements OUTPUT INSERTED.ID values(@user_id, @parking_lot_id, @parking_spot_number, @start_time, @end_time) ";
                cmd.CommandText = commandText;

                cmd.Parameters.Add("@user_id", SqlDbType.Int);
                cmd.Parameters["@user_id"].Value = pa.Buyer.Id;

                cmd.Parameters.Add("@parking_lot_id", SqlDbType.Int);
                cmd.Parameters["@parking_lot_id"].Value = pa.ParentSpot.ParentLot.Id;

                cmd.Parameters.Add("@parking_spot_number", SqlDbType.Int);
                cmd.Parameters["@parking_spot_number"].Value = pa.ParentSpot.Number;

                cmd.Parameters.Add("@start_time", SqlDbType.SmallDateTime);
                cmd.Parameters["@start_time"].Value = pa.StartTime;

                cmd.Parameters.Add("@end_time", SqlDbType.SmallDateTime);
                cmd.Parameters["@end_time"].Value = pa.EndTime;
            }

            else if (o is Auction)
            {
                Auction a = (Auction)o;
                commandText += "ParkPal_Auction_Arrangements (arrangement_id, starting_price) values(@arrangement_id, @starting_price)";

                cmd.CommandText = commandText;

                cmd.Parameters.Add("@arrangement_id", SqlDbType.Int);
                cmd.Parameters["@arrangement_id"].Value = a.SoldArrangement.Id;

                cmd.Parameters.Add("@starting_price", SqlDbType.Int);
                cmd.Parameters["@starting_price"].Value = a.StartingPrice;
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
                               "WHERE id= '" + u.Id + "'";
                cmd.CommandText = commandText;

                cmd.Parameters.AddWithValue("@new_email", u.Email);
                cmd.Parameters.AddWithValue("@new_usr", u.UserName);
                cmd.Parameters.AddWithValue("@new_pwd", u.Password);
                cmd.Parameters.AddWithValue("@new_fname", u.FirstName);
                cmd.Parameters.AddWithValue("@new_lname", u.LastName);
            }

            if (o is Bidder)
            {
                Bidder b = (Bidder)o;
                commandText += "ParkPal_Auction_Bids set bid_limit = @bid_limit" +
                    "where bidder_id = @bidder_id";

                cmd.CommandText = commandText;

                cmd.Parameters.Add("@bidder_id", SqlDbType.Int);
                cmd.Parameters["@bidder_id"].Value = b.Id;

                cmd.Parameters.Add("@parking_lot_id", SqlDbType.Int);
                cmd.Parameters["@parking_lot_id"].Value = b.BiddedLot.Id;

                cmd.Parameters.Add("@bid_time", SqlDbType.SmallDateTime);
                cmd.Parameters["@bid_time"].Value = b.BidLimit;

                cmd.Parameters.Add("@bid_limit", SqlDbType.Int);
                cmd.Parameters["@bid_limit"].Value = b.BidLimit;
            }

            if (o is Auction)
            {
                Auction a = (Auction)o;
                commandText += "ParkPal_Auction_Arrangements set leader_id = @leader_id, current_bid = @current_bid " +
                    "where arrangement_id = @arrangement_id";

                cmd.CommandText = commandText;

                cmd.Parameters.Add("@leader_id", SqlDbType.Int);
                cmd.Parameters["@leader_id"].Value = a.HighestBidder.Id;

                cmd.Parameters.Add("@current_bid", SqlDbType.Int);
                cmd.Parameters["@current_bid"].Value = a.CurrBid;

                cmd.Parameters.Add("@arrangement_id", SqlDbType.Int);
                cmd.Parameters["@arrangement_id"].Value = a.SoldArrangement.Id;

            }

            // Update an arrangement to be an auction.
        }

        //--------------------------------------------------------------------------------------------------
        // Run an SQL querry.
        //--------------------------------------------------------------------------------------------------
        public static int ExecuteSQL(object o, queryType rtype)
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
                case queryType.INSERT:
                case queryType.INSERTwReturn:
                    BuildInsertCommand(cmd, o); // Append fields to insert command.
                    break;
                case queryType.UPDATE:
                    BuildUpdateCommand(cmd, o);
                    break;
                case queryType.DELETE: break;
                default: break;
            }

            try // execute the command.
            {
                if (rtype is queryType.INSERTwReturn) // Parking arragnement id sometimes is needed for auction registration.
                    return (int)cmd.ExecuteScalar();
                else
                    return cmd.ExecuteNonQuery();
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
            return (o is ParkingArrangement && !(o is Auction)) ? ExecuteSQL(o, queryType.INSERTwReturn) : ExecuteSQL(o, queryType.INSERT);
        }

        //--------------------------------------------------------------------------------------------------
        // Insert an object to the database.
        //--------------------------------------------------------------------------------------------------
        public static int Update(object o)
        {
            return ExecuteSQL(o, queryType.UPDATE);
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
                default: selectSTR += " (u.email = '" + login + "' or u.username = '" + login + "') and u.password = '" + password + "'"; break;
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
        public static List<ParkingLot> GetParkingLots(double latitude, double longitude, DateTime startTime, DateTime endTime)
        {
            string selectSTR =
                "SELECT pl.id, pl.name, pl.address, pl.hourly_tariff, pl.num_of_spaces, " +
                "geography::STGeomFromText(geo_location.STAsText(), 4326) as 'geo_location', " +
                "COUNT(distinct reses.id) as 'number of concurrent parkings', " +
                "COUNT(distinct ppaa.arrangement_id) as 'number of auctions', " +
                "CASE " +
                    "WHEN COUNT(distinct reses.id) = pl.num_of_spaces and COUNT(distinct ppaa.arrangement_id) > 0 then 'auctioned' " +
                    "WHEN COUNT(distinct reses.id) = pl.num_of_spaces then 'full' " +
                    "Else 'empty' " +
                "end as 'lot_type' " +
                "FROM ParkPal_Parking_Lots as pl LEFT JOIN " +
                    "(SELECT* " +
                    "FROM ParkPal_Parking_Arrangements " +
                    "WHERE @starting_time BETWEEN start_time AND end_time " +
                            "OR @ending_time BETWEEN start_time AND end_time " +
                            "OR start_time BETWEEN @starting_time AND @ending_time) " +
                    "as reses on pl.id = reses.parking_lot_id " +
                    "LEFT JOIN ParkPal_Auction_Arrangements as ppaa on reses.parking_lot_id = ppaa.arrangement_id " +
                "WHERE @origin.STDistance(geo_location) <= @distance " +
                "GROUP BY pl.id, pl.name, pl.address, pl.hourly_tariff, pl.num_of_spaces, pl.geo_location.STAsText() "; //+
                                                                                                                        //"HAVING COUNT(distinct reses.id) < pl.num_of_spaces;";

            SqlConnection con = Connect("DBConnectionString");
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            cmd.Parameters.Add("@distance", SqlDbType.Int);
            cmd.Parameters["@distance"].Value = ParkingLot.searchRadius;

            cmd.Parameters.Add("@origin", SqlDbType.Udt);
            cmd.Parameters["@origin"].UdtTypeName = "geography";
            cmd.Parameters["@origin"].Value = SqlGeography.Point(latitude, longitude, ParkingLot.SRID);

            cmd.Parameters.Add("@starting_time", SqlDbType.SmallDateTime);
            cmd.Parameters["@starting_time"].Value = startTime;

            cmd.Parameters.Add("@ending_time", SqlDbType.SmallDateTime);
            cmd.Parameters["@ending_time"].Value = endTime;

            try
            {
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr.HasRows == false)
                    return null;
                List<ParkingLot> parkingLots = new List<ParkingLot>();
                while (dr.Read())
                    parkingLots.Add(new ParkingLot(
                                (int)dr["id"],
                                (string)dr["name"],
                                (string)dr["address"],
                                (int)dr["hourly_tariff"],
                                (int)dr["num_of_spaces"],
                                (double)((SqlGeography)dr["geo_location"]).Lat,
                                (double)((SqlGeography)dr["geo_location"]).Long,
                                (string)dr["lot_type"]
                            )
                        );
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
            string selectSTR = "SELECT *, pl.name " +
                                "FROM ParkPal_Parking_Arrangements as pa " +
                                    "join ParkPal_Parking_Lots as pl on pl.id = pa.parking_lot_id " +
                                "WHERE pa.user_id = @user_id and GetDate()";
            selectSTR += (datePeriod == Period.Past) ? " > pa.end_time" : " < pa.start_time ";

            SqlConnection con = Connect("DBConnectionString");
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            cmd.Parameters.Add("@user_id", SqlDbType.Int);
            cmd.Parameters["@user_id"].Value = user_id;

            try
            {
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr.HasRows == false)
                    return null;
                List<ParkingArrangement> parkingArrangements = new List<ParkingArrangement>();
                //List<ParkingLot> parkingLots = new List<ParkingLot>();
                while (dr.Read())
                {
                    parkingArrangements.Add(new ParkingArrangement(
                        (int)dr["id"],
                        new ParkingSpot((int)dr["parking_spot_number"],
                                        new ParkingLot((int)dr["parking_lot_id"],
                                                        (string)dr["name"]
                                                       )
                                        ),
                        (DateTime)dr["start_time"],
                        (DateTime)dr["end_time"]));
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
        // Get list of parking arrangements auctions for a parking lot corresponding to requested time slot. - WIP
        //--------------------------------------------------------------------------------------------------
        public static List<Auction> GetParkingArrangementsAuctions(int parkingLotId, DateTime startTime, DateTime endTime)
        {
            string selectSTR =  "select ppaa.arrangement_id, ppaa.starting_price, ppaa.current_bid," +
                                    "ppaa.leader_id, pppa.parking_lot_id, pppa.parking_spot_number, pppa.start_time, pppa.end_time, " +
                                    "ppu.id as 'seller_id', ppu.username as 'seller_username' " +
                                "from ParkPal_Auction_Arrangements as ppaa join " +
                                    "ParkPal_Parking_Arrangements as pppa on " +
                                    "pppa.id = ppaa.arrangement_id join " +
                                    "ParkPal_Users as ppu on ppu.id = pppa.user_id " +
                                "where parking_lot_id = @parking_lot_id and " +
                                     "start_time = @starting_time and " +
                                     "end_time = @ending_time;";

            SqlConnection con = Connect("DBConnectionString");
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            cmd.Parameters.Add("@parking_lot_id", SqlDbType.Int);
            cmd.Parameters["@parking_lot_id"].Value = parkingLotId;

            cmd.Parameters.Add("@starting_time", SqlDbType.SmallDateTime);
            cmd.Parameters["@starting_time"].Value = startTime;

            cmd.Parameters.Add("@ending_time", SqlDbType.SmallDateTime);
            cmd.Parameters["@ending_time"].Value = endTime;

            try
            {
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr.HasRows == false)
                    return null;
                List<Auction> auctions = new List<Auction>();
                while (dr.Read())
                {
                    Auction auction = new Auction();

                    if ( !dr.IsDBNull( dr.GetOrdinal("leader_id") ) )
                    {
                        auction.HighestBidder = new Bidder();
                        auction.HighestBidder.Id = (int)dr["leader_id"];
                    }

                    if (dr.IsDBNull(dr.GetOrdinal("current_bid")))
                        auction.CurrBid = 0;
                    else
                        auction.CurrBid = (int)dr["current_bid"];

                    auction.SoldArrangement = new ParkingArrangement();
                    auction.SoldArrangement.Id = (int)dr["arrangement_id"];
                    auction.SoldArrangement.Buyer = new Bidder();
                    auction.SoldArrangement.Buyer.Id = (int)dr["seller_id"];
                    auction.SoldArrangement.Buyer.UserName = (string)dr["seller_username"];

                    auction.SoldArrangement.ParentSpot = new ParkingSpot();
                    auction.SoldArrangement.ParentSpot.Number = (int)dr["parking_spot_number"];
                    // auction.SoldArrangement.ParentSpot.ParentLot = new ParkingLot(); 
                    // auction.SoldArrangement.ParentSpot.ParentLot = (int)dr["parking_lot_id"];
                   
                    auction.StartingPrice = (int)dr["starting_price"];
                    
                    auctions.Add(auction);
                }
                return auctions;
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
        // Get list of bidders for a parking lot corresponding to a given time frame.
        //--------------------------------------------------------------------------------------------------
        public static List<Bidder> GetBidders(int parkingLotId, DateTime startTime, DateTime endTime)
        {
            string selectSTR =  "select ppab.*, ppu.username " +
                                "from ParkPal_Auction_Bids as ppab " +
                                "join ParkPal_Users as ppu on ppab.bidder_id=ppu.id " +
                                "where parking_lot_id = @parking_lot_id and " +
                                "for_start_time = @for_start_time and " +
                                "for_end_time = @for_end_time;";

            SqlConnection con = Connect("DBConnectionString");
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            cmd.Parameters.Add("@parking_lot_id", SqlDbType.Int);
            cmd.Parameters["@parking_lot_id"].Value = parkingLotId;

            cmd.Parameters.Add("@for_start_time", SqlDbType.SmallDateTime);
            cmd.Parameters["@for_start_time"].Value = startTime;

            cmd.Parameters.Add("@for_end_time", SqlDbType.SmallDateTime);
            cmd.Parameters["@for_end_time"].Value = endTime;

            try
            {
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr.HasRows == false)
                    return null;

                List<Bidder> bidders = new List<Bidder>();
                while (dr.Read())
                {
                    Bidder bidder = new Bidder((int)dr["bidder_id"], (string)dr["username"], (int)dr["bid_limit"], (DateTime)dr["bid_time"]);
                    bidders.Add(bidder);
                }

                return bidders;
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
        // Unused - Get list of parking arrangements for a parking lot corresponding to requested time slot.
        //--------------------------------------------------------------------------------------------------
        public static ParkingLot GetParkingArrangements(int parkingLotId, DateTime startTime, DateTime endTime)
        {
            string selectSTR = "SELECT * " +
                                "FROM ParkPal_Parking_Arrangements " +
                                "WHERE parking_lot_id = parking_lot_id " +
                                "AND @starting_time BETWEEN start_time AND end_time " +
                                    "OR @ending_time BETWEEN start_time AND end_time " +
                                    "OR start_time BETWEEN @starting_time AND @ending_time;";

            SqlConnection con = Connect("DBConnectionString");
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            cmd.Parameters.Add("@parking_lot_id", SqlDbType.Int);
            cmd.Parameters["@parking_lot_id"].Value = parkingLotId;

            cmd.Parameters.Add("@starting_time", SqlDbType.SmallDateTime);
            cmd.Parameters["@starting_time"].Value = startTime;

            cmd.Parameters.Add("@ending_time", SqlDbType.SmallDateTime);
            cmd.Parameters["@ending_time"].Value = endTime;

            try
            {
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr.HasRows == false)
                    return null;
                List<ParkingArrangement> parkingArrangements = new List<ParkingArrangement>();
                while (dr.Read()) ;
                //parkingLots.Add(new ParkingLot(
                //            (int)dr["id"],
                //            (string)dr["name"],
                //            (string)dr["address"],
                //            (int)dr["hourly_tariff"],
                //            (int)dr["num_of_spaces"],
                //            (double)((SqlGeography)dr["geo_location"]).Lat,
                //            (double)((SqlGeography)dr["geo_location"]).Long
                //        )
                //    );

                //ParkingLot pl = new ParkingLot();

                //return pl;
                return null;
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
        // Get spot number available for reservation for a parking lot corresponding to requested time slot.
        //--------------------------------------------------------------------------------------------------
        public static int GetVacantSlot(ParkingArrangement pa)
        {
            string selectSTR = "select distinct(parking_spot_number) " +
                                "from ParkPal_Parking_Arrangements " +
                                "where parking_lot_id = @parent_lot and " +
                                        "(@starting_time between start_time and end_time " +
                                        "or @ending_time between start_time and end_time " +
                                        "or start_time between @starting_time and @ending_time) " +
                                "order by parking_spot_number;";

            SqlConnection con = Connect("DBConnectionString");
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            cmd.Parameters.Add("@parent_lot", SqlDbType.Int);
            cmd.Parameters["@parent_lot"].Value = pa.ParentSpot.ParentLot.Id;

            cmd.Parameters.Add("@starting_time", SqlDbType.SmallDateTime);
            cmd.Parameters["@starting_time"].Value = pa.StartTime;

            cmd.Parameters.Add("@ending_time", SqlDbType.SmallDateTime);
            cmd.Parameters["@ending_time"].Value = pa.EndTime;

            try
            {
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr.HasRows == false)
                    return 1; // if parking lot is completely vacant - first slot is a good choice.

                List<int> nonVacantSlots = new List<int>();

                while (dr.Read()) // Build the list of non vacant slots/
                    nonVacantSlots.Add((int)dr["parking_spot_number"]);

                // if slot is in list, then it's not vacant
                for (int i = 1; i <= pa.ParentSpot.ParentLot.NumOfSpaces; i++)
                    if (!nonVacantSlots.Contains(i))
                        return i;

                return 0; // all slots are filled.
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