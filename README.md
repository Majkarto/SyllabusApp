# SyllabusApp

## Uruchomienie w Dockerze

```
docker compose up -d --build
```

| Usługa | Adres |
|---|---|
| Frontend (Blazor, nginx) | http://localhost:8080 |
| API | http://localhost:8081 (Scalar UI: `/scalar`) |
| Health check (monitoring) | http://localhost:8081/health |
| PostgreSQL | localhost:5432 |

Konto admina: `admin@syllabus.local` / `Admin1234`.
Stan kontenerów (`healthy`/`unhealthy`): `docker compose ps`.

## Testy

```
dotnet test
```

## Dokumentacja

Sprawozdanie w LaTeX: `docs/main.tex` (gotowy PDF: `docs/main.pdf`).
Kompilacja (wymaga biber): `latexmk -pdf main.tex` w katalogu `docs/`.
