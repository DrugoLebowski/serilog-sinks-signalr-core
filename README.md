# serilog-sinks-signalr-core

[![Build status](https://ci.appveyor.com/api/projects/status/shtaq2n6q7u8hg6s?svg=true)](https://ci.appveyor.com/project/DrugoLebowski/serilog-sinks-signalr-core)

A Serilog' sink that writes event to SignalR Hub.

Inspired by serilog-sink-signalr, I decided to rewrite it because this latter is not fully compatible with Microsoft.AspNetCore.SignalR.Core.

## Usage and configuration
The hub to use with the Sink __must__ be strong-typed and must extend the interface ISerilogHub. An example is as follows

```csharp
public SerilogHub : Serilog.Sinks.SignalR.Core.Interfaces.ISerilogHub
{}
```

An example sink' setup is as follows

```csharp
[...]

var hub = ServiceProvider.GetService<IHubContext<SerilogHub, ISerilogHub>>();

Log.Logger = new LoggerConfiguration()
    .WriteTo.SignalR(
        hub,
        formatProvider: new CustomFormatProvider(),
        groups: new [] { "group1", },
        userIds: new [] { "user2", },
        excludedConnectionIds: new [] { "1", }
    )
    .CreateLogger();

[...]

app.UseSignalR(conf => {
    conf.MapHub<SerilogHub>("/log");
})
```

### Receive the log events

In the client code, subscribe to the `PushEventLog` method.

```csharp
[...]

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:8080/log")
    .Build()

connection.On<string>("PushEventLog", (string message) => {
    Console.WriteLine(message);
});

[...]
```

## Options
In the package it's also available an abstract base hub (BaseSerilogHub), that can be extended and that gives some utility functions (like SubscribeToGroup). Hence, feel free to extend that class if you want.

