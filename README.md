# 🛍️ Modern Tajerly Platform

A comprehensive **.NET 9** backend API built with **Clean Architecture** principles, engineered for **scalability**, **maintainability**, and **enterprise-grade performance**.

---

## 🎯 Project Overview
A robust Tajerly platform designed to manage online stores, products, and orders with advanced features including **dynamic promotions**, **customer reviews**, and flexible **delivery/payment solutions**.

---

## 🏗️ Architecture Overview

This project follows **Clean Architecture** principles with clear separation of concerns across five distinct layers:

### **Presentation Layer**
- Handles HTTP requests and responses
- API controllers and endpoints
- Request/Response models and validation

### **Service Layer**
- Business logic orchestration
- Service interfaces and implementations
- Cross-cutting concerns management

### **Core Layer**
- **CQRS pattern** implementation with MediatR
- Command and Query handlers
- DTOs and manual mapping
- Advanced filtering and pagination logic

### **Domain Layer**
- Core business entities and aggregates
- Domain enums and value objects
- Business rules and domain logic
- Domain events

### **Infrastructure Layer**
- **Entity Framework Core** database context
- Repository pattern implementations
- External service integrations:
  - **Memory Cache** for performance optimization
  - **Paymob** payment gateway integration
  - **MailKit** email services
  - **Azure Translation API** for multi-language support
- Configuration and data seeding

This architecture ensures **testability**, **maintainability**, and **independence** from frameworks and databases.

---

## 🛠️ Technology Stack

### **Framework & Runtime**
- **.NET 9** - Latest .NET runtime with performance improvements
- **ASP.NET Core Web API** - Modern RESTful API framework

### **Architecture Patterns**
- **Clean Architecture** - Layered architecture with dependency inversion
- **CQRS (Command Query Responsibility Segregation)** - Separation of read and write operations
- **MediatR** - In-process messaging and mediator pattern implementation
- **Repository Pattern** - Data access abstraction

### **Data Access & Persistence**
- **Entity Framework Core** - ORM for database operations
- **SQL Server** - Relational database management system
- **Memory Cache** - In-memory caching for performance

### **Authentication & Security**
- **JWT (JSON Web Tokens)** - Token-based authentication
- **Role-Based Access Control (RBAC)** - Policy-based authorization
- **OAuth 2.0** - Google OAuth integration for social login

### **Validation & Mapping**
- **FluentValidation** - Fluent validation library
- **Data Annotations** - Attribute-based validation
- **Manual Mapping** - Explicit object-to-object mapping

### **Real-Time Communication**
- **SignalR** - Real-time web notifications via WebSockets

### **External Integrations**
- **Paymob** - Payment gateway integration for secure transactions
- **MailKit** - Email service for notifications (order confirmation, password reset, email verification)
- **Azure Translation API** - Multi-language translation services

### **Logging & Monitoring**
- **Serilog** - Structured logging framework

### **Localization**
- **Multi-language support** - Internationalization capabilities

---

## 📦 Database & Key Entities

- **Users:** Admin, Customer, Employee roles  
- **Products & Categories** – Comprehensive product catalog with category organization  
- **Cart & CartItems** – Shopping cart management system  
- **Orders & OrderItems** – Complete order processing workflow  
- **Delivery Types:** Standard, Express, Same Day, Scheduled, Pickup from Branch  
- **Payment Methods:** Cash on Delivery, Cash at Branch, Paymob integration  
- **Shipping Address** – Flexible address management for customers  
- **Reviews** – Customer product feedback and rating system  

---

## ✨ Key Features

### **Product & Catalog Management**
- Complete **CRUD operations** for products, categories, and inventory
- Advanced product search with filtering and sorting
- Category-based product organization
- Stock management and validation

### **Authentication & Authorization**
- **JWT-based authentication** with secure token management
- **Role-based access control** (Admin, Customer, Employee)
- **Google OAuth integration** for social login
- Password reset and email verification workflows

### **Shopping Experience**
- **Shopping cart management** with in-memory caching
- Support for both guest and authenticated users
- Real-time inventory validation
- Dynamic pricing with promotions and discounts

### **Order Management**
- Multi-step checkout process with transaction management
- Order status tracking and updates
- Automated order confirmation emails
- Comprehensive order history

### **Payment & Delivery**
- **Paymob payment gateway** integration for secure online payments
- Multiple payment methods (Cash on Delivery, Cash at Branch, Online Payment)
- Flexible delivery options (Standard, Express, Same Day, Scheduled, Pickup)
- Shipping address management

### **Customer Engagement**
- **Product review system** with ratings and feedback
- **Promotions and discounts** management
- Real-time notifications via SignalR for orders and promotions
- Multi-language support with Azure Translation API

### **Search & Filtering**
- Advanced search capabilities with pagination
- Filter products by category, price range, and ratings
- Sort products by various criteria
- Order filtering and management

### **Email Services**
- Automated email notifications for order confirmations
- Password reset functionality
- Email verification for user accounts

---

## 🔮 Future Enhancements

- Enhanced Analytics Dashboard – Order analytics and reporting  
- Recommendation Engine – AI-powered product recommendations  
- Multi-Vendor Support – Support for multiple sellers on the platform  

---

## 💡 Why This Project Matters

- Modern backend best practices  
- Scalable, maintainable, testable architecture  
- Secure authentication & authorization  
- Integration with real payment gateways  

---

## 🏷 Tags

`.NET 9` `.AspNetCore` `.WebAPI` `.CSharp` `.CleanArchitecture` `.JWT` `.CQRS` `.EFCore` `.FluentValidation` `.Paymob` `.MailKit` `.SignalR` `.Serilog` `.MemoryCache` `.Azure` `.OAuth` `.SoftwareEngineering` `.Tajerly` `.BackendDevelopment` `.MultiLayeredArchitecture` `.DatabaseDesign`
