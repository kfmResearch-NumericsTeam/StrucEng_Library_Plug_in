using NUnit.Framework;
using StrucEngLib.Model;

namespace StrucEngLibTest
{
    [TestFixture]
    public class ModelTest
    {
        [Test]
        public void TestModel()
        {
            var m = new Workbench();
            Assert.IsTrue(m.Version == "1", "this should be 1");
        }
    }
}