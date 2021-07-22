#!/bin/bash
set -e

docker exec -i sqlserver mkdir -p "/home/sql/"
echo "created directory"

docker cp ./dbcommand.sh sqlserver:/home/sql/dbcommand.sh
docker cp ./sales.initdb.sql sqlserver:/home/sql/initdb.sql
docker cp ./updatedb.sql sqlserver:/home/sql/updatedb.sql
echo "copied files"

docker exec -i sqlserver chmod +x /home/sql/dbcommand.sh
echo "granted permission"

docker exec -i sqlserver /bin/bash /home/sql/dbcommand.sh "/home/sql/initdb.sql" "master"
echo "cleared database"

docker exec -i sqlserver /bin/bash /home/sql/dbcommand.sh "/home/sql/updatedb.sql" "IFPSSalesDb"
echo "updated database"