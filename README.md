# budget4home API [![action](https://github.com/lfmachadodasilva/budget4home-api/actions/workflows/main.yml/badge.svg)](https://github.com/lfmachadodasilva/budget4home-api/actions/workflows/main.yml)

Welcome to my personal project to control budget for home.

### How to run:

run/start postgres database using docker:

> sh ./scripts/docker-postgres.sh

run project:

> dotnet run --project budget4home

#### or

> docker-compose up

### Open:

- [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)
- [https://localhost:5001/swagger/index.html](https://localhost:5001/swagger/index.html)

### How to run tests:

> dotnet test
