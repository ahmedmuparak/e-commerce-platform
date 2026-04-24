# 🛒 E-Commerce API (.NET)

A robust and scalable **E-Commerce Web API** built using **ASP.NET Core**, following clean architecture principles and modern best practices.

---

## 🚀 Features

* 🔐 Authentication & Authorization (JWT)
* 👤 User Registration & Login
* 🛍️ Product Management
* 📂 Category Management
* ❤️ Favorites (Wishlist)
* 🛒 Shopping Cart
* 📦 Order Processing
* 📍 User Address Management
* 🔑 Role-Based Access (Admin / User)
* 📄 Swagger API Documentation

---

## 🏗️ Architecture

The project follows **Clean Architecture** with clear separation of concerns:

Ecommerce.API           --> Presentation Layer (Controllers)
Ecommerce.Application   --> Business Logic (Services, Interfaces, DTOs)
Ecommerce.Infrastructure--> Data Access (Repositories, DbContext)
Ecommerce.Domain        --> Core Entities


---

## 🛠️ Technologies Used

* ASP.NET Core Web API (.NET 9)
* Entity Framework Core
* SQL Server
* ASP.NET Identity
* JWT Authentication
* LINQ
* AutoMapper
* Repository Pattern
* Dependency Injection
* Swagger (OpenAPI)

---

## 🔐 Authentication

The API uses **JWT Tokens** for secure authentication.



---

## 📌 API Endpoints Overview

### 🔑 Authentication

POST/api/Authentication/login

POST/api/Authentication/register

GET/api/Authentication/EmailExists

GET/api/Authentication/CurrentUser

GET/api/Authentication/address

PUT/api/Authentication/address

### 📦 Products

GET/api/Product

POST/api/Product

GET/api/Product/{id}

PUT/api/Product/{id}

DELETE/api/Product/{id}

### 📂 Categories


GET/api/Category/{id}

PUT/api/Category/{id}

DELETE/api/Category/{id}

POST/api/Category

GET/api/Category

### ❤️ Favorites

GET /api/favoritelist

POST /api/favoritelist

DELETE /api/favoritelist/{id}

### 🛒 Cart

GET/api/cart

POST/api/cart/add

PUT/api/cart/update

DELETE/api/cart/{cartItemId}

DELETE/api/cart/clear

### 📦 Orders

POST/api/Order

GET/api/Order

GET/api/Order/delivery-methods

GET/api/Order/{id}



