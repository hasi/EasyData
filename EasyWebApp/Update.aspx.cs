using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NUnit.Framework;
using EasyData;
using EasyWebApp.Model;

namespace EasyWebApp
{
    public partial class Update : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SimpleUpdateTest();
            ManyToManyUpdateTest();
            //SimpleUpdateByPropertyTest();
        }

        [Test]
        public void SimpleUpdateTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Update] - without relationship
                ProductCategory productCategory = new ProductCategory();

                int resultCount = productCategory.Count(easySession);
                Assert.AreEqual(5, resultCount);

                productCategory.ProductCategoryID = 5;
                productCategory.Name = "Test Category Update";
                //productCategory.Rowguid = Guid.NewGuid();
                productCategory.ModifiedDate = DateTime.Now;
                bool result = productCategory.Update(easySession);

                resultCount = productCategory.Count(easySession);
                Assert.AreEqual(5, resultCount);
            }
        }

        [Test]
        public void SimpleUpdateByPropertyTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Update] - without relationship
                ProductCategory productCategory = new ProductCategory();

                int resultCount = productCategory.Count(easySession);
                Assert.AreEqual(4, resultCount);

                productCategory.ProductCategoryID = 4;
                productCategory.Name = "Test Category Update";
                //productCategory.Rowguid = Guid.NewGuid();
                productCategory.ModifiedDate = DateTime.Now;
                bool result = productCategory.Update(easySession, "Name", "Bikes");

                resultCount = productCategory.Count(easySession);
                Assert.AreEqual(4, resultCount);
            }
        }

        [Test]
        public void ManyToManyUpdateTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Update] - many-to-many relationship
                ProductModelIllustration productModelIllustration = new ProductModelIllustration();
                int resultCount = productModelIllustration.Count(easySession);
                Assert.AreEqual(5, resultCount);

                ProductModel productModel = new ProductModel();
                resultCount = productModel.Count(easySession);
                Assert.AreEqual(129, resultCount);

                productModel.ProductModelID = 47;
                ProductModel resultList = productModel.Find(easySession, EasyLoad.Simple);

                //resultList.Illustration.RemoveAt(0);
                bool result = resultList.Update(easySession);

                resultCount = resultList.Count(easySession);
                Assert.AreEqual(129, resultCount);

                resultCount = productModelIllustration.Count(easySession);
                Assert.AreEqual(5, resultCount);
            }
        }
    }
}
