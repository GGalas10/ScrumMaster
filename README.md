# 🏗️ ScrumMaster Powered by AI

![ScrumMaster](https://img.shields.io/badge/Status-Development-blue?style=flat-square)
![Technology](https://img.shields.io/badge/Technology-.NET%20%20%7C%20Angular%20%7C%20SQL%20Server-purple?style=flat-square)

## 🚀 Overview
**ScrumMaster Powered by AI** is a **SaaS application for Scrum Masters** that helps teams manage Agile workflows, analyze sprint efficiency, and generate automated reports using AI.

This project is **a full microservices-based system**, designed for **small to mid-sized teams** who want to improve their Agile workflow.

---

## 📌 Features
✅ **Sprint & Task Management** (create, update, assign, track progress)  
✅ **AI-powered Sprint Analysis** (bottleneck detection, efficiency insights)  
✅ **Automatic Sprint Report Generation** (PDF reports for retrospectives)  
✅ **User Authentication & Authorization** (JWT, OAuth)  
✅ **Microservices Architecture** (Scalable & independent services)  
✅ **WebSockets & SignalR for real-time updates**  

---

## 🛠️ Technologies
- **Backend:** ASP.NET Core (Microservices)  
- **Frontend:** Angular 18+  
- **Database:** SQL Server, Entity Framework Core  
- **Authentication:** JWT, OAuth2  
- **Communication:** REST API, gRPC, WebSockets  
- **Infrastructure:** Azure Cloud (optional), Docker, Kubernetes  
- **Testing:** xUnit, Moq, Playwright

# 📌 Struktura projektu ScrumMaster

The project is based on a microservices architecture, divided into several main modules:

ScrumMaster
│
├── ScrumMaster.Identity
│   ├── ScrumMaster.Identity.Core
│   ├── ScrumMaster.Identity.Infrastructure
│   ├── ScrumMaster.Identity.Tests
│
├── ScrumMaster.Sprints
│   ├── ScrumMaster.Sprints.Core
│   ├── ScrumMaster.Sprints.Infrastructure
│   ├── ScrumMaster.Sprints.Infrastructure.Tests
│
├── ScrumMaster.Tasks
│   ├── ScrumMaster.Tasks.Core
│   ├── ScrumMaster.Tasks.Infrastructure
│   ├── ScrumMaster.Tasks.Tests

📌 **The project is under development - more features coming soon!**
