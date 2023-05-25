# Integratieproject 1 - 2022

## Team Groenpunt

| Name                | Email                                | Gitlab username                                          | Student number |
| ------------------- | ------------------------------------ | -------------------------------------------------------- | -------------- |
| Brian Nys           | <brian.nys@student.kdg.be>           | [@briannice](https://gitlab.com/briannice)               | 0150721-80     |
| Bjorn Streatemans   | <bjorn.straetemans@student.kdg.be>   | [@BjornStreatemans](https://gitlab.com/BjornStraetemans) | 0146185-06     |
| Niels Van Steen     | <niels.vansteen@student.kdg.be>      | [@NielsVanSteen](https://gitlab.com/NielsVanSteen)       | 0145682-85     |
| Sander Verheyen     | <sander.verheyen@student.kdg.be>     | [@SanderVerheyen](https://gitlab.com/SanderVerheyen)     | 0148447-37     |
| Michiel Verschueren | <michiel.verschueren@student.kdg.be> | [@MichielVers](https://gitlab.com/MichielVers)           | 0147310-64     |


## Omschrijving

url van gedeployde website: www.docreview-groenpunt.be (vergeet niet {projectName}/{controller}/{action}

### Aanmelden & registreren
hieronder bij [Ga naar aanvullende informatie](##Aanvullende Informatie) wordt er meer detail gegeven over de seeded root users, en het aanmelden.
Korte overview:
- https://docreview-groenpunt.be/admin/account/login		- Aanmelden voor managers & admins
- https://docreview-groenpunt.be/admin/account/register		- Registreren voor managers & admins
- Om voor een specifiek project te registeren/aanmelden wordt 'admin' vervangen door de externe project name e.g., https://docreview-groenpunt.be/groenpunt/account/login

### Na het aanmelden
- Als manager kom je na het aanmelden op een pagina met alle projecten (voor admin) of alle toegeweze projecten (manager)
- om verder te gaan klik je op de 'details' knop van een project.

### Navigatie
Onze website heeft 3 main navigations {manager navigation, profiel navigatie & breadcrumbs}

Eerst is er de navigatie die enkel zichtbaar is voor managers & admins deze wordt links op de pagina getoond.
Deze navigatie heeft een icoon met daarachter de tekst. Om naar de pagina te gaan druk je op de tekst.
Als het niet duidelijk is welke pagina waarvoor dient kun je over het icoon hoveren, dat geeft meer uitleg over de pagina's

De 2de vorm van navigatie is zichtbaar als je rechtsboven over je profiel hovered. deze navigatie bevat:
- profiel pagina
- logout
- voor managers ook een hyperlink naar 'backoffice' dit kunnen managers gebruiken om van de 'front office' pagina's waar de backoffice navigation niet zichtbaar
  is terug naar de backoffice te gaan.



## Aanvullende informatie

### Het compileren & aanmelden

Om het project lokaal te runnen heb je 2 docker containers nodig: mysql en redis
- docker run -e "MYSQL_ROOT_PASSWORD=Secret123"  -p 3306:3306 -d  mysql
- docker run -p 6379:6379 -d redis

In dotnet\src\UI.MVC\appSettings.Development.json staan de variables van de containers
(het ip adres van redis & mysql, mysql username, wachtwoord, ..)

Als je het project opstart (en launch browser staat aan) zal het project opstarten en een web pagina openen, maar zal de pagina een 404 not found geven. Dit komt doordat de url geen project name, controller & action bevat.

De routing is voor alle pagina's opgesteld uit {projectName}/{controller}/{action}/{id?}, voor project specifieke pagina's is dit de external project name.
voor niet project specifieke pagina's is dit een 'admin'. Deze 'admin' is nergens hardcoded en is te veranderen in src/UI.MVC/Identity/ApplicationsConstants.cs

om aan te melden ga je naar https://docreview-groenpunt.be/admin/account/register/admin/account/login 
(admin is de 'project name' het is het backoffice keyword voor NIET project specifieke project pagina)

### Test users

#### Admin [e-mail - password]
- admin@groenpunt.be	Admin123/

#### Manager [email - password]
- manager.groenpunt@gmail.com	Manager123/

Er wordt dan een verification e-mail gestuurd om uw account te confirmen, deze e-mail komt waarschijnlijk in de spam folder.

### Node.js & Webpack
- npm run build (wordt automatisch gedaan tijdens het compileren)
- npm run watch (live update javascript & css)
