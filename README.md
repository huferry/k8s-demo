Demo

**Starting minikube**
```
minikube start
```

**Create deployment**
First create the RabbitMQ instance.
```
kubectl create -f ./k8s/01-queue-deployment.yml
```

**Check if pods is running**
```
kubectl get pods
kubectl describe pod <podname>
```

**Create service**
Create the service of RabbitMQ, exposing the management client to simplify testing.
```
kubectl create -f ./k8s/02-queue-service.yml
```

**Check Exposed IP address & ports**
The first one will be the AMQP port and the second one the management page.
```
minikube service quotes-amqp-service --url
```

**Create the feeder**
When the feeder is runing, the queue will be filled with quotes.
```
kubectl create -f ./k8s/03-feeder-deployment.yml
```

# Cleanups
```
kubectl delete deployment quotefeeder-deployment
kubectl delete service quotes-amqp-service
kubectl delete deployment quotes-amqp-deployment
```


