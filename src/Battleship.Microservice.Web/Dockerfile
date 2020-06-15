FROM node:14.4.0-alpine3.12 AS build
WORKDIR /src/
COPY /Battleship.Microservice.Web/package.json  .
RUN npm install --no-optional
COPY /Battleship.Microservice.Web/. .
RUN npm run build --prod

FROM nginx:1.17.1-alpine
RUN rm -rf /usr/share/nginx/html/*
COPY /Battleship.Microservice.Web/nginx.conf /etc/nginx/nginx.conf
COPY --from=build /src/dist/Battleship /usr/share/nginx/html