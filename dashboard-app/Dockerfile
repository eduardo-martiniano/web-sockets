#stage 1
FROM node:14.21.3 as node
WORKDIR /app
COPY dashboard-app/package.json ./
RUN npm install
COPY dashboard-app ./
RUN npm run build
#stage 2
FROM nginx:alpine
COPY --from=node /app/dist/dashboard-app /usr/share/nginx/html
