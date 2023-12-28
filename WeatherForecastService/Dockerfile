FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY . ./
RUN chmod +x run.sh
RUN apt-get update && \
	apt-get install -y curl unzip jq apt-transport-https curl gnupg && \
	curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip" && \
	unzip -uo awscliv2.zip && \
	./aws/install && \
	sh -c "echo 'deb [signed-by=/usr/share/keyrings/datadog-archive-keyring.gpg] https://apt.datadoghq.com/ stable 7' > /etc/apt/sources.list.d/datadog.list" && \
	touch /usr/share/keyrings/datadog-archive-keyring.gpg && \
	chmod a+r /usr/share/keyrings/datadog-archive-keyring.gpg && \
	curl https://keys.datadoghq.com/DATADOG_APT_KEY_CURRENT.public | gpg --no-default-keyring --keyring /usr/share/keyrings/datadog-archive-keyring.gpg --import --batch && \
	curl https://keys.datadoghq.com/DATADOG_APT_KEY_C0962C7D.public | gpg --no-default-keyring --keyring /usr/share/keyrings/datadog-archive-keyring.gpg --import --batch && \
	curl https://keys.datadoghq.com/DATADOG_APT_KEY_F14F620E.public | gpg --no-default-keyring --keyring /usr/share/keyrings/datadog-archive-keyring.gpg --import --batch && \
	curl https://keys.datadoghq.com/DATADOG_APT_KEY_382E94DE.public | gpg --no-default-keyring --keyring /usr/share/keyrings/datadog-archive-keyring.gpg --import --batch && \
	apt-get update && \
	apt-get install datadog-agent datadog-signing-keys

RUN dotnet restore
RUN dotnet publish \
	-c Release \
	-o out

ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={846F5F1C-F9AE-4B07-969E-05C26BC060D8}
ENV CORECLR_PROFILER_PATH=/app/out/datadog/linux-x64/Datadog.Trace.ClrProfiler.Native.so
ENV DD_DOTNET_TRACER_HOME=/app/out/datadog
ENV DD_APM_ENABLED=true
ENV DD_LOGS_ENABLED=true
ENV ECS_FARGATE=true
RUN /bin/bash /app/out/datadog/createLogPath.sh
ENV ASPNETCORE_URLS=http://+:3000
EXPOSE 3000
ENTRYPOINT ["/bin/bash", "run.sh"]