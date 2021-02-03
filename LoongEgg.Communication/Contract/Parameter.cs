using LoongEgg.Communication.Data;
using LoongEgg.MvvmNetX;
using LoongEgg.SharpExtensions;
using System;
using System.Text;
#if DEBUG
using System.Diagnostics;
#endif

namespace LoongEgg.Communication.Contract
{
    public class Parameter : BindableObject
    {
        /// <summary>
        /// 包定义
        /// </summary>
        public ParameterConfig Config { get; }

        /// <summary>
        /// 原始字节
        /// </summary>
        public byte[] Raw { get; private set; }

        /// <summary>
        /// 字符串形式的原始数据
        /// </summary>
        public string Source { get; protected set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            get { return _Value; }
            set { SetProperty(ref _Value, value); }
        }
        protected object _Value;

        /// <summary>
        /// 已经被绑定到特定的参数
        /// </summary>
        public bool IsBindToProperty { get; set; }

        /// <summary>
        /// 被添加到数据监控
        /// </summary>
        public bool IsMonitored
        {
            get { return _IsMonitored; }
            set { SetProperty(ref _IsMonitored, value); }
        }
        private bool _IsMonitored = false;

        public Parameter(ParameterConfig config)
        {
            Config = config;
            Raw = new byte[Config.Length];
        }

        public override string ToString() => $"{Config.Name}={Value}";

        static string GetSource(byte[] packet, SourceTypes source, byte offset, byte length)
        {
            switch (source)
            {
                case SourceTypes.UInt8: return packet[offset].ToString();
                case SourceTypes.UInt16: return BitConverter.ToUInt16(packet, offset).ToString();
                case SourceTypes.UInt32: return BitConverter.ToUInt32(packet, offset).ToString();
                case SourceTypes.Int8: return ((sbyte)packet[offset]).ToString();
                case SourceTypes.Int16: return BitConverter.ToInt16(packet, offset).ToString();
                case SourceTypes.Int32: return BitConverter.ToInt32(packet, offset).ToString();
                case SourceTypes.Float: return BitConverter.ToSingle(packet, offset).ToString();
                case SourceTypes.Double: return BitConverter.ToDouble(packet, offset).ToString();

                /* string */
                default: return BitConverter.ToString(packet, offset, length).ToString();
            }
        }

        static object GetValue(string raw, TargetTypes target, double scalar, Parameter self = null)
        {
            switch (target)
            {
                case TargetTypes.UInt8: return (byte)(byte.Parse(raw) * scalar);
                case TargetTypes.UInt16: return (UInt16)(UInt16.Parse(raw) * scalar);
                case TargetTypes.UInt32: return (UInt32)(UInt32.Parse(raw) * scalar);
                case TargetTypes.Int8: return (sbyte)(sbyte.Parse(raw) * scalar);
                case TargetTypes.Int16: return (Int16)(Int16.Parse(raw) * scalar);
                case TargetTypes.Int32: return (Int32)(Int32.Parse(raw) * scalar);
                case TargetTypes.Float: return (float)(float.Parse(raw) * scalar);
                case TargetTypes.Double: return (double)(double.Parse(raw) * scalar);

                case TargetTypes.Enum:
                    {
                        if (self != null && self.Config.Enums != null)
                        {
                            var res = UInt32.Parse(self.Source).GetEnumFrom(self.Config.Enums);
                            return res;
                        }
                        return null;
                    }

                case TargetTypes.Flag:
                    {
                        if (self != null && self.Config.Flags != null)
                        {
                            var res = self.Raw.GetFlagsFrom(self.Config.Flags, self.Config.IsRevFlag);
                            var ret = new StringBuilder("(");
                            if (res.Length >= 1)
                            {
                                for (int i = 0; i < res.Length - 1; i++)
                                {
                                    ret.Append($"{res[i]} ");
                                }
                                ret.Append(res[res.Length - 1] + ")");
                                return ret;
                            }
                        }
                        return null;
                    }

                /* string */
                default: return raw;
            }
        }

        public bool TryDecodeFromPacket(byte[] packet)
        {
            try
            {
                DecodeFromPacket(packet);
                return true;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
                Debugger.Break();
#endif
                return false;
            }
        }

        private void DecodeFromPacket(byte[] packet)
        {
            Source = GetSource(packet, Config.Tsource, Config.Offset, Config.Length);
            Array.Copy(packet, Config.Offset, Raw, 0, Config.Length);
            Value = GetValue(Source, Config.Ttarget, Config.Scalar, this);
        }
    }
}
