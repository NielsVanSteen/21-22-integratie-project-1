# Docreview DOTNET web application

## Introduction
The PDF file included in the root of this repo gives an in detail explanation of our application.

## Images

---

### Banners

**/banners/{project-id}\_{1920|640}.{extension}**

- Each project has one banner
- Widths:
  - 1920
  - 640

### Logos

**/logos/{project-id}/{guid}\_{256}.{extension}**

- Each project has multiple project logos
- Widths:
  - 256

### Profile pictures

**/users/{user-id}\_{256}.{extension}**

- Each user has a profile picture
- Widths:
  - 256

### Docreview images

**/docreviews/{project-id}/{guid}\_{1920|640}.{extension}**

- Each docreview text can contain multiple images
- Widths
  - 1920
  - 640

## Local development

### Docker

---

Create docker MySQL database.

```bash
docker run -e "MYSQL_ROOT_PASSWORD=Secret123"  -p 3306:3306 -d  mysql
```

Create docker Redis memorycache.

```bash
docker run -p 6379:6379 -d redis
```

List all containers.

```bash
docker container ls
```

Start docker container for databse or memorycache

```bash
docker start <container-id>
```

### Migrations

---

Install dotnet migrations tool for Entity Framework Core.

```bash
dotnet tool install dotnet-ef --version 6.0.0 --global
```

Adding a new migration. Must be done from inside DAL project folder.

```bash
dotnet ef migrations add initial-migration --startup-project ../UI.MVC
```

Update the databse according to the current migrations. Must be done from inside DAL project folder.

```bash
dotnet ef database update --startup-project ../UI.MVC
```

Enkel voor niels

```bash
dotnet ef database update --startup-project ../UI.MVC -- --environment Development.Niels
```

Undo the last migration. Must be done from inside DAL project folder.

```bash
dotnet ef migrations remove --startup-project ../UI.MVC
```

Delete the database. Must be done from inside DAL project folder.

```bash
dotnet ef database drop --startup-project ../UI.MVC -f
```
