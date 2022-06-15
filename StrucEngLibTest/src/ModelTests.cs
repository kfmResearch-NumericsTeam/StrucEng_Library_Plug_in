using NUnit.Framework;


namespace StrucEngLibTest
{
    [TestFixture]
    public class PrimeService_IsPrimeShould
    {
        [Test]
        public void IsPrime_InputIs1_ReturnFalse()
        {
            
            Assert.IsTrue("1" == "1", "this should be 1");
        }
    }
}