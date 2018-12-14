filter= ""

.PHONY: network
network:
	docker network create shared

.PHONY: restore
restore:
	dotnet restore Accounts.sln

.PHONY: build
build: stop restore
	docker-compose build

.PHONY: up
up:
	docker-compose up -d

.PHONY: stop
stop:
	docker-compose stop

.PHONY: down
down:
	docker-compose down

.PHONY: attach
attach: up
	docker-compose exec accounts.api bash

.PHONY: mysql
mysql: up
	docker-compose exec database mysql -u root -h database -psecret --database accounts

.PHONY: redis
redis:
	docker-compose run redis
    
.PHONY: migrate
migrate: up
	cd accounts.repository.mysql && dotnet ef database update

.PHONY: migrate-script
migrate-script: up
	cd accounts.repository.mysql && dotnet ef migrations script

.PHONY: prune
prune: stop
	docker system prune

.PHONY: release
release:
	git tag -a $(version) -m "$(comment)"
