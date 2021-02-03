using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoongEgg.Communication.Contract;
using LoongEgg.SharpExtensions;
using System.IO;

namespace LoongEgg.Communication.Test
{
    [TestClass]
    public class ParameterConfig_Test
    {
        ParameterConfig stubConfig;

        [TestInitialize]
        public void Init()
        {
            stubConfig = new ParameterConfig
            {
                Name = "testName",
                Label = "testLabel",
                Tsource = Data.SourceTypes.Int32,
                Ttarget = Data.TargetTypes.Flag,
                Offset = 2,
                Length = 5,
                Detail = "0:bit0; 1:bit1; 11:bit11"
            };
        }

        [TestMethod]
        public void Length_CanNotBeSet_ButStringType()
        {
            Assert.AreEqual(4, stubConfig.Length);

            stubConfig.Length = 5;
            Assert.AreEqual(4, stubConfig.Length);

            var stringConfig = new ParameterConfig
            {
                Name = "testName",
                Label = "testLabel",
                Tsource = Data.SourceTypes.String,
                Ttarget = Data.TargetTypes.String,
                Offset = 2,
                Length = 5,
                Detail = "0:bit0; 1:bit1; 11:bit11"
            };

            Assert.AreEqual(5, stringConfig.Length);

            stringConfig.Length = 9;
            Assert.AreEqual(9, stringConfig.Length);

        }

        [TestMethod]
        public void Serialize_Deserialize_Check()
        {
            string path = "ParameterConfig_Test.xml".ToLower();
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            path.CreatAxXmlFile(stubConfig);

            Assert.IsTrue(File.Exists(path));
            
            var actual = path.ReadAsXmlFileToObject<ParameterConfig>();

            Assert.IsNotNull(actual);

            Assert.AreEqual(stubConfig.Name, actual.Name);
            Assert.AreEqual(stubConfig.Label, actual.Label);
            Assert.AreEqual(stubConfig.Tsource, actual.Tsource);
            Assert.AreEqual(stubConfig.Ttarget, actual.Ttarget);
            Assert.AreEqual(stubConfig.Offset, actual.Offset);
            Assert.AreEqual(4, actual.Length);
        }
         
    }
}
