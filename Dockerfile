################
## Base Image ##
################
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base

#############
## Restore ##
#############
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS restore

WORKDIR /app
EXPOSE 80

RUN mkdir ./budget4home
RUN mkdir ./budget4home.tests

COPY budget4home/*.csproj ./budget4home
COPY budget4home.tests/*.csproj ./budget4home.tests
COPY budget4home.sln .

RUN ["dotnet", "restore"]

###########
## Build ##
###########
FROM restore AS build

COPY budget4home ./budget4home
COPY budget4home.tests ./budget4home.tests
COPY budget4home.sln .

RUN ["dotnet", "publish", "--no-restore", "-c", "Release"]

################
## Unit Tests ##
################
# FROM restore AS test
# COPY . .
# VOLUME ["/app/coverage"]
# ENTRYPOINT ["dotnet", "test"]
# #, "-c", "Release", "--collect:\"XPlat Code Coverage\""]

###################
## Release Image ##
###################
FROM build AS final
WORKDIR /app
COPY --from=build /app/budget4home/bin/Release/*/publish .
ENTRYPOINT ["dotnet", "budget4home.dll"]
