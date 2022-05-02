#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Trainings.OpenMpi.Api/Trainings.OpenMpi.Api.csproj", "Trainings.OpenMpi.Api/"]
COPY ["Trainings.OpenMpi.Dal/Trainings.OpenMpi.Dal.csproj", "Trainings.OpenMpi.Dal/"]
RUN dotnet restore "Trainings.OpenMpi.Api/Trainings.OpenMpi.Api.csproj"
COPY . .
WORKDIR "/src/Trainings.OpenMpi.Api"
RUN dotnet build "Trainings.OpenMpi.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Trainings.OpenMpi.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Trainings.OpenMpi.Api.dll"]