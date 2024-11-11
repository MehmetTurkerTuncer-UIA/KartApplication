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

 ### 1. Denne koden brukes til å opprette en databasecontainer.



<div style="width: 100%; overflow-x: auto;">
  <table style="width: 100%; border-collapse: collapse;">
    <thead>
      <tr>
        <th style="border: 1px solid black; padding: 10px; text-align: center; vertical-align: middle;">Bash (Mac and Linux)</th>
        <th style="border: 1px solid black; padding: 10px; text-align: center; vertical-align: middle;">Powershell (Windows)</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td style="border: 1px solid black; padding: 10px; text-align: center; vertical-align: middle;">
          <pre style="text-align: left;">docker run --rm --name mariadb -p 
3308:3306/tcp -v "$(pwd)/database"
:/var/lib/mysql -e MYSQL_ROOT_PASSWORD=12345 
-d mariadb:10.5.11</pre>
        </td>
        <td style="border: 1px solid black; padding: 10px; text-align: center; vertical-align: middle;">
          <pre style="text-align: left;">docker run --rm --name mariadb -p 
3308:3306/tcp -v "%cd%\database"
:/var/lib/mysql -e MYSQL_ROOT_PASSWORD=12345 
-d mariadb:10.5.11</pre>
        </td>
      </tr>
    </tbody>
  </table>
</div>

     
 Prosjektet kan koble til databasen. For at proskejtet skal kunne koble til databasen, ble Dockerfile redigert og ApplicationDbContext.cs-filen  oppdatert. 

 ### 2. Gå inn i databasen og lag databasen og tabellene:
        1. docker exec -it mariadb mysql -p.
        2. Når du blir bedt om det skriv passordet (12345).

## Test ut koden på: 
   https://localhost:5103/
  
# Navigering på nettsiden

## Ved gå inn på nettsiden kan bruke velge mellom:
   1. Søk(Hjem): Her kan du følge etter saken din med et gyldig referansenummer.
   2. Logg inn: Her kan du logge inn til din konto.
   3. Registrer deg: Her kan du registrere deg med din e-post adresse.
   4. Gjest: Her kan du opprette en sak uten å logge inn.

## Rollebasert Navigering:
   1. Admin

      - Hjem: Her er det en oversikt over alle typer brukere. 
      - Legge til ansatt: Legg til nye ansatter i systemet.
      - Oppdater: Oppdater brukerinformasjon.
      - Slett: Fjern bruker fra systemet.

   2. Saksbehandler

     - Hjem: Tilgang til saksbahandlerens dashboard.
     - Profil: Vis og redigere din profil. 
     - Aktive Saker: Se pågående saker.
     - Vis Sak: Vis detaljert informasjon om en spesifikk sak.
     - Administer Sak: Administrer, tilordne og oppdater saksdetaljer.
     - Ferdige Saker: Få tilgang til fullførte saker.
     - Vis Sak: Se detaljer om fuølførte saker.
     - Administrer Sak: Gjennomgå, oppdater og avslutt fullførte saker.
  
  3. Kontrolleren
    
     - Hjem: Tilgang til kontrollerens dashbord.
     - Vis Sak: Se saker som er tilordnet for godkjenning.
     - Avvis: Avvis saker som ikke oppfyller godkjenningskrevene.
     - Godkjenn: Godkjenn saker.

  4. Brukeren
       <pre>
     - Hjem: Tilgang til brukerdashbordet.
     - Mine Saker: Se saker sendt inn av deg.
     - Profil: Rediger dine profildetaljer.
     - Opprette Sak: Her kan du opprette en ny sak med detaljer.
       - Marker et punkt i kartet.
       - Velg mellom forskjellige typer kartgrunnlag.
       - Skrive en beskrivelse.
       - Send inn saken.
       - Skriv ut kvittering.
       </pre>
  5. Gjest

     - Hjem: Begrenset tilgang til hovedsiden.
     - Opprette Sak: Her kan du sende inn en sak uten brukerregistering.
       - Marker et punkt i kartet.
       - Velg mellom forskjellige typer kartgrunnlag.
       - Skrive en beskrivelse.
       - Send inn saken.
       - Skriv ut kvittering.
