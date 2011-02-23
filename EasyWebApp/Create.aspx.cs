using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EasyWebApp.Model;
using EasyData;
using NUnit.Framework;

namespace EasyWebApp
{
    public partial class Create : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SimpleCreateTest();
            //OneToOneCreateTest();
            //ManyToOneCreateTest();
            //ManyToManyCreateTest();
        }

        [Test]
        public void SimpleCreateTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Create] - without relationship
                ProductCategory productCategory = new ProductCategory();
                
                int resultCount = productCategory.Count(easySession);
                Assert.AreEqual(4, resultCount);

                productCategory.Name = "Test Category";
                productCategory.Rowguid = Guid.NewGuid();
                productCategory.ModifiedDate = DateTime.Now;
                productCategory.Save(easySession);

                resultCount = productCategory.Count(easySession);
                Assert.AreEqual(5, resultCount);
            }
        }

        [Test]
        public void OneToOneCreateTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Create] - one-to-one relationship
                Customer customer = new Customer();

                int resultCount = customer.Count(easySession);
                Assert.AreEqual(19185, resultCount);

                customer.TerritoryID = 3;
                customer.CustomerType = "S";
                customer.Rowguid = Guid.NewGuid();
                customer.Save(easySession);

                Individual individual = new Individual(customer);

                resultCount = individual.Count(easySession);
                Assert.AreEqual(18484, resultCount);

                individual.ContactID = 2;
                individual.Save(easySession);

                resultCount = customer.Count(easySession);
                Assert.AreEqual(19186, resultCount);

                resultCount = individual.Count(easySession);
                Assert.AreEqual(18485, resultCount);
            }
        }

        [Test]
        public void ManyToOneCreateTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Create] - many-to-one relationship
                ProductSubcategory productSubcategory = new ProductSubcategory();

                int resultCount = productSubcategory.Count(easySession);
                Assert.AreEqual(37, resultCount);

                ProductCategory productCategory = new ProductCategory();
                productCategory.ProductCategoryID = 1;
                productCategory.Find(easySession);

                productSubcategory = new ProductSubcategory(productCategory, "Test Sub Category", Guid.NewGuid());
                productSubcategory.Save(easySession);

                resultCount = productSubcategory.Count(easySession);
                Assert.AreEqual(38, resultCount);
            }
        }

        [Test]
        public void ManyToManyCreateTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Create] - many-to-many relationship
                ProductModel productModel = new ProductModel();
                int resultCount = productModel.Count(easySession);
                Assert.AreEqual(128, resultCount);

                Illustration illustration = new Illustration();
                resultCount = illustration.Count(easySession);
                Assert.AreEqual(5, resultCount);

                ProductModelIllustration productModelIllustration = new ProductModelIllustration();
                resultCount = productModelIllustration.Count(easySession);
                Assert.AreEqual(7, resultCount);

                productModel.Name = "Test Model";
                productModel.Rowguid = Guid.NewGuid();
                productModel.Save(easySession);

                illustration.ProductModel.Add(productModel);
                illustration.Save(easySession);

                resultCount = productModel.Count(easySession);
                Assert.AreEqual(129, resultCount);

                resultCount = illustration.Count(easySession);
                Assert.AreEqual(6, resultCount);

                resultCount = productModelIllustration.Count(easySession);
                Assert.AreEqual(8, resultCount);
            }
        }
    }
}
