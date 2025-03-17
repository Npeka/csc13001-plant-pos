# Project Setup and Run Instructions

## 1. Prerequisites

### **1.1 Backend (Java Spring Boot)**

- Install **Java 17** or a compatible version.
- Install **Maven** to manage dependencies.
- Install **Docker** and **Docker Compose** to run the database and MinIO.

### **1.2 Frontend (.NET C# WinUI)**

- Install **.NET SDK 8** (compatible with the project).
- Install **Visual Studio** (with required workloads for C# and WinUI development).

---

## 2. Running the Backend

1. **Navigate to the backend directory:**

```bash
cd backend
```

2. **Start the database and MinIO using Docker Compose:**

```bash
docker-compose up -d
```

This will start the required services in the background.

To stop and remove containers/volumes, run:

```bash
docker-compose down -v
```

3. **Run the Spring Boot application:**

```bash
mvn spring-boot:run
```

---

## 3. Running the Frontend

1. **Navigate to the main frontend project:**

```bash
cd csc13001-plant-pos-frontend
```

2. **Open the project in Visual Studio and run the application.**

---

## 4. Accessing MinIO

1. **Open MinIO Web UI in a browser:**

```
http://localhost:9001
```

2. **Login with default credentials (if not changed in configuration):**

- **Username:** `plantpos`
- **Password:** `plantpos`

3. **Manage and view uploaded files in MinIO.**

---

## 5. Database Connection Details

- **Host:** `localhost`
- **Port:** `33306`
- **Database:** `plantpos`
- **Username:** `plantpos`
- **Password:** `plantpos`

Use any MySQL client to connect and browse the database.
