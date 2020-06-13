FROM node:14.4.0-alpine3.12 AS build
COPY package.json /usr/angular-workdir/
WORKDIR /usr/angular-workdir
RUN npm install
COPY . .
RUN npm run build

FROM nginx:1.17.1-alpine
RUN rm -rf /usr/share/nginx/html/*
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=build /usr/angular-workdir/dist/Battleship /usr/share/nginx/html