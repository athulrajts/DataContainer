using System;
using System.Linq;
using Xunit;
using KEI.Infrastructure;
using KEI.Infrastructure.Utils;

namespace DataContainer.Tests
{
    public class DataContainerBase_Misc
    {
        [Theory]
        [InlineData("MyProperty")]
        [InlineData("MyProperty2")]
        [InlineData("My_Property")]
        [InlineData("MyProperty_1")]
        public void IdentifierExtensions_IsValidIdentifierReturnsTrue(string identifierName)
        {
            Assert.True(IdentifierExtensions.IsValidIdentifier(identifierName));
        }

        [Theory]
        [InlineData("My Property")]
        [InlineData("MyProperty 2")]
        [InlineData("2MyProperty")]
        [InlineData("My$Property")]
        [InlineData("MyProperty[0]")]
        public void IdentifierExtensions_IsValidIdentifierReturnsFalse(string identifierName)
        {
            Assert.False(IdentifierExtensions.IsValidIdentifier(identifierName));
        }

        #region SnapShots

        [Fact]
        public void SnapShot_GetsAllValuesSingleLevelData()
        {
            IDataContainer A = (DataContainerBase)DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("C", 3)
                .Data("D", 4)
                .Build();

            SnapShot snapShot = A.GetSnapShot();

            Assert.Equal(4, snapShot.Count());
            Assert.Contains("A", snapShot.Keys);
            Assert.Contains("B", snapShot.Keys);
            Assert.Contains("C", snapShot.Keys);
            Assert.Contains("D", snapShot.Keys);

            Assert.Equal(A["A"], snapShot["A"].Value);
            Assert.Equal(A["B"], snapShot["B"].Value);
            Assert.Equal(A["C"], snapShot["C"].Value);
            Assert.Equal(A["D"], snapShot["D"].Value);
        }

        [Fact]
        public void SnapShot_GetAllValuesMultiLevelData()
        {
            IDataContainer A = (DataContainerBase)DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("C", 3)
                .DataContainer("AB", b => b
                    .Data("AB1", 1)
                    .Data("AB2", 2))
                .Build();

            SnapShot snapShot = A.GetSnapShot();

            Assert.Equal(5, snapShot.Count());
            Assert.Contains("A", snapShot.Keys);
            Assert.Contains("B", snapShot.Keys);
            Assert.Contains("C", snapShot.Keys);
            Assert.Contains("AB.AB1", snapShot.Keys);
            Assert.Contains("AB.AB2", snapShot.Keys);

            Assert.Equal(A["A"], snapShot["A"].Value);
            Assert.Equal(A["B"], snapShot["B"].Value);
            Assert.Equal(A["C"], snapShot["C"].Value);
            Assert.Equal(A["AB.AB1"], snapShot["AB.AB1"].Value);
            Assert.Equal(A["AB.AB2"], snapShot["AB.AB2"].Value);
        }

        [Fact]
        public void SnapShot_ValuesShouldNotChangeWhenIDataContainerChanges()
        {
            IDataContainer A = (DataContainerBase)DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Build();

            SnapShot snapShot = A.GetSnapShot();

            Assert.Single(snapShot);
            Assert.Equal(A["A"], snapShot["A"].Value);

            object origValue = A["A"];
            A["A"] = 3;

            Assert.NotEqual(A["A"], snapShot["A"].Value);
            Assert.Equal(origValue, snapShot["A"].Value);
        }

        [Fact]
        public void SnapShot_Difference_OnlyHasValuesWhichAreDifferent()
        {
            IDataContainer A = (DataContainerBase)DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("C", 3)
                .DataContainer("AB", b => b
                    .Data("AB1", 1)
                    .Data("AB2", 2))
                .Build();

            IDataContainer B = (DataContainerBase)DataContainerBuilder.Create("B")
                .Data("A", 1)
                .Data("B", 22)
                .Data("C", 3)
                .DataContainer("AB", b => b
                    .Data("AB1", 1)
                    .Data("AB2", 21))
                .Build();

            SnapShotDiff diff = A.GetSnapShot() - B.GetSnapShot();

            Assert.Equal(2, diff.Count());
            Assert.Contains("B", diff.Keys);
            Assert.Contains("AB.AB2", diff.Keys);

            Assert.Equal(A["B"], diff["B"].Left);
            Assert.Equal(B["B"], diff["B"].Right);

            Assert.Equal(A["AB.AB2"], diff["AB.AB2"].Left);
            Assert.Equal(B["AB.AB2"], diff["AB.AB2"].Right);
        }

        #endregion

        #region Cloning

        [Fact]
        public void IDataContainer_Clone_WorksForValueTypes()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", new Color(1,1,1))
                .Build();

            IDataContainer clone = (IDataContainer)A.Clone();

            Assert.Equal(A.Count, clone.Count);
            Assert.True(A.GetKeys().SequenceEqual(clone.GetKeys()));
            Assert.Equal(A["A"], clone["A"]);
            Assert.Equal(A["B"], clone["B"]);
        }

        [Fact]
        public void IDataCointainer_Clone_WorksForReferenceTypes()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", new object())
                .Data("B", new object())
                .Build();

            IDataContainer clone = (IDataContainer)A.Clone();

            Assert.Equal(A.Count, clone.Count);
            Assert.True(A.GetKeys().SequenceEqual(clone.GetKeys()));

            Assert.NotSame(A["A"], clone["A"]);
            Assert.NotSame(A["B"], clone["B"]);

        }

        #endregion

    }
}
