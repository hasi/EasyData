namespace EasyWebApp.Model
{
    using System;
    using System.Collections;
    using EasyData.Attributes;
    using EasyData.Core;

    [EasyData("ProductPhotos")]
    public class ProductPhoto : EasyDataBase<ProductPhoto>
    {
        private int _productPhotoID;
        private String _thumbnailPhoto;
        private String _thumbnailPhotoFileName;
        private String _largePhoto;
        private String _largePhotoFileName;
        private DateTime _modifiedDate;

        public ProductPhoto()
        {
        }

        [PrimaryKey(PrimaryKeyType.Native)]
        public int ProductPhotoID
        {
            get { return _productPhotoID; }
            set { _productPhotoID = value; }
        }

        [Property]
        public String ThumbnailPhoto
        {
            get { return _thumbnailPhoto; }
            set { _thumbnailPhoto = value; }
        }

        [Property]
        public String ThumbnailPhotoFileName
        {
            get { return _thumbnailPhotoFileName; }
            set { _thumbnailPhotoFileName = value; }
        }

        [Property]
        public String LargePhoto
        {
            get { return _largePhoto; }
            set { _largePhoto = value; }
        }

        [Property]
        public String LargePhotoFileName
        {
            get { return _largePhotoFileName; }
            set { _largePhotoFileName = value; }
        }

        [Property]
        public DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }
    }
}
