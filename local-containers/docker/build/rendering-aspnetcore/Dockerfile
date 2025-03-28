ARG BASE_IMAGE
FROM ${BASE_IMAGE}

ARG PROJECT_FILE

WORKDIR /app

CMD ["dotnet", "watch", "run", -v, "--urls", "http://*:80"]