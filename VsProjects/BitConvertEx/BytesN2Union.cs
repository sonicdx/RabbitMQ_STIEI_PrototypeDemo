using System;
using System.Runtime.InteropServices;

namespace BitConvertEx
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct BytesN2Union
    {
        // map int value to offset zero
        [FieldOffset(0)]
        public UInt16 uint16value;
        // map float value to offset zero - intValue and floatValue now take the same position in memory
        [FieldOffset(0)]
        public Int16 sint16vluae;

        internal BytesN2Union(UInt16 intValue)
        {
            this.sint16vluae = 0;
            this.uint16value = intValue;
        }

        internal BytesN2Union(Int16 intValue)
        {
            this.uint16value = 0;
            this.sint16vluae = intValue;
        }

        public void SetValue(UInt16 intValue)
        {
            this.sint16vluae = 0;
            this.uint16value = intValue;
        }

        public void SetValue(Int16 intValue)
        {
            this.uint16value = 0;
            this.sint16vluae = intValue;
        }

        internal UInt16 GetValue()
        {
           return this.uint16value;
        }

        public static BytesN2Union Create(UInt16 intValue)
        {
            return new BytesN2Union(intValue);
        }

        public static BytesN2Union Create(Int16 intValue)
        {
            return new BytesN2Union(intValue);
        }
    }
}
