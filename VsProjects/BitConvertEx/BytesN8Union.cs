using System;
using System.Runtime.InteropServices;

namespace BitConvertEx
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct BytesN8Union
    {
        // map int value to offset zero
        [FieldOffset(0)]
        public UInt64 uint64value;
        // map float value to offset zero - intValue and floatValue now take the same position in memory
        [FieldOffset(0)]
        public Int64 sint64vluae;

        internal BytesN8Union(UInt64 intValue)
        {
            this.sint64vluae = 0;
            this.uint64value = intValue;
        }

        internal BytesN8Union(Int64 intValue)
        {
            this.uint64value = 0;
            this.sint64vluae = intValue;
        }

        public void SetValue(UInt64 intValue)
        {
            this.sint64vluae = 0;
            this.uint64value = intValue;
        }

        public void SetValue(Int64 intValue)
        {
            this.uint64value = 0;
            this.sint64vluae = intValue;
        }

        internal UInt64 GetValue()
        {
           return this.uint64value;
        }

        public static BytesN8Union Create(UInt64 intValue)
        {
            return new BytesN8Union(intValue);
        }

        public static BytesN8Union Create(Int64 intValue)
        {
            return new BytesN8Union(intValue);
        }
    }
}
