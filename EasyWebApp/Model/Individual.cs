namespace EasyWebApp.Model
{
    using System;
    using System.Collections;
    using EasyData.Attributes;
    using EasyData.Core;

    [EasyData("Individual", Schema = "Sales")]
    public class Individual : EasyDataBase<Individual>
    {
        private Customer _customer;
        private int _customerID;
        private int _contactID;
        private string _demographics;
        private DateTime _modifiedDate;
        

        public Individual()
        {
        }

        public Individual(Customer customer)
		{
            this._customer = customer;
		}

		[OneToOne(Constrained=true)]
		public Customer Customer
		{
			get { return _customer; }
			set { _customer = value; }
		}

        [PrimaryKey(PrimaryKeyType.Foreign, "CustomerID")]
        public int CustomerID
        {
            get { return _customerID; }
            set { _customerID = value; }
        }

        [Property]
        public int ContactID
        {
            get { return _contactID; }
            set { _contactID = value; }
        }

        [Property(ColumnType = "Xml", NotNull = true)]
        public string Demographics
        {
            get { return _demographics; }
            set { _demographics = value; }
        }

        [Property]
        public DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }
    }
}