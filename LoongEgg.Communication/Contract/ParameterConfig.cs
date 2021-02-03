using LoongEgg.Communication.Data;
using LoongEgg.SharpExtensions;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LoongEgg.Communication.Contract
{
    [XmlRoot("Parameter")]
    /// <summary>
    /// 参数定义
    /// </summary>
    public class ParameterConfig
    {
        /// <summary>
        /// 参数变量名, 用于自动化生成参数列表, 应该与模型的变量名一致, 且为英文字符不含空格等特殊字符
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// 标签, 用于在UI中显示的名称
        /// </summary>
        [XmlAttribute]
        public string Label { get; set; }

        /// <summary>
        /// 原始数据类型
        /// </summary>
        [XmlAttribute]
        public SourceTypes Tsource
        {
            get { return _Tsource; }
            set
            {
                _Tsource = value;
                switch (Tsource)
                {
                    case SourceTypes.UInt8: _Length = 1; break;

                    case SourceTypes.UInt16: _Length = 2; break;

                    case SourceTypes.UInt32: _Length = 4; break;

                    case SourceTypes.Int8: _Length = 1; break;

                    case SourceTypes.Int16: _Length = 2; break;

                    case SourceTypes.Int32: _Length = 4; break;

                    case SourceTypes.Float: _Length = 2; break;

                    case SourceTypes.Double: _Length = 4; break;

                    case SourceTypes.String: Ttarget = TargetTypes.String; break;

                    default:
                        break;
                }
            }
        }
        private SourceTypes _Tsource;

        /// <summary>
        /// 目标数据类型
        /// </summary>
        [XmlAttribute]
        public TargetTypes Ttarget
        {
            get { return _Ttarget; }
            set
            {
                if (Tsource == SourceTypes.String)
                    _Ttarget = TargetTypes.String;
                else
                    _Ttarget = value;
            }
        }
        private TargetTypes _Ttarget;

        /// <summary>
        /// 起始字节位置
        /// </summary>
        [XmlAttribute]
        public byte Offset { get; set; }

        /// <summary>
        /// 字节长度, 仅在<see cref="Tsource"/>为string时有用
        /// </summary>
        [XmlAttribute]
        public byte Length
        {
            get { return _Length; }
            set
            {
                if (_Length == 0 || Tsource == SourceTypes.String)
                    _Length = value;
            }
        }
        private byte _Length = 0;

        /// <summary>
        /// 单位
        /// </summary>
        [XmlAttribute]
        public string Unit { get; set; }

        /// <summary>
        /// 缩放比例, 为<see cref="TargetTypes.Flag"/>, <see cref="TargetTypes.String"/>和<see cref="TargetTypes.Enum"/>时没有意义
        /// </summary>
        [XmlAttribute]
        public double Scalar { get; set; }

        /// <summary>
        /// flag反逻辑标志, 当为true时: 即指定位为0, 该位flag判断为真.
        /// [default] = false
        /// </summary>
        [XmlAttribute]
        public bool IsRevFlag { get; set; } = false;

        /// <summary>
        /// 参数含义的详细描述, 如果目标类型为<see cref="TargetTypes.Flag"/>或<see cref="TargetTypes.Enum"/>,
        /// 会被解释为<see cref="Flags"/>或<see cref="Enums"/>
        /// </summary>
        [XmlAttribute]
        public string Detail
        {
            get { return _Detail; }
            set
            {
                _Detail = value;
                if (Detail.Length > 1)
                {
                    if (Ttarget == TargetTypes.Flag)
                    {
                        Flags = Detail.ToFlagDictonary();
                    }
                    else if (Ttarget == TargetTypes.Enum)
                    {
                        Enums = Detail.ToEumDictonary();
                    }
                }
            }
        }
        private string _Detail;

        /// <summary>
        /// flag字典, 注意key为字节的位置而不是值, 比如[example]中的1代表参数的bits[1]代表flag1的状态
        /// </summary>
        /// <example>
        /// "0:flag0; 1:flag1"
        /// </example>
        [XmlIgnore]
        public Dictionary<UInt32, string> Flags { get; protected set; }

        /// <summary>
        /// flag字典, 注意key为值
        /// </summary>
        /// <example>
        /// "0:state0; 1:state1"
        /// </example>
        [XmlIgnore]
        public Dictionary<UInt32, string> Enums { get; protected set; }

        public ParameterConfig() : this("Undefined", SourceTypes.UInt8, TargetTypes.UInt8, 0, 1) { }

        /// <summary>
        /// instance of <see cref="ParameterConfig"/>
        /// </summary>
        /// <param name="name"><see cref="Name"/></param>
        /// <param name="tsource"><see cref="Tsource"/></param>
        /// <param name="ttarget"><see cref="Ttarget"/></param>
        /// <param name="offset"><see cref="Offset"/></param>
        /// <param name="length"><see cref="Length"/></param>
        /// <param name="scalar"><see cref="Scalar"/></param>
        /// <param name="unit"><see cref="Unit"/></param>
        /// <param name="label"><see cref="Label"/></param>
        /// <param name="detail"><see cref="Detail"/></param>
        /// <param name="isRevFlag"><see cref="IsRevFlag"/></param>
        public ParameterConfig(
            string name, SourceTypes tsource, TargetTypes ttarget,
            byte offset, byte length, double scalar = 1,
            string unit = "-", string label = "", string detail = "", bool isRevFlag = false)
        {
            Name = name;

            Tsource = tsource;
            Ttarget = ttarget;

            Offset = offset;
            Length = length;
            Scalar = scalar;

            Label = label;
            Unit = unit;
            Detail = detail;

            IsRevFlag = isRevFlag;
        }

    }
}
