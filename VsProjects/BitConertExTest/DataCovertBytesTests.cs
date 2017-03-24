using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitConvertEx;

namespace BitConertExTest
{
    [TestClass]
    public class DataCovertBytesTests
    {

        #region Byte
        [TestMethod]
        [Description("默认状态字节转字节数组")]
        public void TestByteToBytes()
        {
            Byte src = 0x55;
            var buffer = new byte[1];
            src.ToBytesWithEndianBit(buffer);
            Assert.AreEqual(buffer[0], 0x55, "默认转换时，解码与编码结果不一致");
        }


        [TestMethod]
        [Description("字节转字节数组边界检测")]
        public void TestByteToBytesWithBoundary()
        {
            Byte src = 0x55;
            var buffer = new byte[1];
            try
            {
                src.ToBytesWithEndianBit(buffer, 2);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return;
            }
            Assert.Fail("边界检查失败");
        }

        [TestMethod]
        [Description("字节转字节数组空值检测")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestByteToBytesWithNull()
        {
            Byte src = 0x55;
            src.ToBytesWithEndianBit(null, 0);
            Assert.Fail("空值检查失败");
        }
        #endregion


        #region Int16
        [TestMethod]
        public void TestInt16ToBytesByDefault()
        {
            Int16 src = Int16.MinValue;
            var buffer = new byte[2];
            src.ToBytesWithEndianBit(buffer);
            Int16 res = buffer.GetInt16WithEndianBit();
            Assert.AreEqual(res, src);
        }

        [TestMethod]
        public void TestInt16ToBytesByForceBigEndianBit()
        {
            Int16 src = Int16.MinValue;
            var buffer = new byte[2];
            src.ToBytesWithEndianBit(buffer,0, BitConverterEx.EndianBitType.BigEndian);
            Int16 res = buffer.GetInt16WithEndianBit(0, BitConverterEx.EndianBitType.BigEndian);
            Assert.AreEqual(res, src);
        }
        #endregion

        #region UInt16
        [TestMethod]
        public void TestUInt16ToBytesByDefault()
        {
            UInt16 src = UInt16.MaxValue;
            var buffer = new byte[2];
            src.ToBytesWithEndianBit(buffer);
            UInt16 res = buffer.GetUInt16WithEndianBit();
            Assert.AreEqual(res, src);
        }

        [TestMethod]
        public void TestUInt16ToBytesByForceBigEndianBit()
        {
            UInt16 src = UInt16.MaxValue;
            var buffer = new byte[2];
            src.ToBytesWithEndianBit(buffer, 0, BitConverterEx.EndianBitType.BigEndian);
            UInt16 res = buffer.GetUInt16WithEndianBit(0, BitConverterEx.EndianBitType.BigEndian);
            Assert.AreEqual(res, src);
        }
        #endregion

        #region Int32
        [TestMethod]
        public void TestInt32ToBytesByDefault()
        {
            Int32 src = Int32.MinValue;
            var buffer = new byte[4];
            src.ToBytesWithEndianBit(buffer);
            Int32 res = buffer.GetInt32WithEndianBit();
            Assert.AreEqual(res, src);
        }

        [TestMethod]
        public void TestInt32ToBytesByForceBigEndianBit()
        {
            Int32 src = Int32.MinValue;
            var buffer = new byte[4];
            src.ToBytesWithEndianBit(buffer, 0, BitConverterEx.EndianBitType.BigEndian);
            Int32 res = buffer.GetInt32WithEndianBit(0, BitConverterEx.EndianBitType.BigEndian);
            Assert.AreEqual(res, src);
        }
        #endregion

        #region UInt32
        [TestMethod]
        public void TestUInt32ToBytesByDefault()
        {
            UInt32 src = UInt32.MaxValue;
            var buffer = new byte[4];
            src.ToBytesWithEndianBit(buffer);
            UInt32 res = buffer.GetUInt32WithEndianBit();
            Assert.AreEqual(res, src);
        }

        [TestMethod]
        public void TestUInt32ToBytesByForceBigEndianBit()
        {
            UInt32 src = UInt32.MaxValue;
            var buffer = new byte[4];
            src.ToBytesWithEndianBit(buffer, 0, BitConverterEx.EndianBitType.BigEndian);
            UInt32 res = buffer.GetUInt32WithEndianBit(0, BitConverterEx.EndianBitType.BigEndian);
            Assert.AreEqual(res, src);
        }
        #endregion

        #region Int64
        [TestMethod]
        public void TestInt64ToBytesByDefault()
        {
            Int64 src = Int64.MinValue;
            var buffer = new byte[8];
            src.ToBytesWithEndianBit(buffer);
            Int64 res = buffer.GetInt64WithEndianBit();
            Assert.AreEqual(res, src);
        }

        [TestMethod]
        public void TestInt64ToBytesByForceBigEndianBit()
        {
            Int64 src = Int64.MinValue;
            var buffer = new byte[8];
            src.ToBytesWithEndianBit(buffer, 0, BitConverterEx.EndianBitType.BigEndian);
            Int64 res = buffer.GetInt64WithEndianBit(0, BitConverterEx.EndianBitType.BigEndian);
            Assert.AreEqual(res, src);
        }
        #endregion
    }
}
