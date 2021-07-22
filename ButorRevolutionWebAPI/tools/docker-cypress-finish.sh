#!/bin/bash
set -e
mkdir $1/cypress
docker cp ifps.sales.cypress:/cypress/screenshots $1/cypress/screenshots
docker cp ifps.sales.cypress:/cypress/results $1/cypress/results