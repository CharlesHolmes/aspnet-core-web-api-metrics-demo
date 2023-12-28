#!/bin/bash

sudo yum install -y https://dl.k6.io/rpm/repo.rpm 2>&1
sudo yum install -y --nogpgcheck k6 2>&1