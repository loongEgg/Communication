using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LoongEgg.Communication.Contract
{
    [XmlRoot("Packet")]
    /// <summary>
    /// 包定义
    /// </summary>
    public class PacketConfig
    {

        #region Const common definition

        public const byte Stx_Index = 0;
        public const byte PldLen_Index = 1;
        public const byte Seq_Index = 2;
        public const byte SysId_Index = 3;
        public const byte CompId_Index = 4;
        public const byte MsgId_Index = 5;
        public const byte PldStart_Index = 6;

        public const byte HeaderLength = 6;
        public const byte ChecksumLength = 2; 

        #endregion

        /// <summary>
        /// 额外校验位, 用来防止不同版本之间的通讯
        /// [default] = 0
        /// </summary>
        public static byte Extra_Crc { get; set; } = 0;

        /// <summary>
        /// 使能校验
        /// [default] = false
        /// </summary>
        public static bool ChecksumEnabled { get; set; } = false;

        /// <summary>
        /// 包的名字, 用于自动化匹配包列表到具体实例, 取名应该为英文且不包含空格等特殊字符
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }
        /// <summary>
        /// 用于在UI显示的包名字
        /// </summary>
        [XmlAttribute]
        public string Label { get; set; }
        /// <summary>
        /// 更详细的描述
        /// </summary>
        [XmlAttribute]
        public string Detail { get; set; }

        #region Core property

        /// <summary>
        /// 起始帧标记
        /// </summary>
        [XmlAttribute]
        public byte Stx { get; set; }

        /// <summary>
        /// 负载长度(不包含帧头和尾部的校验码)
        /// </summary>
        [XmlAttribute]
        public byte PldLen
        {
            get { return _PayloadLength; }
            set
            {
                _PayloadLength = value;
                TotalLength = HeaderLength + PldLen + ChecksumLength;
                ChecksumStart_Index = HeaderLength + PldLen;
            }
        }
        private byte _PayloadLength;

        /// <summary>
        /// 系统ID, 飞机/地面站等等
        /// </summary>
        [XmlAttribute]
        public byte SysId { get; set; }

        /// <summary>
        /// 组件ID, 惯导/Gps等的
        /// </summary>
        [XmlAttribute]
        public byte CompId { get; set; }

        /// <summary>
        /// 消息ID
        /// </summary>
        [XmlAttribute]
        public byte MsgId { get; set; }

        [XmlArray("Parameters"), XmlArrayItem("Parameter")]
        public ParameterConfig[] ParameterConfigs { get; set; }

        #endregion

        #region Additional get only property

        /// <summary>
        /// 校验码的起始位置
        /// </summary>
        [XmlIgnore]
        public int ChecksumStart_Index { get; private set; }

        /// <summary>
        /// 一个包的总长度
        /// </summary>
        [XmlIgnore]
        public int TotalLength { get; private set; } 

        #endregion

    }
}
