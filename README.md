# ✅ Full-Stack Todo App (Dockerized, Deployed)

A production-style full-stack Todo application built with:

- **Frontend:** React (served via Nginx) → Deployed on Vercel
- **Backend:** .NET Web API → Deployed on Render (Docker)
- **Database:** PostgreSQL → Managed by Render
- **Local Dev:** Docker Compose (React + .NET + Postgres)

This project demonstrates:

- Clean architecture in .NET
- Dockerized full-stack setup
- Cloud deployment (Render + Vercel)
- Persistent database with EF Core migrations

---

## 🌐 Live URLs

### 🔹 Frontend (Vercel)

👉 https://todo-frontend-docker.vercel.app/

### 🔹 Backend API (Render)

👉 https://todo-fullstack-docker.onrender.com/swagger/index.html

Render dashboard:  
👉 https://dashboard.render.com/web/srv-d6ahknbh46gs738efp90/events

### 🔹 PostgreSQL (Render – postgres DB)

👉 https://dashboard.render.com/d/dpg-d69u4n0gjchc73cn27rg-a

> ⚠️ **Note:** The backend runs on Render’s free tier. The first request may be slow due to cold start.

---

## 🏗️ Architecture

```
User (Browser)
      |
      v
Frontend (React + Nginx)  ───────▶  Backend (.NET API)
   (Vercel)                            (Render - Docker)
                                          |
                                          v
                                   PostgreSQL (Render)
```

---

## 📦 Components

### 🖥️ Frontend

- React app
- Built using Vite
- Served by Nginx in Docker (production-style setup)
- Deployed on Vercel

### 🔙 Backend

- .NET 8 Web API (Clean Architecture)
- Layers:  
  `Controllers → Services → Repositories → EF Core`
- Dockerized
- Automatically runs EF Core migrations on startup

### 🗄️ Database

- PostgreSQL (Render managed DB)
- Persistent data across restarts
- Connection via environment variables

---

## 🐳 Local Development (Docker)

Run the full stack locally with one command:

```bash
docker compose up --build
```
