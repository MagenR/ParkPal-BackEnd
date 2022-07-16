using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ParkPal_BackEnd.Models.DAL;

namespace ParkPal_BackEnd.Models
{
    public class DataBaseObject
    {

        //--------------------------------------------------------------------------------------------------
        // Methods 
        //--------------------------------------------------------------------------------------------------

        // Inserts itself to the database.
        public virtual int Insert()
        {
            return DataServices.Insert(this);
        }

        // Updates it's info
        public virtual int Update()
        {
            return DataServices.Update(this);
        }

    }

}