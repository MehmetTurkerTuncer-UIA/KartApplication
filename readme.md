# Kartverket - programmeringsprosjekt - Gruppe 15

## Gjennomgang av applikasjonen:



## Hvordan bruke apllikasjonen 

## Forutseninger: ##
Webapplikasjonen bruker to Docker Containere for å kjøre.
En for webapplikasjonen og en for databasen. For å kjøre applikasjonen må du ha (Docker) (https://www.docker.com/) installert på systemet ditt. 

## 1. For å starte applikasjonen: ##
For å gå inn applkasjonmappen cd KartApplication 'cd KartApplication'

## Bygg og start Docker Container med webapplikasjon:
   
    1. 'docker image build -t webapp . '
    2. 'docker container run --rm -it -d --name webapp --publish 80:80 webapp
    

