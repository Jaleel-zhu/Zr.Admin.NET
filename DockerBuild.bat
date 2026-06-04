docker build -f ZR.Admin.WebApi/Dockerfile -t zradmin-webapi:latest . ^
    && docker save -o zradmin-webapi.tar zradmin-webapi:latest