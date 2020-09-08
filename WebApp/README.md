### dockerコマンドメモ

#### build & run
```
cd WebApp
docker build -t webapp .
docker run -d -p 8080:80 --env ASPNETCORE_ENVIRONMENT=Development --name myapp webapp

SET AWS_ACCESS_KEY_ID=xxx
SET AWS_SECRET_ACCESS_KEY=xxxx
SET AWS_REGION=ap-northeast-1
docker run -d -p 8080:80 --env ASPNETCORE_ENVIRONMENT= --env AWS_ACCESS_KEY_ID=%AWS_ACCESS_KEY_ID% --env AWS_SECRET_ACCESS_KEY=%AWS_SECRET_ACCESS_KEY% --env AWS_REGION=%AWS_REGION% --name myapp webapp
```

#### delete container
```
docker ps -aq
SET CONTAINER_ID=8ab2271a9020
docker stop %CONTAINER_ID%
docker rm %CONTAINER_ID%
```


### login container
```
docker exec -it 66244e0b59ae  /bin/bash
```


### push Amazon ECR
```
SET ACCOUNT_ID=
SET AWS_REGION=ap-northeast-1
SET ECR_REPOSITORY=webapp
SET DOCKER_IMAGE_ID=
aws ecr get-login-password --region %AWS_REGION%| docker login --username AWS --password-stdin %ACCOUNT_ID%.dkr.ecr.%AWS_REGION%.amazonaws.com
docker tag %DOCKER_IMAGE_ID% %ACCOUNT_ID%.dkr.ecr.%AWS_REGION%.amazonaws.com/%ECR_REPOSITORY%
docker push %ACCOUNT_ID%.dkr.ecr.%AWS_REGION%.amazonaws.com/%ECR_REPOSITORY%
```