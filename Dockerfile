# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the sln and csproj files / Копиране sln и csproj файловете
COPY RentAutoApp.sln ./
#COPY Directory.Packages.props ./
#COPY NuGet.config ./

COPY src/RentAutoApp.Web/*.csproj ./src/RentAutoApp.Web/
COPY src/RentAutoApp.Data/*.csproj ./src/RentAutoApp.Data/
COPY src/RentAutoApp.Data.Models/*.csproj ./src/RentAutoApp.Data.Models/
COPY src/RentAutoApp.Data.Common/*.csproj ./src/RentAutoApp.Data.Common/
COPY src/RentAutoApp.Services.Core/*.csproj ./src/RentAutoApp.Services.Core/
COPY src/RentAutoApp.Services.AutoMapping/*.csproj ./src/RentAutoApp.Services.AutoMapping/
COPY src/RentAutoApp.Services.Common/*.csproj ./src/RentAutoApp.Services.Common/
COPY src/RentAutoApp.Web.ViewModels/*.csproj ./src/RentAutoApp.Web.ViewModels/
COPY src/RentAutoApp.Web.Infrastructure/*.csproj ./src/RentAutoApp.Web.Infrastructure/
COPY src/RentAutoApp.GCommon/*.csproj ./src/RentAutoApp.GCommon/
COPY src/RentAutoApp.Infrastructure.Email/RentAutoApp.Infrastructure.Email.csproj src/RentAutoApp.Infrastructure.Email/
COPY src/RentAutoApp.Services.Messaging/RentAutoApp.Services.Messaging.csproj src/RentAutoApp.Services.Messaging/


# Addiction recovery / Възстановяване на зависимости
RUN dotnet restore "src/RentAutoApp.Web/RentAutoApp.Web.csproj"

# Copy all code/ Копиране на целия код
COPY . ./

# Publish only RentAutoApp.Web / Публикуване само RentAutoApp.Web
WORKDIR /src/src/RentAutoApp.Web
RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production
COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "RentAutoApp.Web.dll"]
