Clear-Host;

$containers = $(docker container ps -q);

if ( $null -ne $containers ) {
    docker kill $containers
}

docker container prune --force    
docker volume prune -f  
docker network prune -f  

#wsl --shutdown
# wsl -l -v
