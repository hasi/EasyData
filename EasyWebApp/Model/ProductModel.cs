namespace EasyWebApp.Model
{
    using System;
	using System.Collections;
    using EasyData.Attributes;
    using EasyData.Core;
    using System.Collections.Generic;
    using EasyData;

    [EasyData("ProductModel", Schema = "Production")]
    public class ProductModel : EasyDataBase<ProductModel>
    {

        private int _productModelID;
        private string _name;
        private string _catalogDescription;
        private string _instructions;
        private Guid _rowguid;
        private IList _illustration = new ArrayList();

        public ProductModel()
        {
            
        }

        [PrimaryKey(PrimaryKeyType.Native)]
        public int ProductModelID
        {
            get { return _productModelID; }
            set { _productModelID = value; }
        }

        [Property(PartialLoad=true)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [Property(ColumnType="Xml", NotNull=true)]
        public string CatalogDescription
        {
            get { return _catalogDescription; }
            set { _catalogDescription = value; }
        }

        [Property(ColumnType = "Xml", NotNull = true)]
        public string Instructions
        {
            get { return _instructions; }
            set { _instructions = value; }
        }

        [Property]
        public Guid Rowguid
        {
            get { return _rowguid; }
            set { _rowguid = value; }
        }

        [HasAndBelongsToMany(typeof(Illustration), Schema = "Production",
            Table = "ProductModelIllustration", ColumnKey = "ProductModelID", ColumnRef = "IllustrationID", Inverse = true)]
        public IList Illustration
        {
            get { return _illustration; }
            set { _illustration = value; }
        }

        
    }

    //public class ProductModels : List<ProductModel>
    //{
    //    //EasyDataBase<ProductModel>
    //    //public override void DeleteAll(EasySession easySession)
    //    //{
            
    //    //}
    //}
}
