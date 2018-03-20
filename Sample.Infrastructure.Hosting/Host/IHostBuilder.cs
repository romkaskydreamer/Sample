﻿using System;
using Autofac;
using Microsoft.Extensions.Configuration;
using Sample.Infrastructure.Hosting.Service;
using Serilog;

namespace Sample.Infrastructure.Hosting.Host
{
    public interface IHostBuilder
    {
        IHostBuilder WithConfiguration(Action<IConfigurationBuilder> configurator);
        IHostBuilder WithLogger(Func<LoggerConfiguration, LoggerConfiguration> loggerConfigurator);
        IHostBuilder With(Action<ContainerBuilder> serviceConfigurator);
        IHostBuilder With(Action<ContainerBuilder, IConfiguration> serviceConfigurator);
        IHostBuilder WithService<TService>() where TService : IService, new();

        IHost Build();
    }
}