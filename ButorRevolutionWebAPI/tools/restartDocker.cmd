:Reset Docker and Containers
@echo off
cls
pushd %~dp0

echo Wait for Docker to finish start...
timeout /t 150
docker ps -a 

:STOP
echo.
echo.
echo Stopping Docker...
net stop com.docker.service
taskkill /im "Docker*" /f

:START
echo.
echo Starting Docker...
net start com.docker.service
start "" "C:\Program Files\Docker\Docker\Docker Desktop.exe"
echo.

echo Wait for Docker to finish start...
:WAIT
tasklist /fi "imagename eq Docker Desktop.exe" |find ":" > nul
if %ERRORLEVEL%==0 goto WAIT
timeout /t 75

:SHOW
echo.
docker ps -a

:END
popd
echo.