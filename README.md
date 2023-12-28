# MetricsDemo

To observe counters while running on your local computer, first make sure dotnet-counters is installed:

```
dotnet tool update -g dotnet-counters
```

Then, after launching the application, start dotnet-counters:

```
dotnet-counters monitor -n MetricsDemo weather_forecast_api
```