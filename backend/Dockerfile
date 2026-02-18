# Step 1: Build the app
# get .NET build tools
# set working folder
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build   
WORKDIR /src                                     

# copy your source code
# restore NuGet packages
# build your API
COPY . .                                         
RUN dotnet restore                             
RUN dotnet publish -c Release -o /app/publish   

# Step 2: Run the app
# runtime-only image
FROM mcr.microsoft.com/dotnet/aspnet:8.0.        
WORKDIR /app

# copy built files
COPY --from=build /app/publish .                 

# app listens on 8080
ENV ASPNETCORE_URLS=http://+:8080                
EXPOSE 8080

# start your API
ENTRYPOINT ["dotnet", "TodoApi.dll"]             
