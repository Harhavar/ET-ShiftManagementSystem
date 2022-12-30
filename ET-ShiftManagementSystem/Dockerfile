#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY "ET-ShiftManagementSystem.csproj", "ET-ShiftManagementSystem/"
COPY ["ShiftManagementServises/ShiftManagementServises.csproj", "ShiftManagementServises/"]
COPY ["ShiftMgtDbContext/ShiftMgtDbContext.csproj", "ShiftMgtDbContext/"]
RUN dotnet restore "ET-ShiftManagementSystem/ET-ShiftManagementSystem.csproj"
COPY . .
WORKDIR "/src/ET-ShiftManagementSystem"
RUN dotnet build "ET-ShiftManagementSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ET-ShiftManagementSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ET-ShiftManagementSystem.dll"]
