#!/bin/bash

if [ "$1" = "/opt/mssql/bin/sqlservr" ]; then
  if [ ! -f /tmp/app-initialized ]; then
  function initialize_app_database(){

      n=0
      until [ $n -ge 60 ]
      do
        echo "Trying to connect to the server ($n)"
        /opt/mssql-tools/bin/sqlcmd -S localhost -d "master" -U SA -P ${SA_PASSWORD} -Q "select getdate()" && break 
        n=$[$n+1]
        sleep 5
      done

      echo "Initializing database"
      chmod +x /home/sql/dbcommand.sh
      chmod +x /home/sql/genDbInitQuery.sh
      
      echo "Init ${DB_SALES_NAME} database"
      /bin/bash /home/sql/genDbInitQuery.sh ${DB_SALES_NAME} > /home/sql/initdb-sales.sql
      /bin/bash /home/sql/dbcommand.sh "master" "${SA_PASSWORD}" "/home/sql/initdb-sales.sql" || return 
      echo "Seed ${DB_SALES_NAME} database"
      /bin/bash /home/sql/dbcommand.sh "${DB_SALES_NAME}" "${SA_PASSWORD}" "/home/sql/updatedb-sales.sql" || return
      
      echo "Init ${DB_SALES_NAME} database"
      /bin/bash /home/sql/genDbInitQuery.sh ${DB_SALES_HANGFIRE_NAME} > /home/sql/initdb-saleshf.sql
      /bin/bash /home/sql/dbcommand.sh "master" "${SA_PASSWORD}" "/home/sql/initdb-saleshf.sql" || return 

      
      echo "Init ${DB_FACTORY_NAME} database"
      /bin/bash /home/sql/genDbInitQuery.sh ${DB_FACTORY_NAME} > /home/sql/initdb-factory.sql
      /bin/bash /home/sql/dbcommand.sh "master" "${SA_PASSWORD}" "/home/sql/initdb-factory.sql" || return 
      echo "Seed ${DB_FACTORY_NAME} database"
      /bin/bash /home/sql/dbcommand.sh "${DB_FACTORY_NAME}" "${SA_PASSWORD}" "/home/sql/updatedb-factory.sql" || return

      echo "Init ${DB_INTEGRATION_HANGFIRE_NAME} database"
      /bin/bash /home/sql/genDbInitQuery.sh ${DB_INTEGRATION_HANGFIRE_NAME} > /home/sql/initdb-integrationhf.sql
      /bin/bash /home/sql/dbcommand.sh "master" "${SA_PASSWORD}" "/home/sql/initdb-integrationhf.sql" || return 

      touch /tmp/app-initialized
      echo "Database updated"
    }
    initialize_app_database &
  fi
fi


exec "$@"