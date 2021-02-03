using LoongEgg.SharpExtensions;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
#if DEBUG
using System.Diagnostics;
#endif

namespace LoongEgg.Communication.Contract
{
    public class ModelConfig
    {
        public const string Example_Path = "\\Example\\ModelConfig.xml";

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public byte Stx
        {
            get { return _Stx; }
            set
            {
                _Stx = value;
                PacketConfigs.ForEach(p => p.Stx = Stx);
            }
        }
        private byte _Stx;

        [XmlAttribute]
        public string Label { get; set; }

        [XmlAttribute]
        public string Detail { get; set; }

        [XmlAttribute]
        public bool ChecksumEnabled { get; set; } = false;

      
        [XmlArray, XmlArrayItem("Path")]
        public string[] PacketConfigPaths
        {
            get { return _PacketConfigPaths; }
            set
            {
                _PacketConfigPaths = value;

                if (PacketConfigPaths != null)
                {
                    PacketConfigs.Clear();

                    PacketConfig config;
                    foreach (var path in PacketConfigPaths)
                    {
                        config = path.ReadAsXmlFileToObject<PacketConfig>();
                        config.Stx = Stx;

                        if (config != null)
                            PacketConfigs.Add(config);

#if DEBUG
                        if (config == null)
                            Debugger.Break();
#endif
                    }
                }
            }
        }
        private string[] _PacketConfigPaths;

        [XmlIgnore]
        public List<PacketConfig> PacketConfigs { get; set; } = new List<PacketConfig>();
    }
}
