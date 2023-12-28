#!/bin/bash

aws cloudformation delete-stack --stack-name metrics-demo
aws cloudformation wait stack-delete-complete --stack-name metrics-demo
aws cloudformation delete-stack --stack-name metrics-demo-repos
aws cloudformation wait stack-delete-complete --stack-name metrics-demo-repos