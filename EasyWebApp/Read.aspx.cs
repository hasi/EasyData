using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NUnit.Framework;
using EasyData;
using EasyWebApp.Model;
using System.Collections;

namespace EasyWebApp
{
    public partial class Read : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SimpleReadByPropertyInstanceTest();

            //SimpleReadByPrimaryKeyTypeTest();
            //SimpleReadByPrimaryKeyInstanceTest();

            //OneToOneReadByPrimaryKeyTest();

            //SimpleReadTest();
            //OneToOneReadTest();
            //OneToManyReadTest();
            ManyToOneReadTest();
            //ManyToManyReadTest();

            //ReadByPropertyTest();
        }

        [Test]
        public void ReadByPropertyTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Read] - one-to-one relationship
                Location location = new Location();
                location.ModifiedDate = Convert.ToDateTime("6/1/1998");

                List<Location> resultList = location.FindAll(easySession, "ModifiedDate");

                Assert.IsNotNull(resultList, "Result is null");
            }
        }

        [Test]
        public void SimpleReadTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Read] - without relationship
                Location location = new Location();
                IList resultList = location.FindAll(easySession);

                Assert.AreEqual(14, resultList.Count);
            }
        }

        [Test]
        public void OneToOneReadTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Read] - one-to-one relationship
                Customer customer = new Customer();
                IList resultList = customer.FindAll(easySession);

                Assert.AreEqual(19185, resultList.Count);
            }
        }

        [Test]
        public void OneToManyReadTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Read] - one-to-many relationship
                ProductCategory productCategory = new ProductCategory();
                IList resultList = productCategory.FindAll(easySession);

                Assert.AreEqual(4, resultList.Count);
            }
        }

        [Test]
        public void ManyToOneReadTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Read] - many-to-one relationship
                ProductSubcategory productSubcategory = new ProductSubcategory();
                IList resultList = productSubcategory.FindAll(easySession);

                Assert.AreEqual(38, resultList.Count);
            }
        }

        [Test]
        public void ManyToManyReadTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Read] - many-to-many relationship
                ProductModel productModel = new ProductModel();
                //IList resultList = productModel.FindAll(easySession); //, EasyLoad.None
                List<ProductModel> resultList = productModel.FindAll(easySession, EasyLoad.Simple);
                
                //ProductModels test = new ProductModels();
                //test.d
                //ProductModels ProductList = (ProductModels)Convert.ChangeType(resultList, typeof(ProductModels));
                
                Assert.AreEqual(129, resultList.Count);
            }
        }

        [Test]
        public void SimpleReadByPrimaryKeyInstanceTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Read] - without relationship
                Location location = new Location();
                location.LocationID = 1;

                Location resultList = location.Find(easySession);

                Assert.IsNotNull(resultList, "Result is null");
            }
        }

        [Test]
        public void SimpleReadByPrimaryKeyTypeTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Read] - without relationship
                Location location = new Location();
                object id = 1;

                Location resultList = location.Find(easySession, id);

                Assert.IsNotNull(resultList, "Result is null");
            }
        }

        [Test]
        public void OneToOneReadByPrimaryKeyTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Read] - one-to-one relationship
                Customer customer = new Customer();
                customer.CustomerID = 11000;

                Customer resultList = customer.Find(easySession);

                Assert.IsNotNull(resultList, "Result is null");
            }
        }

        [Test]
        public void SimpleReadByPropertyInstanceTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Read] - without relationship
                Location location = new Location();
                location.Availability = 120.00M;

                IList resultList = location.FindAll(easySession, "Availability");
                GridView1.DataSource = resultList;
                GridView1.DataBind();

                Assert.AreEqual(4, resultList.Count);
            }
        }
    }
}
