namespace EasyWebApp.Model
{
    using System;
	using System.Collections;
    using EasyData.Attributes;
    using EasyData.Core;

    [EasyData("ProductModelIllustration", Schema = "Production")]
    public class ProductModelIllustration : EasyDataBase<ProductModelIllustration>
    {
        private int _productModelID;
        private int _illustrationID;
        private DateTime _modifiedDate;

        public ProductModelIllustration()
        {
        }

        [KeyProperty]
        public int ProductModelID
        {
            get { return _productModelID; }
            set { _productModelID = value; }
        }

        [KeyProperty]
        public int IllustrationID
        {
            get { return _illustrationID; }
            set { _illustrationID = value; }
        }

        [Property]
        public DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }
    }
}
