using System;
using System.Collections.Generic;
using KEI.Infrastructure;

namespace DataContainer.Tests
{
    public class Inputs
    {
        public static IEnumerable<object[]> PrimitiveTypeValues()
        {
            return new List<object[]>
            {
                new object[]{ (byte)1},
                new object[]{ true },
                new object[]{ (short)1},
                new object[]{ (long)1},
                new object[]{ (ushort)1},
                new object[]{ (uint)1},
                new object[]{ (ulong)1},
                new object[]{ 1 },
                new object[]{ 1.1 },
                new object[]{ 1.2f},
                new object[]{ '@'},
                new object[]{ "Hello"},
                new object[]{ DateTime.Now},
                new object[]{ TimeSpan.FromSeconds(20)},
                new object[]{ new Point(2,2)},
                new object[]{ new Color(255,255,255) },
                new object[]{ BindingMode.OneWayToSource},
                new object[]{ new int[] { 1, 2, 3} },
                new object[]{ new int[,] { {1,2}, {1,2} } }
            };
        }
    }
}
