# PlantPOS - Plant Store Management System

A comprehensive Point of Sale (POS) system designed specifically for plant stores, featuring modern desktop application with AI integration for plant care consultation.

## 🎬 Demo Video

<div align="center">

[![PlantPOS Demo](https://img.youtube.com/vi/oViAZzM5dco/0.jpg)](https://www.youtube.com/watch?v=oViAZzM5dco)

</div>

## 🏗️ Project Structure

```
csc13001-plant-pos/
├── backend/                    # Spring Boot API Server
│   ├── src/main/java/         # Java source code
│   └── docker-compose.yml     # Database & MinIO setup
├── frontend/                  # WinUI3 Desktop Application
│   └── csc13001-plant-pos/   # Main frontend project
├── docs/                     # Documentation & Screenshots
└── README.md
```

## 🛠️ Tech Stack

### Backend

![Java][Java] ![Spring Boot][Spring Boot] ![MySQL][MySQL] ![Redis][Redis] ![MinIO][MinIO] ![Docker][Docker] ![Swagger][Swagger]

### Frontend

![C#][C#] ![WinUI3][WinUI3] ![.NET][.NET]

### External Services

![Gemini AI][Gemini] ![Gmail][Gmail]

### Development Tools

![Maven][Maven] ![Visual Studio][Visual Studio] ![Git][Git]

<!-- Tech Stack Badges -->

[Java]: https://img.shields.io/badge/Java_17-ED8B00?style=for-the-badge&logo=openjdk&logoColor=white
[Spring Boot]: https://img.shields.io/badge/Spring_Boot-6DB33F?style=for-the-badge&logo=spring-boot&logoColor=white
[MySQL]: https://img.shields.io/badge/MySQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white
[Redis]: https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis&logoColor=white
[MinIO]: https://img.shields.io/badge/MinIO-C72E49?style=for-the-badge&logo=minio&logoColor=white
[Docker]: https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white
[Swagger]: https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black
[C#]: https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white
[WinUI3]: https://img.shields.io/badge/WinUI_3-0078D4?style=for-the-badge&logo=microsoft&logoColor=white
[.NET]: https://img.shields.io/badge/.NET_8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white
[Gemini]: https://img.shields.io/badge/Gemini_AI-8E75B2?style=for-the-badge&logo=google&logoColor=white
[Gmail]: https://img.shields.io/badge/Gmail-EA4335?style=for-the-badge&logo=gmail&logoColor=white
[Maven]: https://img.shields.io/badge/Maven-C71A36?style=for-the-badge&logo=apache-maven&logoColor=white
[Visual Studio]: https://img.shields.io/badge/Visual_Studio-5C2D91?style=for-the-badge&logo=visual-studio&logoColor=white
[Git]: https://img.shields.io/badge/Git-F05032?style=for-the-badge&logo=git&logoColor=white

## 📱 Features & Screenshots

### Authentication & User Management

<div align="center">
<img src="docs/Login.png" width="500" alt="Login Interface">
</div>

- Secure login with credential storage
- OTP-based password recovery
- Role-based access control (Admin/Staff)

### Sales Management

<div align="center">
<img src="docs/Sale.png" width="500" alt="Sales Interface">
</div>

- Product selection and quantity management
- Customer membership integration
- Real-time discount application
- PDF invoice generation

### Admin Dashboard

<div align="center">
<img src="docs/AdminDashBoard.png" width="500" alt="Admin Dashboard">
</div>

- Revenue analytics with charts
- Top-selling products tracking
- Inventory alerts
- Excel report exports

### Product Management

<div align="center">
<img src="docs/ProductList.png" width="500" alt="Product Management">
</div>

- Comprehensive product catalog
- Advanced search and filtering
- Category management
- Stock level monitoring

### Customer Management

<div align="center">
<img src="docs/Profile.png" width="500" alt="Customer Profile">
</div>

- Member registration and profiles
- Purchase history tracking
- Loyalty program integration

### Order Management

<div align="center">
<img src="docs/OrderManagement.png" width="500" alt="Order Management">
</div>

- Complete order tracking
- Transaction history
- Payment processing

### AI Chat Assistant

<div align="center">
<img src="docs/Chatbox.png" width="500" alt="AI Chatbot">
</div>

- Plant care consultation
- Product recommendations
- Business insights
- Multi-language support

### Invoice System

<div align="center">
<img src="docs/Invoice.png" width="500" alt="Invoice System">
</div>

- Professional invoice templates
- PDF export functionality
- Print integration
- Transaction records

### Email Notifications

<div align="center">
<img src="docs/Email.png" width="500" alt="Email System">
</div>

- Automated daily reports
- Inventory alerts
- Customer notifications

## 🛠️ Installation & Setup

### Prerequisites

- **Java 17+**
- **Maven**
- **Docker & Docker Compose**
- **.NET 8 SDK**
- **Visual Studio** (with WinUI workload)

### 1. Clone Repository

```bash
git clone https://github.com/Npeka/csc13001-plant-pos.git
cd csc13001-plant-pos
```

### 2. Configure Backend

Create your application.properties file from the template:

```bash
cd backend/src/main/resources
cp application.properties.template application.properties
```

Edit `application.properties` and replace the following placeholders:

- `YOUR_JWT_SECRET_KEY_HERE` - Generate a secure JWT secret key
- `YOUR_EMAIL@gmail.com` - Your Gmail address for notifications
- `YOUR_EMAIL_APP_PASSWORD` - Gmail app password (not your regular password)
- `YOUR_GEMINI_API_KEY_HERE` - Your Google Gemini API key

**How to get required API keys:**

1. **Gmail App Password**: Go to Google Account Settings → Security → 2-Step Verification → App passwords
2. **Gemini API Key**: Visit [Google AI Studio](https://makersuite.google.com/app/apikey) to generate your key

### 3. Configure Frontend

Create your appsettings.json file from the template:

```bash
cd frontend/csc13001-plant-pos/csc13001-plant-pos
cp appsettings.template.json appsettings.json
```

Edit `appsettings.json` and replace the following placeholders:

- `YOUR_EMAIL@gmail.com` - Your Gmail address for notifications
- `YOUR_EMAIL_APP_PASSWORD` - Gmail app password (same as backend)
- `YOUR_SYNCFUSION_LICENSE_KEY` - Your Syncfusion license key (optional, for UI components)
- `YOUR_ENCRYPTION_KEY_HERE` - Custom encryption key for secure data storage

### 4. Start Backend Services

```bash
cd backend
docker-compose up -d
mvn spring-boot:run
```

### 5. Run Frontend Application

```bash
cd csc13001-plant-pos-frontend
dotnet restore
# Open in Visual Studio and run (F5)
```

### 6. Access Services

- **Application**: Run from Visual Studio
- **API Documentation**: http://localhost:8080/swagger-ui/index.html
- **MinIO Console**: http://localhost:9001 (plantpos/plantpos)
- **Database**: localhost:33306 (plantpos/plantpos)

## 🏛️ System Architecture

<div align="center">

### Database Schema

<img src="docs/Database.png" width="700" alt="Database Schema">

### Component Architecture

<img src="docs/ComponentDiagram.jpg" width="700" alt="Component Diagram">

</div>

The system follows a clean architecture pattern with:

- **Frontend**: WinUI3 with MVVM pattern
- **Backend**: Spring Boot with layered architecture
- **Database**: MySQL with optimized schemas
- **Storage**: MinIO for scalable file management
- **Cache**: Redis for performance optimization
