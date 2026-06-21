FROM node:22 AS build
WORKDIR /app

# Install dependencies
COPY package*.json ./
RUN npm ci

# Copy source and build
COPY . .
RUN npm run build --configuration=production

# Serve using Nginx
FROM nginx:alpine AS runtime
COPY --from=build /app/dist/loan-portal-angular/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
