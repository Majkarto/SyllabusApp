FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/SyllabusApp.Application/ src/SyllabusApp.Application/
COPY src/SyllabusApp.Domain/ src/SyllabusApp.Domain/
COPY frontend/SyllabusApp.Web/ frontend/SyllabusApp.Web/
WORKDIR /src/frontend/SyllabusApp.Web

ADD https://github.com/tailwindlabs/tailwindcss/releases/download/v4.3.3/tailwindcss-linux-x64 /usr/local/bin/tailwindcss
RUN chmod +x /usr/local/bin/tailwindcss \
    && tailwindcss -i wwwroot/css/tailwind.css -o wwwroot/css/output.css --minify

ARG API_BASE_URL=http://localhost:8081/
RUN printf '{\n  "ApiBaseUrl": "%s"\n}\n' "$API_BASE_URL" > wwwroot/appsettings.json \
    && rm -f wwwroot/appsettings.Development.json
RUN dotnet publish SyllabusApp.Web.csproj -c Release -o /app

FROM nginx:alpine
COPY frontend/SyllabusApp.Web/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/wwwroot /usr/share/nginx/html
EXPOSE 80
