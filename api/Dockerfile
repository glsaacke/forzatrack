# Use the .NET SDK image for development
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy everything (needed for hot reload)
COPY . ./

# Install dependencies
RUN dotnet restore

# Expose API port (change if needed)
EXPOSE 8080

# Run the app in watch mode for hot reload
CMD ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:8080"]


#docker build -t <tag name> .
#docker run -p <port>:<port> -v ${PWD}:/app -w /app <image name>
