#!/bin/bash

echo "USE master
IF EXISTS(select * from sys.databases where name='$1')
DROP DATABASE $1

CREATE DATABASE $1

GO" 
