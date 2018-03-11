using System;
using System.Collections.Generic;
using System.Text;

namespace Inspector
{
    public class Type<T>
    {
        public static T New(params object[] args) =>
            throw new NotImplementedException();

        public static T Uninitialized() =>
            throw new NotImplementedException();
    }
}
