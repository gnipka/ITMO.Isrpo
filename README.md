# IO.Swagger - ASP.NET Core 2.0 Server

This is a sample Workout Schedule Service based on the OpenAPI 3.0 specification. 

## Run

Linux/OS X:

```
sh build.sh
```

Windows:

```
build.bat
```

## Run in Docker

```
cd src/IO.Swagger
docker build -t io.swagger .
docker run -p 5000:5000 io.swagger
```
