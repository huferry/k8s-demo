# Demo 01 - Pod

**Create pod**
```shell
kubectl create -f ./dog-app.yml
```

**Check pod**
```shell
kubectl get pod
```

**Describe pod**
```shell
kubectl describe pod dog-app-demo
```

**Port forwarding**
```shell
kubectl port-forward dog-app-demo 3001:3000
```