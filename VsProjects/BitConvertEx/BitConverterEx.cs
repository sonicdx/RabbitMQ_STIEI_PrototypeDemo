using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitConvertEx
{
    /// <summary>
    /// 字节转换增强
    /// </summary>
    public static class BitConverterEx
    {
        #region 公开类型
        /// <summary>
        /// 对齐方式类型
        /// </summary>
        public enum EndianBitType:byte 
        {
            /// <summary>
            /// 小端对齐
            /// </summary>
            LittleEndian  = 0,
            /// <summary>
            /// 大端对齐
            /// </summary>
            BigEndian 
        }
        #endregion

        #region 工具函数
        // reverse byte order (16-bit)
        public static UInt16 ReverseBytes(UInt16 value)
        {
            return (UInt16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }

        // reverse byte order (32-bit)
        public static UInt32 ReverseBytes(UInt32 value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                   (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }

        // reverse byte order (64-bit)
        public static UInt64 ReverseBytes(UInt64 value)
        {
            return (value & 0x00000000000000FFUL) << 56 | (value & 0x000000000000FF00UL) << 40 |
                   (value & 0x0000000000FF0000UL) << 24 | (value & 0x00000000FF000000UL) << 8 |
                   (value & 0x000000FF00000000UL) >> 8 | (value & 0x0000FF0000000000UL) >> 24 |
                   (value & 0x00FF000000000000UL) >> 40 | (value & 0xFF00000000000000UL) >> 56;
        }

        private static void CheckArguments(byte[] value, int startIndex, int byteLength)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            // confirms startIndex is not negative or too far along the byte array
            if ((uint)startIndex > value.Length - byteLength)
            {
                throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region ToBytes Boolean
        public static void ToBytesWithEndianBit(this bool inValue, byte[] des, int index = 0)
        {
            CheckArguments(des, index, 1);

            if (inValue)
                des[index] = 0x01;
            else
                des[index] = 0x00;
        }

        public static byte[] GetBytesWithEndianBit(this bool inValue)
        {
            var b = new byte[1];
            ToBytesWithEndianBit(inValue, b, 0);
            return b;
        }
        #endregion

        #region ToBytes Byte
        public static void ToBytesWithEndianBit(this Byte inValue, byte[] des, int index = 0)
        {
            CheckArguments(des, index, 1);
            des[index] = inValue;
        }

        public static byte[] GetBytesWithEndianBit(this Byte inValue)
        {
            var b = new byte[1];
            ToBytesWithEndianBit(inValue, b, 0);
            return b;
        }
        #endregion

        #region ToBytes UINT16 INT16
        public static void ToBytesWithEndianBit(this UInt16 inValue, byte[] des, int index = 0, EndianBitType desType = EndianBitType.LittleEndian)
        {
            bool isNeedRev = false;
            if (BitConverter.IsLittleEndian && desType == EndianBitType.BigEndian)
            {
                isNeedRev = true;
            }
            UInt16 result = inValue;
            if (isNeedRev)
                result = ReverseBytes(result);

            des[index + 0] = (byte)(result & 0xFFU);
            des[index + 1] = (byte)((result & 0xFF00U) >> 8);
        }

        public static void ToBytesWithEndianBit(this Int16 inValue, byte[] des, int index = 0, EndianBitType desType = EndianBitType.LittleEndian)
        {
            UInt16 _convert = BytesN2Union.Create(inValue).GetValue();
            ToBytesWithEndianBit(_convert, des, index, desType);
        }

        public static byte[] GetBytesWithEndianBit(this UInt16 inValue, EndianBitType desType = EndianBitType.LittleEndian)
        {
            var b = new byte[2];
            ToBytesWithEndianBit(inValue, b, 0, desType);
            return b;
        }

        public static byte[] GetBytesWithEndianBit(this Int16 inValue, EndianBitType desType = EndianBitType.LittleEndian)
        {
            var b = new byte[2];
            ToBytesWithEndianBit(inValue, b, 0, desType);
            return b;
        }
        #endregion

        #region ToBytes UINT32 INT32
        public static void ToBytesWithEndianBit(this UInt32 inValue, byte[] des, int index = 0, EndianBitType desType = EndianBitType.LittleEndian)
        {
            bool isNeedRev = false;
            if (BitConverter.IsLittleEndian && desType == EndianBitType.BigEndian)
            {
                isNeedRev = true;
            }
            UInt32 result = inValue;
            if (isNeedRev)
                result = ReverseBytes(result);

            des[index + 0] = (byte)(result & 0xFFU);
            des[index + 1] = (byte)((result & 0xFF00U) >> 8);
            des[index + 2] = (byte)((result & 0xFF0000U) >> 16);
            des[index + 3] = (byte)((result & 0xFF000000U) >> 24);
        }

        public static void ToBytesWithEndianBit(this Int32 inValue, byte[] des, int index = 0, EndianBitType desType = EndianBitType.LittleEndian)
        {
            UInt32 _convert = BytesN4Union.Create(inValue).GetValue();
            ToBytesWithEndianBit(_convert, des, index, desType);
        }

        public static byte[] GetBytesWithEndianBit(this UInt32 inValue, EndianBitType desType = EndianBitType.LittleEndian)
        {
            var b = new byte[4];
            ToBytesWithEndianBit(inValue, b, 0, desType);
            return b;
        }

        public static byte[] GetBytesWithEndianBit(this Int32 inValue, EndianBitType desType = EndianBitType.LittleEndian)
        {
            var b = new byte[4];
            ToBytesWithEndianBit(inValue, b, 0, desType);
            return b;
        }
        #endregion

        #region ToBytes UINT64 INT64
        public static void ToBytesWithEndianBit(this UInt64 inValue, byte[] des, int index = 0, EndianBitType desType = EndianBitType.LittleEndian)
        {
            bool isNeedRev = false;
            if (BitConverter.IsLittleEndian && desType == EndianBitType.BigEndian)
            {
                isNeedRev = true;
            }
            UInt64 result = inValue;
            if (isNeedRev)
                result = ReverseBytes(result);

            des[index + 0] = (byte)(result & 0xFFUL);
            des[index + 1] = (byte)((result & 0xFF00UL) >> 8);
            des[index + 2] = (byte)((result & 0xFF0000UL) >> 16);
            des[index + 3] = (byte)((result & 0xFF000000UL) >> 24);
            des[index + 4] = (byte)((result & 0xFF00000000UL) >> 32);
            des[index + 5] = (byte)((result & 0xFF0000000000UL) >> 40);
            des[index + 6] = (byte)((result & 0xFF000000000000UL) >> 48);
            des[index + 7] = (byte)((result & 0xFF00000000000000UL) >> 56);
        }

        public static void ToBytesWithEndianBit(this Int64 inValue, byte[] des, int index = 0, EndianBitType desType = EndianBitType.LittleEndian)
        {
            UInt64 _convert = BytesN8Union.Create(inValue).GetValue();
            ToBytesWithEndianBit(_convert, des, index, desType);
        }

        public static byte[] GetBytesWithEndianBit(this UInt64 inValue, EndianBitType desType = EndianBitType.LittleEndian)
        {
            var b = new byte[8];
            ToBytesWithEndianBit(inValue, b, 0, desType);
            return b;
        }

        public static byte[] GetBytesWithEndianBit(this Int64 inValue, EndianBitType desType = EndianBitType.LittleEndian)
        {
            var b = new byte[8];
            ToBytesWithEndianBit(inValue, b, 0, desType);
            return b;
        }
        #endregion

        #region FromBytes
        public static Boolean GetBooleanWithEndianBit(this byte[] src, int index = 0)
        {
            CheckArguments(src, index, 1);
            Boolean _result = false;
            if (src[index] != 0x00)
                _result = true;
            return _result;
        }

        public static Byte GetByteWithEndianBit(this byte[] src, int index = 0)
        {
            CheckArguments(src, index, 1);
            return src[index];
        }

        public static UInt16 GetUInt16WithEndianBit(this byte[] src, int index = 0, EndianBitType srcType = EndianBitType.LittleEndian)
        {
            CheckArguments(src, index, sizeof(UInt16));
            UInt16 result = (UInt16)((src[index]) | (src[index + 1] << 8));
            bool isNeedRev = false;
            if (BitConverter.IsLittleEndian && srcType == EndianBitType.BigEndian)
            {
                isNeedRev = true;
            }
            if (isNeedRev)
                result = ReverseBytes(result);
            return result;
        }

        public static Int16 GetInt16WithEndianBit(this byte[] src, int index = 0, EndianBitType srcType = EndianBitType.LittleEndian)
        {
            UInt16 result = GetUInt16WithEndianBit(src, index, srcType);
            return BytesN2Union.Create(result).sint16vluae;
        }

        public static UInt32 GetUInt32WithEndianBit(this byte[] src, int index = 0, EndianBitType srcType = EndianBitType.LittleEndian)
        {
            CheckArguments(src, index, sizeof(UInt32));
            UInt32 result = (UInt32)((src[index]) | (src[index + 1] << 8) | (src[index + 2] << 16) | (src[index + 3] << 24));
            bool isNeedRev = false;
            if (BitConverter.IsLittleEndian && srcType == EndianBitType.BigEndian)
            {
                isNeedRev = true;
            }
            if (isNeedRev)
                result = ReverseBytes(result);
            return result;
        }

        public static Int32 GetInt32WithEndianBit(this byte[] src, int index = 0, EndianBitType srcType = EndianBitType.LittleEndian)
        {
            UInt32 result = GetUInt32WithEndianBit(src, index, srcType);
            return BytesN4Union.Create(result).sint32vluae;
        }

        public static UInt64 GetUInt64WithEndianBit(this byte[] src, int index = 0, EndianBitType srcType = EndianBitType.LittleEndian)
        {
            CheckArguments(src, index, sizeof(UInt64));
            UInt32 lowBytes = (UInt32)((src[index]) | (src[index + 1] << 8) | (src[index + 2] << 16) | (src[index + 3] << 24));
            UInt32 highBytes = (UInt32)((src[index + 4]) | (src[index + 5] << 8) | (src[index + 6] << 16) | (src[index + 7] << 24));

            UInt64 result = (UInt64)((UInt64)lowBytes + ((UInt64)highBytes << 32));
            bool isNeedRev = false;
            if (BitConverter.IsLittleEndian && srcType == EndianBitType.BigEndian)
            {
                isNeedRev = true;
            }
            if (isNeedRev)
                result = ReverseBytes(result);
            return result;
        }

        public static Int64 GetInt64WithEndianBit(this byte[] src, int index = 0, EndianBitType srcType = EndianBitType.LittleEndian)
        {
            UInt64 result = GetUInt64WithEndianBit(src, index, srcType);
            return BytesN8Union.Create(result).sint64vluae;
        }
        #endregion

    }
}
