#Restore project
FROM mcr.microsoft.com/dotnet/core/sdk:2.2.402
RUN mkdir /test
WORKDIR /test
CMD tail -f /dev/null
