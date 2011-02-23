namespace EasyWebApp.Model
{
    using System;
    using System.Collections;
    using EasyData.Attributes;
    using EasyData.Core;

    [EasyData("ProductProductPhotos")]
    public class ProductProductPhoto : EasyDataBase<ProductProductPhoto>
    {
        private int _productID;
        private int _productPhotoID;
        private Boolean _primary;
        private DateTime _modifiedDate;

        public ProductProductPhoto()
        {
        }

        [PrimaryKey(PrimaryKeyType.Native)]
        public int ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }

        [Property]
        public int ProductPhotoID
        {
            get { return _productPhotoID; }
            set { _productPhotoID = value; }
        }

        [Property]
        public Boolean Primary
        {
            get { return _primary; }
            set { _primary = value; }
        }

        [Property]
        public DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }
    }
}
