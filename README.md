Demo

**Starting minikube**
```
minikube start
```

**Create deployment**
```
kubectl create -f ./k8s/deployment.yml
```

**Check if pods is running**
```
kubectl get pods
kubectl describe pod <podname>
```

**Create service**
```
kubectl create -f ./k8s/service.yml
```

**Check Exposed IP address & ports**
```
minikube service quotes-service --url
```
# Cleanups
```
kubectl delete service quotes-service
kubectl delete deployment quotes-deployment
```


