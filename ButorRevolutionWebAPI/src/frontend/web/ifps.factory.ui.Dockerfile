# build stage
FROM node:12.13.0 as install-stage
RUN npm install gulp-cli -g
WORKDIR /app

COPY ["frontend/web/package.json","frontend/web/package-lock.json", "./"]

RUN npm install 

#Generate TSClient project
FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS tsGen-stage
WORKDIR /src
COPY ./backend .
WORKDIR /src/factory/IFPS.Factory.API/
RUN dotnet build "IFPS.Factory.API.csproj" -c Release  -o /app 

WORKDIR /src/build/TSClientGenerator/
RUN dotnet build "TSClientGenerator.csproj" -c Release -o /app
WORKDIR /app
RUN ["dotnet", "TSClientGenerator.dll", "/app/generatedClient/index.ts", "IFPS.Factory.API.dll", "IFPS.Factory.API.Common.IFPSControllerBase"]

#build
FROM install-stage as build-stage
WORKDIR /app

COPY frontend/web/ .
COPY --from=tsGen-stage /app/generatedClient/index.ts ./projects/factory/src/app/shared/clients/
#COPY --from=karpinszkyandras/ifpsrepository:sales-api /app/generatedClient/index.ts ./projects/sales/src/app/shared/clients/

RUN gulp iconfont --cwd ./projects/factory/src
#RUN npm rebuild node-sass
RUN yarn build butor-shared-lib
RUN yarn build factory -c prodlike

# production stage
FROM nginx:1.16.1-alpine as production-stage
COPY --from=build-stage /app/dist/factory /usr/share/nginx/html
COPY frontend/web/tools/nginx/nginx.factory.conf /etc/nginx/nginx.conf

EXPOSE 80

CMD ["/bin/sh",  "-c",  "envsubst < /usr/share/nginx/html/assets/settings.template.json > /usr/share/nginx/html/assets/settings.json && exec nginx -g 'daemon off;'"]


