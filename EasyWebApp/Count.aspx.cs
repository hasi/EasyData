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
    public partial class Count : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SimpleCountTest();
            //SimpleCountByPropertyAndTypeTest();
            //SimpleCountByPropertyAndInstanceTest();
        }

        [Test]
        public void SimpleCountTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Read] - without relationship
                Location location = new Location();
                int resultList = location.Count(easySession);

                Assert.AreEqual(14, resultList);
            }
        }

        [Test]
        public void SimpleCountByPropertyAndTypeTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Read] - without relationship
                Location location = new Location();
                int resultList = location.Count(easySession, "Availability", 120.00);

                Assert.AreEqual(4, resultList);
            }
        }

        [Test]
        public void SimpleCountByPropertyAndInstanceTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Read] - without relationship
                Location location = new Location();
                location.Availability = 120.00M;
                int resultList = location.Count(easySession, "Availability");

                Assert.AreEqual(4, resultList);
            }
        }
    }
}
