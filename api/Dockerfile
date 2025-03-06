# Use the .NET SDK to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy project files and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining files and build the application
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Use a smaller runtime image for deployment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory in the runtime container
WORKDIR /app

# Copy the built application from the previous stage
COPY --from=build /app/publish .

# Expose API port (change if needed)
EXPOSE 8080

# Run the application
ENTRYPOINT ["dotnet", "api.dll"]
