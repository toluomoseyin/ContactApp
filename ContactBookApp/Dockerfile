FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base
WORKDIR /src
EXPOSE 80
COPY *.sln .
COPY ["ContactBookApp/ContactBookApp.csproj", "ContactBookApp/"]
COPY . .
RUN dotnet restore

# (1)  docker build --tag tolu .   (2) docker run -d -p 8000:80 tolu
#Publishing
FROM base AS publish
WORKDIR /src/ContactBookApp
RUN dotnet publish -c Release -o /src/publish
#Get the runtime into a folder called app
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS runtime
WORKDIR /src
COPY --from=publish /src/publish .

#ENTRYPOINT ["dotnet", "ContactBookApp.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ContactBookApp.dll