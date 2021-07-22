USE master
IF EXISTS(select * from sys.databases where name='IFPSSalesDb')
	DROP DATABASE IFPSSalesDb

CREATE DATABASE IFPSSalesDb

GO
	