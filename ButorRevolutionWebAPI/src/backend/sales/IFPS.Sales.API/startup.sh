#!/bin/bash
if ! [ -d "AppData" ]; then
  mkdir AppData
fi
if ! [ "$(ls -A AppData)" ]; then
     echo "Coping files to AppData..."
fi
cp -r AppData_default/* AppData
dotnet IFPS.Sales.API.dll