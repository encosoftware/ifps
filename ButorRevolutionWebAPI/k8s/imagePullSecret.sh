kubectl create secret docker-registry harbor-credential \
     --docker-server=https://registry.encosoft-dev.hu/ \
     --docker-username=svc_build_butor \
     --docker-password=<your-password> \ # $ characters in password must be escaped with \ "...\$..."
     --docker-email=svc_build_butor@encosoft-dev.hu