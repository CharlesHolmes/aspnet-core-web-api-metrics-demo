#!/bin/bash

node helloworld.js 2>&1 &
while true
do
	k6 run simulate_load.js 2>&1
done
