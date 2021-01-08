FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY bin/Debug/net5.0/publish/ /app/
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000
ENTRYPOINT ["dotnet", "linkmir.dll"]
