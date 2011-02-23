namespace EasyWebApp.Model
{
    using System;
	using System.Collections;
    using EasyData.Attributes;
    using EasyData.Core;

    [EasyData("ProductSubcategory", Schema = "Production")]
    public class ProductSubcategory : EasyDataBase<ProductSubcategory>
    {
        private int _productSubcategoryID;
        private ProductCategory _productCategory;
        private string _name;
        private Guid _rowguid;
        private DateTime _modifiedDate;

        public ProductSubcategory()
        {
            _productCategory = new ProductCategory();
        }

        public ProductSubcategory(ProductCategory productCategory, string Name, Guid RowGuid)
        {
            this._productCategory = productCategory;
            this._name = Name;
            this._rowguid = RowGuid;
        }

        [PrimaryKey(PrimaryKeyType.Native)]
        public int ProductSubcategoryID
        {
            get { return _productSubcategoryID; }
            set { _productSubcategoryID = value; }
        }

        [BelongsTo("ProductCategoryID")]
        public ProductCategory ProductCategory
        {
            get { return _productCategory; }
            set { _productCategory = value; }
        }

        [Property]
        public string Name
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
    }
}
