install:
	mvn clean install -DskipTests

run:
	mvn spring-boot:run



test:
	mvn test

# docker
d-up:
	docker-compose up

d-up-d:
	docker-compose up -d

d-down:
	docker-compose down

d-down-v:
	docker-compose down -v

d-reset:
	docker-compose down -v
	docker-compose up -d