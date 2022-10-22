using Autofac;
using MeasuringStations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasuringStations
{
    internal class Bootstrapper
    {
        public static IContainer Container { get; private set; }

        static Bootstrapper()
        {
            RegisterTypes();
        }

        private static void RegisterTypes()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<StationService>().As<IStationService>();
            builder.RegisterType<StationFileSaverFactory>().As<IStationFileSaverFactory>();
            builder.RegisterType<FIleDIalogPathProvider>().As<IPathProvider>();
            builder.RegisterType<MainViewModel>().AsSelf();
            Container = builder.Build();
        }
    }
}
