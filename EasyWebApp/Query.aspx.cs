using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NUnit.Framework;
using EasyData.Query;
using EasyData;
using EasyWebApp.Model;

namespace EasyWebApp
{
    public partial class Query : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            QueryTest();
            //SPTest();
        }

        [Test]
        protected void QueryTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ProductModel productModel = new ProductModel();
                int resultCount = productModel.Count(easySession);
                Assert.AreEqual(129, resultCount);

                ObjectQuery testOQ = new ObjectQuery();
                //testOQ.EasyQuery(easySession).From(productModel.GetType()).Where().
                //var result = testOQ.DeleteQuery("DELETE FROM Production.ProductModel WHERE ProductModelID IN (:list)", easySession).SetParameterList("list", new int[] { 1, 2, 3 }, typeof(int)).Execute();

                resultCount = productModel.Count(easySession);
                Assert.AreEqual(126, resultCount);
            }
        }

        [Test]
        protected void SPTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ProductModel productModel = new ProductModel();
                int resultCount = productModel.Count(easySession);
                Assert.AreEqual(129, resultCount);

                ObjectQuery testOQ = new ObjectQuery();
                //testOQ.DeleteSP("uspDeleteProductModels", easySession).SetParameterList("list", new int[] { 1, 2, 3 }, typeof(int)).Execute();

                resultCount = productModel.Count(easySession);
                Assert.AreEqual(126, resultCount);
            }
        }
    }
}
