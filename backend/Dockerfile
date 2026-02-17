# Step 1: Build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build   # get .NET build tools
WORKDIR /src                                     # set working folder

COPY . .                                         # copy your source code
RUN dotnet restore                               # restore NuGet packages
RUN dotnet publish -c Release -o /app/publish    # build your API

# Step 2: Run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0.        # runtime-only image
WORKDIR /app

COPY --from=build /app/publish .                 # copy built files

ENV ASPNETCORE_URLS=http://+:8080                # app listens on 8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TodoApi.dll"]             # start your API
