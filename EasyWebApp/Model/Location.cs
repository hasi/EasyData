namespace EasyWebApp.Model
{
    using System;
    using System.Collections;
    using EasyData.Attributes;
    using EasyData.Core;

    [EasyData("Location", Schema = "Production")]
    public class Location : EasyDataBase<Location>
    {

        private int _locationID;
        private string _name;
        private decimal _costRate;
        private decimal _availability;
        private DateTime _modifiedDate;

        public Location()
        {
        }

        [PrimaryKey(PrimaryKeyType.Native)]
        public int LocationID
        {
            get { return _locationID; }
            set { _locationID = value; }
        }

        [Property]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [Property]
        public decimal CostRate
        {
            get { return _costRate; }
            set { _costRate = value; }
        }

        [Property]
        public decimal Availability
        {
            get { return _availability; }
            set { _availability = value; }
        }

        [Property]
        public DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }
    }
}