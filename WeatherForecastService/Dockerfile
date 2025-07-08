FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . ./

RUN dotnet restore
RUN dotnet publish \
	-c Release \
	-o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/out .
COPY run.sh .
RUN chmod +x run.sh
ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={846F5F1C-F9AE-4B07-969E-05C26BC060D8}
ENV CORECLR_PROFILER_PATH=/app/datadog/linux-x64/Datadog.Trace.ClrProfiler.Native.so
ENV DD_DOTNET_TRACER_HOME=/app/datadog
ENV DD_LOGS_INJECTION=true
ENV DD_RUNTIME_METRICS_ENABLED=true
ENV DD_LOGS_DIRECT_SUBMISSION_INTEGRATIONS=ILogger
ENV ASPNETCORE_URLS=http://+:3000
EXPOSE 3000
ENTRYPOINT ["/bin/bash", "run.sh"]