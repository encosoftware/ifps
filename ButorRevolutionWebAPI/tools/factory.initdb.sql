USE master
IF EXISTS(select * from sys.databases where name='IFPSSalesDb')
	DROP DATABASE IFPSFactoryDb

CREATE DATABASE IFPSFactoryDb

GO
	