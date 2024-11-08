# Kartverket - programmeringsprosjekt - Gruppe 15

## Gjennomgang av applikasjonen:



## Hvordan bruke apllikasjonen 

## Forutseninger: ##

Webapplikasjonen bruker to Docker Containere for å kjøre.
En for webapplikasjonen og en for databasen. For å kjøre applikasjonen må du ha [Docker](https://www.docker.com/) installert på systemet ditt. 

## 1. For å starte applikasjonen: 

For å gå inn applkasjonmappen cd KartApplication 

### Bygg og start Docker Container med webapplikasjon: ###
   
    1. docker image build -t webapp . 
    2. docker container run --rm -it -d --name webapp --publish 80:80 webapp

## 2. For å starte databasen : 
    
    1. Denne koden brukes til å opprette en databasecontainer.
    | Bash (Mac and Linux) | Powershell (Windows) |
    | docker run --rm --name mariadb -p 3308:3306/tcp -v 
    | "$(pwd)/database":/var/lib/mysql -e MYSQL_ROOT-PASSWORD=12345 -d maiadb:10.5.11 | docker run --rm --name mariadb -p 3308:3306/tcp -v "%cd%\database":/var/lib/mysql -e MYSQL_ROOT_PASSWORD=12345 -d mariadb:10.5.11 |
    <br>
     Prosjektet kan koble til databasen. For at proskejtet skal kunne koble til databasen, ble Dockerfile redigert og ApplicationDbContext.cs-filen  oppdatert. 
