namespace EasyWebApp.Model
{
    using System;
    using System.Collections;
    using EasyData.Attributes;
    using EasyData.Core;

    [EasyData("Products")]
    public class Product : EasyDataBase<Product>
    {
        private int _productID;
        private String _name;
        private String _productNumber;
        private Boolean _makeFlag;
        private Boolean _finishedGoodsFlag;
        private String _color;
        private int _safetyStockLevel;
        private int _reorderPoint;
        private Decimal _standardCost;
        private Decimal _listPrice;
        private String _size;
        private String _sizeUnitMeasureCode;
        private String _weightUnitMeasureCode;
        private Decimal _weight;
        private int _daysToManufacture;
        private String _productLine;
        private String _class;
        private String _style;
        private int _productSubcategoryID;
        private int _productModelID;
        private DateTime _sellStartDate;
        private DateTime _sellEndDate;
        private DateTime _discontinuedDat;
        private Guid _rowguid;
        private DateTime _modifiedDate;

        public Product()
        {
        }

        [PrimaryKey(PrimaryKeyType.Native)]
        public int ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }

        [Property]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [Property]
        public String ProductNumber
        {
            get { return _productNumber; }
            set { _productNumber = value; }
        }

        [Property]
        public Boolean MakeFlag
        {
            get { return _makeFlag; }
            set { _makeFlag = value; }
        }

        [Property]
        public Boolean FinishedGoodsFlag
        {
            get { return _finishedGoodsFlag; }
            set { _finishedGoodsFlag = value; }
        }

        [Property]
        public String Color
        {
            get { return _color; }
            set { _color = value; }
        }

        [Property]
        public int SafetyStockLevel
        {
            get { return _safetyStockLevel; }
            set { _safetyStockLevel = value; }
        }

        [Property]
        public int ReorderPoint
        {
            get { return _reorderPoint; }
            set { _reorderPoint = value; }
        }

        [Property]
        public Decimal StandardCost
        {
            get { return _standardCost; }
            set { _standardCost = value; }
        }

        [Property]
        public Decimal ListPrice
        {
            get { return _listPrice; }
            set { _listPrice = value; }
        }

        [Property]
        public String Size
        {
            get { return _size; }
            set { _size = value; }
        }

        [Property]
        public String SizeUnitMeasureCode
        {
            get { return _sizeUnitMeasureCode; }
            set { _sizeUnitMeasureCode = value; }
        }

        [Property]
        public String WeightUnitMeasureCode
        {
            get { return _weightUnitMeasureCode; }
            set { _weightUnitMeasureCode = value; }
        }

        

        [Property]
        public Decimal Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        [Property]
        public int DaysToManufacture
        {
            get { return _daysToManufacture; }
            set { _daysToManufacture = value; }
        }

        [Property]
        public String ProductLine
        {
            get { return _productLine; }
            set { _productLine = value; }
        }

        [Property]
        public String Class
        {
            get { return _class; }
            set { _class = value; }
        }

        [Property]
        public String Style
        {
            get { return _style; }
            set { _style = value; }
        }

        [Property]
        public int ProductSubcategoryID
        {
            get { return _productSubcategoryID; }
            set { _productSubcategoryID = value; }
        }

        [Property]
        public int ProductModelID
        {
            get { return _productModelID; }
            set { _productModelID = value; }
        }

        [Property]
        public DateTime SellStartDate
        {
            get { return _sellStartDate; }
            set { _sellStartDate = value; }
        }

        [Property]
        public DateTime SellEndDate
        {
            get { return _sellEndDate; }
            set { _sellEndDate = value; }
        }

        [Property]
        public DateTime DiscontinuedDat
        {
            get { return _discontinuedDat; }
            set { _discontinuedDat = value; }
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
    }
}
