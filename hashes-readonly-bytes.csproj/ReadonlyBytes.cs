using System;
using System.Collections.Generic;
using System.Text;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable<byte>
    {
        private byte[] arr;
        private int hash = -1;
        public byte this[int ab] { get => arr[ab]; }
        public int Length { get => arr.Length; }
        
        public override int GetHashCode()
        {
            const long first = 17;
            const long second = 2147483029;
            const long third = 331;

            if (hash == -1)
            {
                long i = first;
                foreach (var b in arr)
                    i = (i * third + b) % second;
                hash = (int)i;
            }
            return hash;
        }

        public override bool Equals(object ent)
        {
            if (!GetHashCode().Equals(ent.GetHashCode())) return false;
            var that = ent as IEnumerable<byte>;
            if (that == null) return false;
            var a = GetEnumerator();
            var b = that.GetEnumerator();
            for (; ; )
            {
                bool hasA = a.MoveNext();
                bool hasB = b.MoveNext();
                if (hasA != hasB) return false;
                if (!hasA) return true;
                if (a.Current != b.Current) return false;
            }
        }
        
        public override string ToString()
        {
            var strBldr = new StringBuilder();
            strBldr.Append("[");
            bool first = true;
            foreach (var b in arr)
            {
                if (first) first = false;
                else strBldr.Append(", ");
                strBldr.Append(b);
            }
            strBldr.Append("]");
            return strBldr.ToString();
        }
        
        public ReadonlyBytes(params byte[] arr)
        {
            if (arr == null)
                throw new ArgumentNullException();
            this.arr = new byte[arr.Length];
            Array.Copy(arr, this.arr, arr.Length);
        }

        public static ReadonlyBytes operator +(ReadonlyBytes a, ReadonlyBytes b)
        {
            var x = new ReadonlyBytes
            {
                arr = new byte[a.Length + b.Length]
            };
            a.arr.CopyTo(x.arr, 0);
            b.arr.CopyTo(x.arr, x.Length);
            return x;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public virtual IEnumerator<byte> GetEnumerator() =>
            ((IEnumerable<byte>)arr).GetEnumerator();
    }
}