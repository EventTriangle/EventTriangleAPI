echo "========================================"
echo "Docker client/server"
echo "========================================"
docker version

echo
echo "========================================"
echo "Docker daemon configuration"
echo "========================================"
docker info

echo
echo "========================================"
echo "Docker Buildx"
echo "========================================"
docker buildx version
docker buildx ls
docker buildx inspect --bootstrap

echo
echo "========================================"
echo "Docker context"
echo "========================================"
docker context ls
docker context show
docker context inspect

echo
echo "========================================"
echo "Docker environment"
echo "========================================"
env | grep -Ei '^(DOCKER|BUILDKIT|HTTP_PROXY|HTTPS_PROXY|NO_PROXY|http_proxy|https_proxy|no_proxy)=' || true

echo
echo "========================================"
echo "Docker daemon connection"
echo "========================================"
echo "DOCKER_HOST=${DOCKER_HOST:-<not set>}"
echo "DOCKER_TLS_CERTDIR=${DOCKER_TLS_CERTDIR:-<not set>}"

echo
echo "========================================"
echo "DNS"
echo "========================================"
cat /etc/resolv.conf
getent hosts docker || true

echo
echo "========================================"
echo "Docker disk usage"
echo "========================================"
docker system df
docker system df -v

echo
echo "========================================"
echo "Containers"
echo "========================================"
docker ps -a

echo
echo "========================================"
echo "Images"
echo "========================================"
docker images --digests

echo
echo "========================================"
echo "Networks"
echo "========================================"
docker network ls
