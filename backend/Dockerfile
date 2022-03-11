FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY bin/Release/net6.0/linux-x64/publish  .
RUN mkdir -p /app/database
EXPOSE 5000
CMD ["dotnet", "webbot.dll"]