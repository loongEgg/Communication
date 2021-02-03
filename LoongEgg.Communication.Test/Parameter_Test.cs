using LoongEgg.Communication.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LoongEgg.Communication.Test
{
    [TestClass]
    public class Parameter_Test
    {
        ParameterConfig stubConfig = new ParameterConfig(
            "stubName",
            Data.SourceTypes.Int16,
            Data.TargetTypes.Float,
            2,
            15,
            0.1);

        [TestMethod]
        public void CanDecodeFrom_Bytes()
        {
            Parameter stubParameter = new Parameter(stubConfig);

            var bytes = BitConverter.GetBytes((UInt16)1234);
            var buffer = new byte[5];

            Assert.AreEqual(2, bytes.Length);
            Assert.AreEqual(2, stubParameter.Raw.Length);

            buffer[stubConfig.Offset] = bytes[0];
            buffer[stubConfig.Offset + 1] = bytes[1];

            Assert.IsTrue(stubParameter.TryDecodeFromPacket(buffer));
            for (int i = 0; i < bytes.Length; i++)
            {
                Assert.AreEqual(bytes[i], stubParameter.Raw[i]);
            }

            Assert.AreEqual(0.1, stubConfig.Scalar);

            Assert.AreEqual("123.4", stubParameter.Value.ToString());
        }
         
    }
}
