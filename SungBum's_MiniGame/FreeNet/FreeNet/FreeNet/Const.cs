using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeNet
{
    public struct Const<T>
    {
        public T value
        {
            get; private set;
        }

        public Const(T value) : this()
        {
            this.value = value;
        }
    }
}
