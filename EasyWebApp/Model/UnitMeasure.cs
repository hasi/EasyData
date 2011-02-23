namespace EasyWebApp.Model
{
    using System;
    using System.Collections;
    using EasyData.Attributes;
    using EasyData.Core;

    [EasyData("UnitMeasures")]
    public class UnitMeasure : EasyDataBase<UnitMeasure>
    {
        private int _unitMeasureCode;
        private String _name;
        private DateTime _modifiedDate;

        public UnitMeasure()
        {
        }

        [PrimaryKey(PrimaryKeyType.Native)]
        public int UnitMeasureCode
        {
            get { return _unitMeasureCode; }
            set { _unitMeasureCode = value; }
        }

        [Property]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [Property]
        public DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }
    }
}
