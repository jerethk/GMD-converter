using GMD_converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMe()
        {
            var input = new byte[]
            {
                08,
            };

            var result = MidiEvent.GetLength(input);
            Console.WriteLine(result);

            Assert.AreEqual(0, 1);
        }
    }
}
