namespace EasyWebApp.Model
{
    using System;
	using System.Collections;
    using EasyData.Attributes;
    using EasyData.Core;

    [EasyData("Illustration", Schema="Production")]
    public class Illustration : EasyDataBase<Illustration>
    {
        private int _illustrationID;
        private string _diagram;
        private DateTime _modifiedDate;
        private IList _productModel = new ArrayList();

        public Illustration()
        {
        }

        [PrimaryKey(PrimaryKeyType.Native)]
        public int IllustrationID
        {
            get { return _illustrationID; }
            set { _illustrationID = value; }
        }

        [Property(ColumnType = "Xml", NotNull = true)]
        public string Diagram
        {
            get { return _diagram; }
            set { _diagram = value; }
        }

        [Property]
        public DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }

        [HasAndBelongsToMany(typeof(ProductModel), Schema = "Production",
            Table = "ProductModelIllustration", ColumnKey = "IllustrationID", ColumnRef = "ProductModelID", Inverse = true)]
        public IList ProductModel
        {
            get { return _productModel; }
            set { _productModel = value; }
        }
    }
}
