#K8s Demo

This is a demonstration of the use of Kubernetes to setup a multi-layer application. 

## 1. The Application
As an example we are building an application that displays stock prices from the AEX Index (Amsterdam stock exchange). The application contains 3 parts.

### 1.1 RabbitMQ
We will pull this image directly from the Docker Hub. For testing purposes, we will include the management client of it.

### 1.2 QuoteFeeder
This service will feed the quotes onto the queue. The prices are just a simulation and are not the real prices. The source code is included in this repository. The docker image is also available on Docker hub as `huferry/quote-feeder`.

### 1.3 QuoteView
This service is an HTTP-server that reads the quotes from the queue and sends those to the web-browser. The source code is included in this repository and the docker image is available on Docker hub as `huferry/quote-view`.

## 2. Usage
I assume that Kubernetes is already installed on the machine. In this example, it will use `minikube` and `kubectl`.

**Starting minikube**
Make sure you have the `minikube` started.
```
minikube start
```

**Create The Secrets**
For RabbitMQ, the credentials are:
- username: quotes.user
- password: ToP$3cret!
  
How we get the base64 encoding:
```
echo -n "your secret" | base64
```
Create the secrets on kubernetes:
```
kubectl create -f ./k8s/00-secrets.yml
```

**Create deployment**
First create the RabbitMQ instance. After the deployment is created, an instance of RabbitMQ will run in a pod. This will not available from outside the pod until a service is created.
```
kubectl create -f ./k8s/01-queue-deployment.yml
```

**Check if pods is running**
```
kubectl get pods
kubectl describe pod <podname>
```

**Create service**
Create the service of RabbitMQ, exposing the management client to simplify testing. By creating a service, we expose the ports from outside the pod. The service will have it's own IP address, and the following step shows us how to reveal this IP address.
```
kubectl create -f ./k8s/02-queue-service.yml
```

**Check Exposed IP address & ports**
This will show the IP address/ports exposed. The first one will be the AMQP port and the second one the management page. In some cases, `minikube` is unable to create its own IP address, in this case it will use the `localhost` and choose another port numbers. 
```
minikube service quotes-amqp-service --url
```

**Create the feeder**
When the feeder is runing, the queue will be fed with quotes.
```
kubectl create -f ./k8s/03-feeder-deployment.yml
```

**Cleanups**
```
kubectl delete deployment quotefeeder-deployment
kubectl delete service quotes-amqp-service
kubectl delete deployment quotes-amqp-deployment
kubectl delete secret quotes-secrets
```


