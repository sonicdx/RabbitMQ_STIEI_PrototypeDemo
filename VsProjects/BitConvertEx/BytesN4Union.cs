using System;
using System.Runtime.InteropServices;

namespace BitConvertEx
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct BytesN4Union
    {
        // map int value to offset zero
        [FieldOffset(0)]
        public UInt32 uint32value;
        // map float value to offset zero - intValue and floatValue now take the same position in memory
        [FieldOffset(0)]
        public Int32 sint32vluae;

        internal BytesN4Union(UInt32 intValue)
        {
            this.sint32vluae = 0;
            this.uint32value = intValue;
        }

        internal BytesN4Union(Int32 intValue)
        {
            this.uint32value = 0;
            this.sint32vluae = intValue;
        }

        public void SetValue(UInt32 intValue)
        {
            this.sint32vluae = 0;
            this.uint32value = intValue;
        }

        public void SetValue(Int32 intValue)
        {
            this.uint32value = 0;
            this.sint32vluae = intValue;
        }

        internal UInt32 GetValue()
        {
           return this.uint32value;
        }

        public static BytesN4Union Create(UInt32 intValue)
        {
            return new BytesN4Union(intValue);
        }

        public static BytesN4Union Create(Int32 intValue)
        {
            return new BytesN4Union(intValue);
        }
    }
}
