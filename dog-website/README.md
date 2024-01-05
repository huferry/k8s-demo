# Dog Website

## Docker

**Create Image**
```shell
docker build -t huferry/dog-website .
```

**Run Image**
```shell
docker run -p 3000:3000 huferry/dog-website 
```

**Tagging Image**
```shell
docker tag huferry/dog-website huferry/dog-website:1.0.0
```

**Pushing Image**
```shell
docker login
docker push huferry/dog-website:1.0.0
```
