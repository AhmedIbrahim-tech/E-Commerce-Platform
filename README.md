# 🚀 E-Commerce Web API

E-Commerce backend API built with **.NET 9** and **Clean Architecture**, designed for **scalability**, **maintainability**, and **high performance**.

---

## 🎯 Project Goal
Build a smart platform for managing online stores, products, and orders, with interactive customer engagement features like **promotions**, **reviews**, and multiple **delivery/payment options**.

---

## 🏗 Architecture & Layers

- **Presentation Layer** – Handles API requests & responses  
- **Service Layer** – Business logic & service orchestration  
- **Core Layer** – Commands, Queries, Mapping, and Filters  
- **Domain Layer** – Entities, Enums, and Business Rules  
- **Infrastructure Layer** – Database context, Configuration, Data Seeding, Repositories, External Integrations (Distributed SQL Cache, Paymob, Email, Flaunt API)  

This layered design ensures **separation of concerns**, **testability**, and **maintainability**.

---

## 🛠 Tech Stack

- **.NET 9 Web API** – Clean Architecture  
- **Entity Framework Core + SQL Server**  
- **CQRS + MediatR** – Commands & Queries separation  
- **FluentValidation + Data Annotations** – Strong input validation  
- **JWT Authentication** – Role & Policy-based access  
- **AutoMapper** – DTO ↔ Entity mapping  
- **Distributed SQL Server Cache** – Cart management and caching  
- **Paymob Integration** – Secure payment processing  
- **MailKit** – Confirm Email, Reset Password, Order Confirmation  
- **Serilog** – Structured logging  
- **Localization** – Multi-language support  

---

## 📦 Database & Key Entities

- **Users:** Admin, Customer, Employee  
- **Products & Categories** – Product management, reviews, and category organization  
- **Cart & CartItems** – Shopping cart management  
- **Orders & OrderItems** – Order processing  
- **Delivery Types:** Standard, Express, Same Day, Scheduled, Pickup from Branch  
- **Payment Methods:** Cash on Delivery, Cash at Branch, Paymob  
- **Shipping Address** – Flexible address management  
- **Reviews** – Customer product feedback  

---

## ⚡ Key Features (Implemented)

- CRUD for Products & Categories  
- Promotions & Discounts – Interactive offers  
- Secure User Authentication – JWT with roles & policies  
- Cart & Order Management – Multi-step checkout  
- Advanced Search & Filtering – Products & orders  
- Reviews – Customer feedback for products  

---

## 🔮 Future Enhancements

- Real-Time Notifications – For orders & promotions  
- Google OAuth Login – For seamless social authentication  

---

## 💡 Why This Project Matters

- Modern backend best practices  
- Scalable, maintainable, testable architecture  
- Secure authentication & authorization  
- Integration with real payment gateways  

---

## 🏷 Tags

`.NET 9` `.AspNetCore` `.WebAPI` `.CSharp` `.CleanArchitecture` `.JWT` `.CQRS` `.EFCore` `.FluentValidation` `.Paymob` `.MailKit` `.Serilog` `.DistributedCache` `.SoftwareEngineering` `.ECommerce` `.BackendDevelopment` `.MultiLayeredArchitecture` `.DatabaseDesign`
