#!/bin/bash
set -e

echo "Stopping all container:"
cd "$(dirname "$0")"
RUNNINGCONTAINERS=$(docker container ls -a -q)
if [ ! -z "$RUNNINGCONTAINERS" ]
then
	docker container stop $RUNNINGCONTAINERS
fi

#leave containers for cache
#echo "Remove all container"
#docker container prune -f

echo "Removing unnecessary images:"
docker images | grep "karpinszkyandras"
docker rmi -f $(docker images | grep "karpinszkyandras" | awk '{print $3}') 

#docker system prune -a -f --volumes