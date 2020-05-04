#!/bin/bash
kubectl apply \
    -f ./rabbitmq.yaml \
    -f ./sqlserver.yaml 
