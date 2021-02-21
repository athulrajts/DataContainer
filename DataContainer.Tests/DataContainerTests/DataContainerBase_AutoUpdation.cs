using Xunit;
using KEI.Infrastructure;
using System.IO;
using System.Threading;
using DataContainer.Utils;

namespace DataContainer.Tests
{
    public class DataContainerBase_AutoUpdation
    {

        [Fact]
        public void DataContainerAutoSaver_ShouldAutoSave()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("D", 3)
                .Build();

            var tester = new AutoSaveTester(A);
            tester.AutoSaver.SaveDelay = 10;

            Assert.False(tester.SavingStartedInvoked);
            Assert.False(tester.SavingFinishedInvoked);

            A["B"] = 5;

            // wait timer
            Thread.Sleep((int)tester.AutoSaver.SaveDelay + 20);

            Assert.True(tester.SavingStartedInvoked);
            Assert.True(tester.SavingFinishedInvoked);
        }

        [Fact]
        public void DataContainerAutoSaver_ShouldAutoSaveWithFilters()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("D", 3)
                .Build();

            var tester = new AutoSaveTester(A);
            tester.AutoSaver.SaveDelay = 10;
            tester.AutoSaver.UseFilters = true;
            tester.AutoSaver.AddFilter("D");

            Assert.False(tester.SavingStartedInvoked);
            Assert.False(tester.SavingFinishedInvoked);

            A["B"] = 5;

            // wait timer
            Thread.Sleep((int)tester.AutoSaver.SaveDelay + 20);

            Assert.False(tester.SavingStartedInvoked);
            Assert.False(tester.SavingFinishedInvoked);

            A["D"] = 10;

            // wait timer
            Thread.Sleep((int)tester.AutoSaver.SaveDelay + 20);

            Assert.True(tester.SavingStartedInvoked);
            Assert.True(tester.SavingFinishedInvoked);
        }

        [Fact]
        public void DataContainerAutoSaver_WillNotUseFiltersIfUseFilterIsFalse()
        {
            IDataContainer A = DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("D", 3)
                .Build();

            var tester = new AutoSaveTester(A);
            tester.AutoSaver.SaveDelay = 10;
            tester.AutoSaver.UseFilters = false;
            tester.AutoSaver.AddFilter("D");

            Assert.False(tester.SavingStartedInvoked);
            Assert.False(tester.SavingFinishedInvoked);

            A["B"] = 5;

            // wait timer
            Thread.Sleep((int)tester.AutoSaver.SaveDelay + 20);

            Assert.True(tester.SavingStartedInvoked);
            Assert.True(tester.SavingFinishedInvoked);
        }


        [Fact]
        public void DataContainerAutoUpdater_ShouldUpdateValues()
        {
            string path = Path.GetTempFileName();

            DataContainerBase A = (DataContainerBase)DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("D", 3)
                .Build();

            A.FilePath = path;
            A.SaveAsXml(path);

            var tester = new AutoUpateTester(A);
            tester.AutoUpdater.PollingInterval = 10;

            Assert.False(tester.UpdateStartedInvoked);
            Assert.False(tester.UpdateFinishedInvoked);

            IDataContainer Updated = (DataContainerBase)DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("D", 5)
                .Build();

            Updated.SaveAsXml(path);

            // wait timer
            Thread.Sleep((int)tester.AutoUpdater.PollingInterval + 20);

            Assert.True(tester.UpdateStartedInvoked);
            Assert.True(tester.UpdateFinishedInvoked);

            Assert.Equal(5, (int)A["D"]);
            Assert.Equal(Updated["D"], A["D"]);

            File.Delete(path);
        }

        [Fact]
        public void DataContainerAutoUpdater_ShouldUpdateValuesWithMultiLevelData()
        {
            string path = Path.GetTempFileName();

            DataContainerBase A = (DataContainerBase)DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("D", 3)
                .DataContainer("AB", b => b
                    .Data("AB1", 1))
                .Build();

            A.FilePath = path;
            A.SaveAsXml(path);

            var tester = new AutoUpateTester(A);
            tester.AutoUpdater.PollingInterval = 10;

            Assert.False(tester.UpdateStartedInvoked);
            Assert.False(tester.UpdateFinishedInvoked);

            IDataContainer Updated = (DataContainerBase)DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("D", 3)
                .DataContainer("AB", b => b
                    .Data("AB1", 2))
                .Build();

            Updated.SaveAsXml(path);

            // wait timer
            Thread.Sleep((int)tester.AutoUpdater.PollingInterval + 20);

            Assert.True(tester.UpdateStartedInvoked);
            Assert.True(tester.UpdateFinishedInvoked);

            IDataContainer AB = (IDataContainer)A["AB"];
            IDataContainer U_AB = (IDataContainer)Updated["AB"];

            Assert.Equal(2, (int)AB["AB1"]);
            Assert.Equal(U_AB["AB1"], AB["AB1"]);

            File.Delete(path);
        }

        [Fact]
        public void DataContainerAutoUpdater_ShouldAddNewValues()
        {
            string path = Path.GetTempFileName();

            DataContainerBase A = (DataContainerBase)DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("D", 3)
                .Build();

            A.FilePath = path;
            A.SaveAsXml(path);

            var tester = new AutoUpateTester(A);

            Assert.False(tester.UpdateStartedInvoked);
            Assert.False(tester.UpdateFinishedInvoked);

            // allow adding new items
            tester.AutoUpdater.CanAddItems = true;
            tester.AutoUpdater.PollingInterval = 10;

            IDataContainer Updated = (DataContainerBase)DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("C", 3)
                .Data("D", 5)
                .Build();

            Updated.SaveAsXml(path);

            // wait timer
            Thread.Sleep((int)tester.AutoUpdater.PollingInterval + 20);

            Assert.True(tester.UpdateStartedInvoked);
            Assert.True(tester.UpdateFinishedInvoked);

            // 1 new property added
            Assert.Equal(4, A.Count);
            // new value is present
            Assert.True(A.ContainsData("C"));
            // has correct value
            Assert.Equal(3, (int)A["C"]);

            File.Delete(path);
        }

        [Fact]
        public void DataContainerAutoUpdater_ShouldAddNewValuesWithMultiLevelData()
        {
            string path = Path.GetTempFileName();

            DataContainerBase A = (DataContainerBase)DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("D", 3)
                .DataContainer("AB", b => b
                    .Data("AB1", 1))
                .Build();

            A.FilePath = path;
            A.SaveAsXml(path);

            var tester = new AutoUpateTester(A);
            tester.AutoUpdater.PollingInterval = 10;

            // allow adding
            tester.AutoUpdater.CanAddItems = true;

            Assert.False(tester.UpdateStartedInvoked);
            Assert.False(tester.UpdateFinishedInvoked);

            IDataContainer Updated = (DataContainerBase)DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("D", 3)
                .DataContainer("AB", b => b
                    .Data("AB1", 1)
                    .Data("AB2", 2))
                .Build();

            Updated.SaveAsXml(path);

            // wait timer
            Thread.Sleep((int)tester.AutoUpdater.PollingInterval + 20);

            Assert.True(tester.UpdateStartedInvoked);
            Assert.True(tester.UpdateFinishedInvoked);

            IDataContainer AB = (IDataContainer)A["AB"];

            // 1 value added
            Assert.Equal(2, AB.Count);
            // check if value exist
            Assert.True(AB.ContainsData("AB2"));
            // check it has correct value
            Assert.Equal(2, (int)AB["AB2"]);

            File.Delete(path);
        }

        [Fact]
        public void DataContainerAutoUpdater_ShouldRemoveValues()
        {
            string path = Path.GetTempFileName();

            DataContainerBase A = (DataContainerBase)DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("D", 3)
                .Build();

            A.FilePath = path;
            A.SaveAsXml(path);

            var tester = new AutoUpateTester(A);
            tester.AutoUpdater.PollingInterval = 10;

            Assert.False(tester.UpdateStartedInvoked);
            Assert.False(tester.UpdateFinishedInvoked);

            // allow removing items
            tester.AutoUpdater.CanRemoveItems = true;

            IDataContainer Updated = (DataContainerBase)DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("D", 5)
                .Build();

            Updated.SaveAsXml(path);

            // wait timer
            Thread.Sleep((int)tester.AutoUpdater.PollingInterval + 20);

            Assert.True(tester.UpdateStartedInvoked);
            Assert.True(tester.UpdateFinishedInvoked);

            // 1 new property removed
            Assert.Equal(2, A.Count);
            // new value is not present
            Assert.False(A.ContainsData("B"));

            File.Delete(path);
        }

        [Fact]
        public void DataContainerAutoUpdater_ShouldRemoveValuesWithMultiLevelData()
        {
            string path = Path.GetTempFileName();

            DataContainerBase A = (DataContainerBase)DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("D", 3)
                .DataContainer("AB", b => b
                    .Data("AB1", 1)
                    .Data("AB2", 2))
                .Build();

            A.FilePath = path;
            A.SaveAsXml(path);

            var tester = new AutoUpateTester(A);

            // allow adding
            tester.AutoUpdater.CanRemoveItems = true;
            tester.AutoUpdater.PollingInterval = 10;

            Assert.False(tester.UpdateStartedInvoked);
            Assert.False(tester.UpdateFinishedInvoked);

            IDataContainer Updated = (DataContainerBase)DataContainerBuilder.Create("A")
                .Data("A", 1)
                .Data("B", 2)
                .Data("D", 3)
                .DataContainer("AB", b => b
                    .Data("AB1", 1))
                .Build();

            Updated.SaveAsXml(path);

            // wait timer
            Thread.Sleep((int)tester.AutoUpdater.PollingInterval + 20);

            Assert.True(tester.UpdateStartedInvoked);
            Assert.True(tester.UpdateFinishedInvoked);

            IDataContainer AB = (IDataContainer)A["AB"];

            // 1 value removed
            Assert.Equal(1, AB.Count);
            // check if value exist
            Assert.False(AB.ContainsData("AB2"));

            File.Delete(path);
        }

    }
}
