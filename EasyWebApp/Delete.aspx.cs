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
    public partial class Delete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SimpleDeleteTest();
            //SimpleDeleteUsingWhereTest();
            //SimpleDeleteAllTest();
        }

        [Test]
        public void SimpleDeleteTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Create] - without relationship
                Location location = new Location();

                int resultCount = location.Count(easySession);
                Assert.AreEqual(14, resultCount);

                location.LocationID = 5;
                bool result = location.Delete(easySession);

                resultCount = location.Count(easySession);
                Assert.AreEqual(13, resultCount);
            }
        }

        [Test]
        public void SimpleDeleteUsingWhereTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Create] - without relationship
                Location location = new Location();

                int resultCount = location.Count(easySession);
                Assert.AreEqual(13, resultCount);

                bool result = location.DeleteAllByCustomWhere(easySession, "Availability = 120.00");

                resultCount = location.Count(easySession);
                Assert.AreEqual(9, resultCount);
            }
        }

        [Test]
        public void SimpleDeleteAllTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Create] - without relationship
                Location location = new Location();

                int resultCount = location.Count(easySession);
                Assert.AreEqual(9, resultCount);

                bool result = location.DeleteAll(easySession);

                resultCount = location.Count(easySession);
                Assert.AreEqual(0, resultCount);
            }
        }
    }
}
