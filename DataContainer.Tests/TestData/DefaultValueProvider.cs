using System;
using System.Collections.Generic;
using System.Text;

namespace DataContainer.Tests.TestData
{
    public class DefaultValueProvider
    {
        public object GetValue(Type t)
        {
            if (t == typeof(string))
            {
                return "Hello";
            }
            else if (t == typeof(bool))
            {
                return true;
            }
            else if (t == typeof(int[]))
            {
                return new int[] { 1, 2, 3 };
            }
            else if (t == typeof(int[,]))
            {
                return new int[,] { { 1, 1 }, { 2, 2 } };
            }
            else
            {
                return Activator.CreateInstance(t);
            }
        }
    }
}
