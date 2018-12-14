FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 8080

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Accounts.sln ./
COPY Accounts.API/Accounts.API.csproj Accounts.API/
COPY Accounts.Adapter/Accounts.Adapter.csproj Accounts.Adapter/
COPY Accounts.DTO/Accounts.DTO.csproj Accounts.DTO/
COPY Accounts.Entities/Accounts.Entities.csproj Accounts.Entities/
COPY Accounts.API.Messages/Accounts.API.Messages.csproj Accounts.API.Messages/
COPY Accounts.DI/Accounts.DI.csproj Accounts.DI/
COPY Accounts.Services.Entity/Accounts.Services.Entity.csproj Accounts.Services.Entity/
COPY Accounts.Repository/Accounts.Repository.csproj Accounts.Repository/
COPY Accounts.Repository.MySQL/Accounts.Repository.MySQL.csproj Accounts.Repository.MySQL/
COPY docker-compose.dcproj ./
RUN dotnet restore Accounts.sln -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Accounts.API
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Accounts.API.dll"]
