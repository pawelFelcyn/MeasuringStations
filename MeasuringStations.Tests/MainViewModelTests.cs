using FluentAssertions;
using MeasuringStations.Exceptions;
using MeasuringStations.Models;
using MeasuringStations.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MeasuringStations.Tests
{
    public class MainViewModelTests
    {
        private readonly TestNotifier _notifier;

        public MainViewModelTests()
        {
            _notifier = new();
        }

        [Fact]
        public void GetAllStationsCommand_AfterSuccess_AddsStationsToCollection()
        {
            var serviceMock = new Mock<IStationService>();
            var allStations = new Station[] { new(), new(), new() };
            serviceMock.Setup(m => m.GetAllAsync()).ReturnsAsync(allStations);

            var vm = new MainViewModel(serviceMock.Object, null, null, _notifier);
            vm.GetAllStationsCommand.Execute(null);

            vm.AllStations.Count().Should().Be(allStations.Length);

            foreach (var station in allStations)
            {
                vm.AllStations.Should().Contain(station);
            }
        }

        [Fact]
        public void GetAllStationsCommand_AfterFailure_NOtifiesUser()
        {
            var serviceMock = new Mock<IStationService>();
            serviceMock.Setup(m => m.GetAllAsync()).ThrowsAsync(new Exception());
            var vm = new MainViewModel(serviceMock.Object, null, null, _notifier);
            vm.GetAllStationsCommand.Execute(null);
            _notifier.LastMessage.Should().Be("Couldn't load stations.");
        }

        [Fact]
        public void SaveToFileCommand_ForNotLoadedStation_NotifiesUser()
        {
            var vm = new MainViewModel(null, null, null, _notifier);
            vm.SaveToFileCommand.Execute(null);
            _notifier.LastMessage.Should().Be("Station to save has not been loaded.");
        }

        [Fact]
        public void SaveToFileCommand_ForNotSupportedExtension_NotifiesUser()
        {
            var pathProvider = GetPathProvider();
            var factoryMock = new Mock<IStationFileSaverFactory>();
            factoryMock.Setup(m => m.Create(It.IsAny<string>())).Throws(new NotSupportedExtensionException());
            var vm = new MainViewModel(null, pathProvider, factoryMock.Object, _notifier);
            vm.Station = new();
            vm.SaveToFileCommand.Execute(null);
            _notifier.LastMessage.Should().Be("This extension is not supported.");
        }

        private IPathProvider GetPathProvider()
        {
            var pathProviderMock = new Mock<IPathProvider>();
            string path = "path.txt";
            pathProviderMock.Setup(m => m.TryGetPath(out path)).Returns(true);
            return pathProviderMock.Object;
        }

        [Fact]
        public void SaveToFileCommand_ForSupportedExtension_SavesStationToFile()
        {
            var pathProvider = GetPathProvider();
            var factoryMock = new Mock<IStationFileSaverFactory>();
            var saver = new TestStationSaver();
            factoryMock.Setup(m => m.Create(It.IsAny<string>())).Returns(saver);
            var vm = new MainViewModel(null, pathProvider, factoryMock.Object, _notifier);
            vm.Station = new();
            vm.SaveToFileCommand.Execute(null);
            saver.HasBeenSaved.Should().BeTrue();
        }

        [Fact]
        public void GetStationCommand_ForNotFoundStation_NotifiesUser()
        {
            var vm = new MainViewModel(null, null, null, _notifier);
            vm.SelectedStationName = "test";
            vm.GetStationCommand.Execute(null);
            _notifier.LastMessage.Should().Be("Couldn't find given station.");
        }

        [Fact]
        public void GetStationCommand_WhenExceptionIsThrown_NotifiesUser()
        {
            var serviceMock = new Mock<IStationService>();
            serviceMock.Setup(m => m.GetDetailsAsync(It.IsAny<int>())).ThrowsAsync(new Exception());
            var vm = new MainViewModel(serviceMock.Object, null, null, _notifier);
            vm.SelectedStationName = "test";
            vm.AllStations.Add(new() { StationName = "test"});
            vm.GetStationCommand.Execute(null);
            _notifier.LastMessage.Should().Be("Operaton failed.");
        }

        private class TestStationSaver : IStationFileSaver
        {
            public bool HasBeenSaved { get; private set; } = false;

            public Task SaveAsync(StationDetails station, string path)
            {
                HasBeenSaved = true;
                return Task.CompletedTask;
            }
        }

        private class TestNotifier : INotifier
        {
            public string LastMessage { get; private set; }

            public void Notify(string notification)
            {
                LastMessage = notification;
            }
        }
    }
}
