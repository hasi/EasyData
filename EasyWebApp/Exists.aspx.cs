using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EasyData;
using EasyWebApp.Model;
using NUnit.Framework;

namespace EasyWebApp
{
    public partial class Exists : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SimpleExistsTest();
            SimpleExistsByPropertyByTypeTest();
            //SimpleExistsByPropertyByInstanceTest();
        }

        [Test]
        public void SimpleExistsTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Read] - without relationship
                Location location = new Location();
                bool resultList = location.Exists(easySession, 8);

                Assert.IsTrue(resultList, "It does not exists.");
            }
        }

        //[Test]
        //public void SimpleExistsByPropertyByTypeTest()
        //{
        //    using (EasySession easySession = new EasySession())
        //    {
        //        ///[Test Read] - without relationship
        //        Location location = new Location();
        //        bool resultList = location.Exists(easySession, "Availability", 120.00);

        //        Assert.IsTrue(resultList, "It does not exists.");
        //    }
        //}

        [Test]
        public void SimpleExistsByPropertyByTypeTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Read] - without relationship
                Location location = new Location();
                bool resultList = location.Exists(easySession, "Name", "Paint");

                Assert.IsTrue(resultList, "It does not exists.");
            }
        }

        [Test]
        public void SimpleExistsByPropertyByInstanceTest()
        {
            using (EasySession easySession = new EasySession())
            {
                ///[Test Read] - without relationship
                Location location = new Location();
                location.Availability = 120.00M;
                bool resultList = location.Exists(easySession, "Availability");

                Assert.IsTrue(resultList, "It does not exists.");
            }
        }
    }
}
