#!/bin/bash

/opt/mssql-tools/bin/sqlcmd -S localhost -d $1 -U SA -P $2 -i $3
