# Project Setup and Run Instructions

## 1. Install Docker

- Download and install Docker from [here](https://www.docker.com/products/docker-desktop).

- Verify installation by running the following command:

  ```bash
  docker --version
  ```

## 2. Start the Database with Docker Compose

- In the project root folder, run the following command to start the database:

  ```bash
  docker-compose up -d
  ```

  This will start the database container in the background.

- To stop the database and remove the containers and volumes, run:

  ```bash
  docker-compose down -v
  ```

- Connect to MySQL Database

  - **Host**: `localhost`
  - **Port**: `3306`
  - **Database**: `plantstore`
  - **Username**: `plantstore`
  - **Password**: `plantstore`

- You should now be connected to the MySQL database and able to browse the database schema and execute SQL queries.

## 3. Install Project Dependencies

- Restore the project dependencies:

  ```bash
  dotnet restore
  ```
