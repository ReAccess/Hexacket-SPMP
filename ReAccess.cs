using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReAccess
{
    public static class ArrayUtility<T>
    {
        public static T[] Concate(params T[][] ArrayX)
        {
            List<T> A = new List<T>();
            foreach (T[] X in ArrayX) A.AddRange(X);
            return A.ToArray();
        }
    }
}
