.PHONY: build
build:
	docker-compose build resident-vulnerabilities-data-pipeline

.PHONY: shell
shell:
	docker-compose run resident-vulnerabilities-data-pipeline bash

.PHONY: test
test:
	docker-compose build resident-vulnerabilities-data-pipeline-test && docker-compose up resident-vulnerabilities-data-pipeline-test

.PHONY: lint
lint:
	-dotnet tool install -g dotnet-format
	dotnet tool update -g dotnet-format
	dotnet format
