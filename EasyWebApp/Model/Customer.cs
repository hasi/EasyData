namespace EasyWebApp.Model
{
    using System;
    using System.Collections;
    using EasyData.Attributes;
    using EasyData.Core;

    [EasyData("Customer", Schema = "Sales")]
    public class Customer : EasyDataBase<Customer>
    {
        private int _customerID;
        private int _territoryID;
        private string _accountNumber;
        private string _customerType;
        private Guid _rowguid;
        private DateTime _modifiedDate;
        private Individual _individual;

        public Customer()
        {
            _individual = new Individual();
        }

        [PrimaryKey(PrimaryKeyType.Native)]
        public int CustomerID
        {
            get { return _customerID; }
            set { _customerID = value; }
        }

        [Property]
        public int TerritoryID
        {
            get { return _territoryID; }
            set { _territoryID = value; }
        }

        [Property]
        public string AccountNumber
        {
            get { return _accountNumber; }
            set { _accountNumber = value; }
        }

        [Property]
        public string CustomerType
        {
            get { return _customerType; }
            set { _customerType = value; }
        }

        [Property]
        public Guid Rowguid
        {
            get { return _rowguid; }
            set { _rowguid = value; }
        }

        [Property]
        public DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }

        [OneToOne]
        public Individual Individual
        {
            get { return _individual; }
            set { _individual = value; }
        }
    }
}