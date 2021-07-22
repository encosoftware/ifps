#!/bin/bash
set -e

/opt/mssql-tools/bin/sqlcmd -S localhost -d $2 -U SA -P Asdf123. -i $1