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
# Autentisering 
  Prosjektet har et autentiseringssystem som sikrer at brukeren kun kan se knappene som er relevante for dem. 
Prosjektet bruker tabellene AspNetRoll og AspNetUserRoll. I rolletabellen er rollene Admin, Saksbehandler, Kontrolleren og Bruker definert. Disse rollene brukes til å kontrollere hvilke knapper hver bruker har tilgang til.
Kodeeksempelet nedenfor viser hvordan @if … brukes.
        
                @if (User.IsInRole(UserRoles.Role_Admin)) {
                <li><a href="/Admin/index" class="menu-item"><i class="bi bi-folder2"></i> Admin </a></li>
                     }
                            @if (User.IsInRole(UserRoles.Role_Saksbehandler))
                            {
                              
                            <li><a href="/Saksbehandler/index" class="menu-item"><i class="bi bi-folder2"></i> Saksbehandler </a></li>
                            }
                      
                            @if (User.IsInRole(UserRoles.Role_Kontrolleren))
                            {
        
                                <li><a href="/Kontrolleren/index" class="menu-item"><i class="bi bi-folder2"></i> Kontrolleren </a></li>
                            }
                            
                            @if (User.IsInRole(UserRoles.Role_Bruker))
                            {  
        
                                <li><a href="/home/index" class="menu-item"><i class="bi bi-pencil-square"></i> Opprett Saker</a></li>
                                <li><a href="/home/minesaker" class="menu-item"><i class="bi bi-folder2"></i> Mine Saker</a></li>
                            }
        
            </ul>
        
# Autorisering
  For å kontrollere hvilken bruker som har tilgang til hvilke sider og for å forhindre at eksterne personer får tilgang til sidene ved å skrive inn sidens navn direkte i nettleseren, benyttes kontrollene i Controller-filene.

      [Authorize(Roles = UserRoles.Role_Admin )]
     
      [Authorize(Roles = UserRoles.Role_Kontrolleren)]
     
      [Authorize(Roles = UserRoles.Role_Bruker + "," + UserRoles.Role_Admin + "," + UserRoles.Role_Saksbehandler + "," + UserRoles.Role_Kontrolleren)]
     
# Systemarkitektur

 <img src="systemarkitektur.png" alt="Diagram showing system architecture" width="400">


