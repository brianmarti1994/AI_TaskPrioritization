# 🤖 AI Task Prioritization App

<img width="450" height="300" alt="image" src="https://github.com/user-attachments/assets/3fa3e4aa-179d-4495-bf5d-128590c82609" />


> A full-stack **.NET + Blazor WebAssembly** application that uses **AI (Google Gemini)** to automatically predict and highlight **High / Medium / Low** priority tasks.

---

## ✨ What is this project?

**AI Task Prioritization** helps teams and individuals focus on what matters most.

You add tasks with a title, description, and optional due date —  
the AI analyzes urgency, impact, and context, then assigns a priority:

- 🔴 **High** – urgent, high impact
- 🟡 **Medium** – important but not critical
- 🟢 **Low** – non-urgent or maintenance tasks

Tasks are visually highlighted so priorities are instantly clear.

---

## 🧠 How AI is used

- Uses **Google Gemini (GenAI)** for natural-language understanding
- No ML.NET, no training required
- AI runs **server-side** in the ASP.NET Core API (keys are never exposed)
- Includes normalization & fallback logic for reliability

---

## 🏗️ Tech Stack

### Backend (API)
- **ASP.NET Core Web API (.NET 8)**
- **Entity Framework Core**
- **SQLite** (lightweight, file-based DB)
- **Google Gemini (Google.GenAI SDK)**

### Frontend (UI)
- **Blazor WebAssembly**
- Clean, modern UI with priority-based colors
- Fully decoupled from backend

### Architecture
- DTO-based API contracts
- Clean separation of concerns
- AI provider isolated in a service layer
- Ready for future provider switching (OpenAI / Alibaba / Gemini)

---

## 📸 Features

- ✅ Create tasks with title, description, and due date
- ✅ AI-powered priority prediction
- ✅ Color-coded task list (High / Medium / Low)
- ✅ Search tasks by title or description
- ✅ SQLite persistence
- ✅ Secure API key handling (env vars / launch settings)

---

## 🚀 Getting Started

### 1️⃣ Clone the repo
```bash
git clone https://github.com/your-username/ai-task-prioritization.git
cd ai-task-prioritization
