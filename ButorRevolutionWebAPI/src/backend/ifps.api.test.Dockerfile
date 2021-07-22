#Restore project
FROM mcr.microsoft.com/dotnet/core/sdk:2.2.402
RUN mkdir /testResults
WORKDIR /src
COPY . .
CMD tail -f /dev/null
