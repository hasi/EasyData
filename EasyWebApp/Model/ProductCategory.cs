namespace EasyWebApp.Model
{
    using System;
    using System.Collections;
    using EasyData.Attributes;
    using EasyData.Core;

    [EasyData("ProductCategory", Schema="Production")]
    public class ProductCategory : EasyDataBase<ProductCategory>
    {
        private int _productCategoryID;
        private String _name;
        private Guid _rowguid;
        private DateTime _modifiedDate;
        private IList _productSubcategories;

        public ProductCategory()
        {
        }

        [PrimaryKey(PrimaryKeyType.Native)]
        public int ProductCategoryID
        {
            get { return _productCategoryID; }
            set { _productCategoryID = value; }
        }

        [Property]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
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

        [HasMany(typeof(ProductSubcategory), Table = "ProductSubcategory", ColumnKey = "ProductCategoryID" )]
        public IList ProductSubcategories
        {
            get { return _productSubcategories; }
            set { _productSubcategories = value; }
        }
    }
}
