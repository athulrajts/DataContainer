using Xunit;
using System.Linq;
using KEI.Infrastructure;
using System.Collections.Generic;
using DataContainer.Utils;

namespace DataContainer.Tests
{
    public class DataContainerBase_Comparing
    {
        [Fact]
        public void IDataContainer_Union_UsesFirstsNameForResultName()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 22)
                .Build();

            IDataContainer B = DataContainerBuilder.Create("B")
                .Data("B", 33)
                .Build();

            IDataContainer AunionB = A.Union(B);
            IDataContainer BunionA = B.Union(A);

            Assert.Equal(A.Name, AunionB.Name);
            Assert.Equal(B.Name, BunionA.Name);
        }

        [Fact]
        public void IDataContainer_Union_UsesValuesFromFirstForSameProperty()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Build();

            IDataContainer B = DataContainerBuilder.Create("B")
                .Data("A", 2)
                .Data("B", 1)
                .Build();

            IDataContainer AunionB = A.Union(B);
            IDataContainer BunionA = B.Union(A);

            int AA = (int)A["A"];
            int AB = (int)A["B"];

            int BA = (int)B["A"];
            int BB = (int)B["B"];

            int AUBA = (int)AunionB["A"];
            int AUBB = (int)AunionB["B"];
            int BUAA = (int)BunionA["A"];
            int BUAB = (int)BunionA["B"];

            Assert.Equal(AA, AUBA);
            Assert.Equal(AB, AUBB);

            Assert.Equal(BA, BUAA);
            Assert.Equal(BB, BUAB);
        }

        [Fact]
        public void IDataContainer_Union_MustWorkWithSingleLevelData()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("C", 3)
                .Data("E", 5)
                .Data("G", 7)
                .Data("Z", 26)
                .Build();

            IDataContainer B = DataContainerBuilder.Create("A")
                .Data("B", 2)
                .Data("D", 4)
                .Data("F", 6)
                .Data("H", 8)
                .Data("Z", 28)
                .Build();

            IDataContainer AunionB = A.Union(B);

            Assert.Equal(9, AunionB.Count); // 4 + 4 + 1 (common)

            var keys = AunionB.GetKeys().ToList();
            keys.Sort();
            Assert.Equal("ABCDEFGHZ", string.Join("", keys)); // all keys are present.
        }

        [Fact]
        public void IDataContainer_Union_MustWorkWithMultiLevelData()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .DataContainer("AA", b => b
                    .Data("A1", 11)
                    .Data("A2", 12)
                    .Data("A3", 13))
                .Data("C", 3)
                .DataContainer("CC", b => b
                    .Data("C1", 31)
                    .Data("C3", 33))
                .Build();

            IDataContainer B = DataContainerBuilder.Create("B")
                .Data("A", 1)
                .Data("B", 2)
                .DataContainer("AA", b => b
                    .Data("A4", 14)
                    .Data("A5", 15)
                    .Data("A6", 16))
                .Data("C", 3)
                .DataContainer("CC", b => b
                    .Data("C2", 32))
                .Build();

            IDataContainer AunionB = A.Union(B);

            Assert.Equal(5, AunionB.Count); // both have same keys, 5

            IDataContainer AA = (IDataContainer)AunionB["AA"];
            Assert.Equal(6, AA.Count); // 3 + 3

            IDataContainer CC = (IDataContainer)AunionB["CC"];
            Assert.Equal(3, CC.Count); // 2 + 1
        }

        [Fact]
        public void IDataContainer_Intersect_UsesValuesFromFirstForSameProperty()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Build();

            IDataContainer B = DataContainerBuilder.Create("B")
                .Data("A", 2)
                .Data("B", 1)
                .Build();

            IDataContainer AintersectB = A.Intersect(B);
            IDataContainer BintersectA = B.Intersect(A);

            int AA = (int)A["A"];
            int AB = (int)A["B"];

            int BA = (int)B["A"];
            int BB = (int)B["B"];

            int AUBA = (int)AintersectB["A"];
            int AUBB = (int)AintersectB["B"];
            int BUAA = (int)BintersectA["A"];
            int BUAB = (int)BintersectA["B"];

            Assert.Equal(AA, AUBA);
            Assert.Equal(AB, AUBB);

            Assert.Equal(BA, BUAA);
            Assert.Equal(BB, BUAB);
        }

        [Fact]
        public void IDataContainer_Intersect_UsesFirstsNameForResultName()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 22)
                .Build();

            IDataContainer B = DataContainerBuilder.Create("B")
                .Data("A", 33)
                .Build();

            IDataContainer AintersectB = A.Intersect(B);
            IDataContainer BintersectA = B.Intersect(A);

            Assert.Equal(A.Name, AintersectB.Name);
            Assert.Equal(B.Name, BintersectA.Name);
        }

        [Fact]
        public void IDataContainer_Intersect_MustWorkWithSingleLevelData()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("C", 3)
                .Data("E", 5)
                .Data("G", 7)
                .Data("Z", 26)
                .Build();

            IDataContainer B = DataContainerBuilder.Create("A")
                .Data("B", 2)
                .Data("D", 4)
                .Data("F", 6)
                .Data("H", 8)
                .Data("Z", 28)
                .Build();

            IDataContainer AintersectB = A.Intersect(B);

            Assert.Equal(1, AintersectB.Count); // only 1 common

            var keys = AintersectB.GetKeys().ToList();
            keys.Sort();
            Assert.Equal("Z", string.Join("", keys)); // all keys are present.
        }

        [Fact]
        public void IDataContainer_Intersect_MustWorkWithMultiLevelData()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .DataContainer("AA", b => b
                    .Data("A1", 11)
                    .Data("A2", 12)
                    .Data("A3", 13))
                .Data("C", 3)
                .DataContainer("CC", b => b
                    .Data("C1", 31)
                    .Data("C3", 33))
                .DataContainer("BB", b => b
                    .Data("B1", 21)
                    .Data("B2", 22))
                .Build();

            IDataContainer B = DataContainerBuilder.Create("B")
                .Data("A", 1)
                .Data("B", 2)
                .DataContainer("AA", b => b
                    .Data("A4", 14)
                    .Data("A5", 15)
                    .Data("A6", 16))
                .Data("C", 3)
                .DataContainer("CC", b => b
                    .Data("C2", 32))
                .DataContainer("BB", b => b
                    .Data("B1", 23)
                    .Data("B2", 24)
                    .Data("B3", 25))
                .Build();

            IDataContainer AintersectB = A.Intersect(B);

            Assert.Equal(6, AintersectB.Count); // both have same keys, 6

            IDataContainer AA = (IDataContainer)AintersectB["AA"];
            Assert.Equal(0, AA.Count); // nothing common

            IDataContainer CC = (IDataContainer)AintersectB["CC"];
            Assert.Equal(0, CC.Count); // nothing common

            IDataContainer BB = (IDataContainer)AintersectB["BB"];
            Assert.Equal(2, BB.Count); // 2 common, 1 only in B
        }

        [Fact]
        public void IDataContainer_IsIdenticalReturnsTrueForSingleLevelData()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("C", 3)
                .Build();

            IDataContainer B = DataContainerBuilder.Create("B")
                .Data("A", 2)
                .Data("B", 3)
                .Data("C", 4)
                .Build();

            Assert.True(A.IsIdentical(B));
            Assert.True(B.IsIdentical(A));
        }

        [Fact]
        public void IDataContainer_IsIdenticalReturnsFalseForSingleLevelData()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Build();

            IDataContainer B = DataContainerBuilder.Create("B")
                .Data("A", 2)
                .Data("B", 3)
                .Data("C", 4)
                .Build();

            Assert.False(A.IsIdentical(B));
            Assert.False(B.IsIdentical(A));
        }

        [Fact]
        public void IDataContainer_IsIdenticalReturnsTrueForMultiLevelData()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .DataContainer("AA", b => b
                    .Data("A1", 1)
                    .Data("A2", 2)
                    .Data("A3", 3))
                .Data("B", 2)
                .Data("C", 3)
                .DataContainer("CC", b => b
                    .Data("C1", 1)
                    .Data("C2", 2))
                .Build();

            IDataContainer B = DataContainerBuilder.Create("B")
                .Data("A", 2)
                .DataContainer("AA", b => b
                    .Data("A1", 1)
                    .Data("A2", 2)
                    .Data("A3", 3))
                .Data("B", 3)
                .Data("C", 4)
                .DataContainer("CC", b => b
                    .Data("C1", 1)
                    .Data("C2", 2))
                .Build();

            Assert.True(A.IsIdentical(B));
            Assert.True(B.IsIdentical(A));
        }

        [Fact]
        public void IDataContainer_IsIdenticalReturnsFalseForMultiLevelData()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .DataContainer("AA", b => b
                    .Data("A1", 1)
                    .Data("A3", 3))
                .Data("B", 2)
                .Data("C", 3)
                .DataContainer("CC", b => b
                    .Data("C1", 1)
                    .Data("C2", 2))
                .Build();

            IDataContainer B = DataContainerBuilder.Create("B")
                .Data("A", 2)
                .DataContainer("AA", b => b
                    .Data("A1", 1)
                    .Data("A2", 2)
                    .Data("A3", 3))
                .Data("B", 3)
                .Data("C", 4)
                .DataContainer("CC", b => b
                    .Data("C1", 1)
                    .Data("C2", 2))
                .Build();

            Assert.False(A.IsIdentical(B));
            Assert.False(B.IsIdentical(A));
        }

        [Fact]
        public void IDataContainer_IsIdenticalReturnsFalseForMultiLevelDataWithSameNumberOfKeys()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .DataContainer("AA", b => b
                    .Data("A1", 1)
                    .Data("A3", 3))
                .Data("B", 2)
                .Data("C", 3)
                .DataContainer("CC", b => b
                    .Data("C1", 1)
                    .Data("C2", 2))
                .Build();

            IDataContainer B = DataContainerBuilder.Create("B")
                .Data("A", 2)
                .DataContainer("AA", b => b
                    .Data("A3", 3))
                .Data("B", 3)
                .Data("C", 4)
                .DataContainer("CC", b => b
                    .Data("C1", 1)
                    .Data("C2", 2)
                    .Data("C3", 3))
                .Build();

            Assert.False(A.IsIdentical(B));
            Assert.False(B.IsIdentical(A));
        }

        [Fact]
        public void IDataContainer_Except_RemovesCommonProperties()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("C", 3)
                .DataContainer("AA", b => b
                    .Data("A1", 11)
                    .Data("A2", 12))
                .Data("D", 4)
                .Build();

            IDataContainer B = DataContainerBuilder.Create("B")
                .Data("F", 1)
                .Data("B", 2)
                .Data("C", 3)
                .Data("X", 4)
                .Data("Y", 2)
                .Data("Z", 3)
                .Build();

            IDataContainer AdifferenceB = A.Except(B);
            IDataContainer BdifferenceA = B.Except(A);

            Assert.Equal(3, AdifferenceB.Count); // remove B,C from A
            Assert.Equal(4, BdifferenceA.Count); // remove B,C from B
        }

        [Fact]
        public void IDataContainer_Merge_MergesSingleLevelData()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("Z", 26)
                .Build();

            IDataContainer B = DataContainerBuilder.Create("B")
                .Data("A", 2)
                .Data("B", 3)
                .Data("C", 4)
                .Data("D", 4)
                .Build();

            A.Merge(B);

            Assert.Equal(5, A.Count);
        }

        [Fact]
        public void IDataContainer_Merge_MergesMultiLevelData()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .DataContainer("AA", b => b
                    .Data("A1", 1)
                    .Data("A3", 3))
                .Data("B", 2)
                .Data("C", 3)
                .DataContainer("CC", b => b
                    .Data("C5", 1)
                    .Data("C6", 2))
                .Build();

            IDataContainer B = DataContainerBuilder.Create("B")
                .Data("A", 2)
                .DataContainer("AA", b => b
                    .Data("A2", 3))
                .Data("B", 3)
                .Data("C", 4)
                .DataContainer("CC", b => b
                    .Data("C1", 1)
                    .Data("C2", 2)
                    .Data("C3", 3))
                .Build();

            A.Merge(B);

            Assert.Equal(5, A.Count);

            IDataContainer AA = (IDataContainer)A["AA"];
            Assert.Equal(3, AA.Count);

            IDataContainer CC = (IDataContainer)A["CC"];
            Assert.Equal(5, CC.Count);
        }

        [Fact]
        public void IDataContainer_GetAllKeysGetsKeysFromAllLevel()
        {
            IDataContainer A = DataContainerBuilder.Create()
                .Data("A", 1)
                .DataContainer("AA", b => b
                    .Data("A1", 1)
                    .Data("A3", 3))
                .Data("B", 2)
                .Data("C", 3)
                .DataContainer("CC", b => b
                    .Data("C5", 1)
                    .Data("C6", 2))
                .Build();

            List<string> keys = A.GetAllKeys().ToList();

            Assert.Equal(7, keys.Count);
        }

        [Fact]
        public void IDataContainer_Remove_RemovesCommonProperties()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("C", 3)
                .DataContainer("AA", b => b
                    .Data("A1", 11)
                    .Data("A2", 12))
                .Data("D", 4)
                .Build();

            var cloneA = A.Clone() as IDataContainer;

            IDataContainer B = DataContainerBuilder.Create("B")
                .Data("F", 1)
                .Data("B", 2)
                .Data("C", 3)
                .Data("X", 4)
                .Data("Y", 2)
                .Data("Z", 3)
                .Build();

            var cloneB = B.Clone() as IDataContainer;

            cloneA.Remove(B);
            cloneB.Remove(A);

            Assert.Equal(3, cloneA.Count); // remove B,C from A
            Assert.Equal(4, cloneB.Count); // remove B,C from B
        }

        [Fact]
        public void IDataContainer_InplaceIntersect_Works()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("C", 3)
                .DataContainer("AA", b => b
                    .Data("A1", 11)
                    .Data("A2", 12))
                .Data("D", 4)
                .Build();

            IDataContainer B = DataContainerBuilder.Create("B")
                .Data("A", 1)
                .Data("B", 2)
                .Data("C", 3)
                .Data("X", 4)
                .Data("D", 2)
                .Data("Z", 3)
                .Build();

            int commonCount = A.GetAllKeys().Intersect(B.GetAllKeys()).Count();

            A.InplaceIntersect(B);

            Assert.Equal(commonCount, A.GetAllKeys().Count());
        }

        [Fact]
        public void IDataContainer_RefreshUpdatesChangedValue()
        {
            IDataContainer A = DataContainerBuilder.Create()
                .Data("A", 2)
                .Data("B", 3)
                .Data("C", 4)
                .Build();
            
            IDataContainer A2 = DataContainerBuilder.Create()
                .Data("A", 2)
                .Data("B", 3)
                .Data("C", 5)
                .Build();

            var listener = new PropertyChangedListener(A);

            Assert.NotEqual(A["C"], A2["C"]);

            A.Refresh(A2);

            Assert.Equal(A["C"], A2["C"]);
            Assert.Equal("C", listener.LastChangedProperty);
            Assert.Single(listener.PropertiesChanged);

        }

        [Fact]
        public void IDataContainer_Merge_ShouldUpdateCategoryDescriptionAndDisplayName()
        {
            IDataContainer A = PropertyContainerBuilder.Create("A")
                .Property("A", 1)
                .Property("B", 2)
                .Property("C", 26)
                .Build();

            IDataContainer B = PropertyContainerBuilder.Create("B")
                .Property("A", 2 , b => b
                    .SetDisplayName("PropertyA")
                    .SetDescription("PropertyA")
                    .SetCategory("Category"))
                .Property("B", 3, b => b
                    .SetDisplayName("PropertyB")
                    .SetDescription("PropertyB")
                    .SetCategory("Category"))
                .Property("C", 4, b => b
                    .SetDisplayName("PropertyC")
                    .SetDescription("PropertyC")
                    .SetCategory("Category"))
                .Build();

            bool result = A.Merge(B);

            foreach (var item in A.OfType<PropertyObject>())
            {
                Assert.Equal($"Property{item.Name}", item.DisplayName);
                Assert.Equal($"Property{item.Name}", item.Description);
                Assert.Equal("Category", item.Category);
            }

            Assert.True(result);
        }

        [Fact]
        public void IDataContainer_Merge_ReturnsFalseIfNoChange()
        {
            IDataContainer A = PropertyContainerBuilder.Create("A")
                .Property("A", 1)
                .Property("B", 2)
                .Property("C", 26)
                .Build();

            IDataContainer B = PropertyContainerBuilder.Create("B")
                .Property("A", 1)
                .Property("B", 2)
                .Property("C", 26)
                .Build();

            Assert.False(A.Merge(B));
        }
    }
}
