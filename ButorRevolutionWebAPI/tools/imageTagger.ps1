$newtag="tender_final"

docker pull registry.encosoft-dev.hu/butorrevolution/sqlserver:latest
docker tag registry.encosoft-dev.hu/butorrevolution/sqlserver:latest "registry.encosoft-dev.hu/butorrevolution/sqlserver:$($newtag)"
docker push "registry.encosoft-dev.hu/butorrevolution/sqlserver:$($newtag)"

docker pull registry.encosoft-dev.hu/butorrevolution/sales-ui:latest
docker tag registry.encosoft-dev.hu/butorrevolution/sales-ui:latest "registry.encosoft-dev.hu/butorrevolution/sales-ui:$($newtag)"
docker push "registry.encosoft-dev.hu/butorrevolution/sales-ui:$($newtag)"

docker pull registry.encosoft-dev.hu/butorrevolution/webshop-ui:latest
docker tag registry.encosoft-dev.hu/butorrevolution/webshop-ui:latest "registry.encosoft-dev.hu/butorrevolution/webshop-ui:$($newtag)"
docker push "registry.encosoft-dev.hu/butorrevolution/webshop-ui:$($newtag)"

docker pull registry.encosoft-dev.hu/butorrevolution/factory-ui:latest
docker tag registry.encosoft-dev.hu/butorrevolution/factory-ui:latest "registry.encosoft-dev.hu/butorrevolution/factory-ui:$($newtag)"
docker push "registry.encosoft-dev.hu/butorrevolution/factory-ui:$($newtag)"

docker pull registry.encosoft-dev.hu/butorrevolution/sales-api:latest
docker tag registry.encosoft-dev.hu/butorrevolution/sales-api:latest "registry.encosoft-dev.hu/butorrevolution/sales-api:$($newtag)"
docker push "registry.encosoft-dev.hu/butorrevolution/sales-api:$($newtag)"

docker pull registry.encosoft-dev.hu/butorrevolution/factory-api:latest
docker tag registry.encosoft-dev.hu/butorrevolution/factory-api:latest "registry.encosoft-dev.hu/butorrevolution/factory-api:$($newtag)"
docker push "registry.encosoft-dev.hu/butorrevolution/factory-api:$($newtag)"

docker pull registry.encosoft-dev.hu/butorrevolution/integration-api:latest
docker tag registry.encosoft-dev.hu/butorrevolution/integration-api:latest "registry.encosoft-dev.hu/butorrevolution/integration-api:$($newtag)"
docker push "registry.encosoft-dev.hu/butorrevolution/integration-api:$($newtag)"
pause