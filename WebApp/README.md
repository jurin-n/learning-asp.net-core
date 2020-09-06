### dockerコマンドメモ

#### build & run
```
cd WebApp
docker build -t webapp .
docker run -d -p 8080:80 --env ASPNETCORE_ENVIRONMENT=Development --name myapp webapp
```

#### delete container
```
docker ps -aq
docker rm 8ab2271a9020
```


### login container
```
docker exec -it 66244e0b59ae  /bin/bash
```
