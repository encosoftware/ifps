IFPS konténerek nevei és url-jei:

Db:
- ifps.sqlserver
- localhost:1433
- user: sa
- password: Asdf123.

Sales API:
- ifps.sales.api
- http://localhost:5020/swagger/

Sales UI:
- ifps.sales.ui
- http://localhost:4201/login

Webshop UI:
- ifps.webshop.ui
- http://localhost:4221

Factory API:
- ifps.factory.api
- http://localhost:5030/swagger/

Factory UI:
- ifps.factory.ui
- http://localhost:4211/login

Integration API:
- ifps.integration.api
- http://localhost:5040/swagger/


--------------------------------------------------------------
Parancsok CSAK futtatáshoz: 
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml pull
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml up 

Adatbázisoknak első indításkor erőforrástól függően kell 0.5 - 1 perc mire felállnak és automatikusan felseedelik az adatokat. 

//docker mappából
--------------------------------------------------------------
Parancsok fejlesztéshez:

Összes image buildelése:
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml build

Egyes image-ek buildelése:
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml build <kont?ner n?v 1> <kont?ner n?v 2>

Összes kontoner futtatása:
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml up 
- kil?p?s Ctrl + c

Kontoner logjának listázása
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml logs <kont?ner n?v>

Futó kontonerek leállítása és törlése
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml down

//docker mappából
--------------------------------------------------------------
Ha valami összeborulna:

Futó konténerek listázása:
docker ps 

Összes konténer listázása:
docker ps -a

?Összes konténer leállítása: 
docker stop $(docker ps -aq)

?Összes konténer törlése:
docker container prune -f

Minden törlése: 
docker stop $(docker ps -aq)
docker container prune -f
docker system prune -a -f --volumes

Ha így se megy:
Tálca->Docker Desktop icon -> Jobb click -> Settings -> Reset fül -> Reset to factory defaults