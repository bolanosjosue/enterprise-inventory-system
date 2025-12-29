# 🏢 Enterprise Inventory Management System

Sistema completo de gestión de inventario empresarial con **Clean Architecture**, **CQRS** y tecnologías modernas.

---

## 📸 **Screenshots**

### Dashboard
![Dashboard.png](https://i.postimg.cc/xTph7W6J/dashboard.png)

### Gestión de Productos
![Products.png](https://i.postimg.cc/WpH7PTPJ/products.png)

### Movimientos de Inventario
![Movements.png](https://i.postimg.cc/R0gHQLSM/movements.png)

---

## ✨ **Características Principales**

### **Backend (.NET 9 + PostgreSQL)**
- ✅ **Clean Architecture** con 4 capas (Domain, Application, Infrastructure, API)
- ✅ **CQRS** con MediatR para separación de comandos y consultas
- ✅ **Domain-Driven Design** con entidades ricas y lógica de negocio encapsulada
- ✅ **JWT Authentication** con roles (Admin, Supervisor, Operator, Viewer)
- ✅ **FluentValidation** para validaciones declarativas
- ✅ **Soft Delete** y auditoría automática en todas las entidades
- ✅ **Swagger/OpenAPI** con autenticación JWT integrada
- ✅ **Rate Limiting** y middleware de manejo de excepciones

### **Frontend (SvelteKit + TailwindCSS)**
- ✅ **Interfaz moderna y responsive** (Desktop, Tablet, Mobile)
- ✅ **File-based routing** con SvelteKit
- ✅ **Componentes reutilizables** con TailwindCSS
- ✅ **Modales** para crear/editar sin cambio de página
- ✅ **Validación en tiempo real** con mensajes de error detallados
- ✅ **Protección de rutas** por rol con redirección automática

### **Funcionalidades del Sistema**
- ✅ **Productos**: CRUD completo con SKU único, precios, stock mínimo/máximo
- ✅ **Categorías**: Organización jerárquica de productos
- ✅ **Proveedores**: Gestión de información y contacto
- ✅ **Bodegas**: Control multi-almacén con capacidad máxima
- ✅ **Movimientos**: Compra, Venta, Transferencia entre bodegas
- ✅ **Usuarios**: Gestión de roles y permisos (Admin only)
- ✅ **Dashboard**: Estadísticas en tiempo real y acciones rápidas
- ✅ **Historial completo**: Auditoría de todos los movimientos

---

## 🏗️ **Arquitectura del Sistema**

![Arquitectura.png](https://i.postimg.cc/SsJJLXxJ/Diagrama-Arquitectura.png)
---

## 🛠️ **Stack Tecnológico**

### **Backend**
- **.NET 9.0** - Framework
- **ASP.NET Core Web API** - RESTful API
- **Entity Framework Core 9** - ORM
- **PostgreSQL 14+** - Base de datos
- **MediatR** - CQRS pattern
- **FluentValidation** - Validaciones
- **BCrypt.Net** - Password hashing
- **JWT** - Autenticación
- **Swagger** - Documentación

### **Frontend**
- **SvelteKit 2.0** - Framework web
- **TailwindCSS 3.4** - Estilos
- **Axios** - HTTP client
- **Lucide Svelte** - Iconos
- **date-fns** - Manejo de fechas

---

## 📂 **Estructura del Proyecto**
```
enterprise-inventory-system/
│
├── backend/                        # Backend .NET
│   ├── InventorySystem.API/        # Capa de presentación (Controllers)
│   ├── InventorySystem.Application/ # Capa de aplicación (CQRS)
│   ├── InventorySystem.Domain/     # Capa de dominio (Entidades)
│   ├── InventorySystem.Infrastructure/ # Capa de infraestructura (EF Core)
│   ├── InventorySystem.sln
│   └── README.md
│
├── frontend/                       # Frontend SvelteKit
│   ├── src/
│   │   ├── lib/                    # Librerías compartidas
│   │   │   ├── api/                # API clients
│   │   │   ├── components/         # Componentes Svelte
│   │   │   ├── stores/             # Estado global
│   │   │   └── utils/              # Utilidades
│   │   └── routes/                 # Páginas (file-based routing)
│   │       ├── (auth)/             # Login, Register
│   │       └── (app)/              # Dashboard, Products, etc
│   ├── static/
│   ├── package.json
│   └── README.md
│
└── README.md
```

---

## 🚀 **Instalación y Ejecución**

### **Prerrequisitos**
- **.NET 9 SDK** ([Descargar](https://dotnet.microsoft.com/download))
- **Node.js 18+** ([Descargar](https://nodejs.org))
- **PostgreSQL 14+** ([Descargar](https://www.postgresql.org/download/))
- **Git**

---

### **1️⃣ Clonar el Repositorio**
```bash
git clone https://github.com/bolanosjosue/enterprise-inventory-system.git
cd enterprise-inventory-system
```

---

### **2️⃣ Configurar Backend**

#### **2.1 Crear base de datos**
```sql
CREATE DATABASE InventorySystemDb;
```

#### **2.2 Configurar variables**

Edita `backend/InventorySystem.API/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=InventorySystemDb;Username=postgres;Password=TU_PASSWORD"
  },
  "JwtSettings": {
    "Secret": "tu-secret-key-de-al-menos-32-caracteres-aqui",
    "ExpirationInHours": 8,
    "Issuer": "InventorySystemAPI",
    "Audience": "InventorySystemClient"
  }
}
```

#### **2.3 Aplicar migraciones**
```bash
cd backend
dotnet ef database update --project InventorySystem.Infrastructure --startup-project InventorySystem.API
```

#### **2.4 Ejecutar backend**
```bash
dotnet run --project InventorySystem.API
```

Backend disponible en: `http://localhost:5094`

---

### **3️⃣ Configurar Frontend**

#### **3.1 Instalar dependencias**
```bash
cd frontend
npm install
```

#### **3.2 Configurar variables**

Crea `frontend/.env`:
```env
VITE_API_URL=http://localhost:5094/api
```

#### **3.3 Ejecutar frontend**
```bash
npm run dev
```

Frontend disponible en: `http://localhost:5173`

---

## 🔐 **Credenciales de Prueba**

El sistema incluye datos de prueba (seed data):
```
Email: admin@inventory.com
Password: Admin123!
Rol: Administrador (acceso total)
```

Para probar otros roles:
1. Registra un nuevo usuario (será Operador por defecto)
2. Inicia sesión como Admin
3. Ve a **Usuarios** y cambia el rol

---

## 🎯 **Roles y Permisos**

| Acción | Admin | Supervisor | Operator | Viewer |
|--------|-------|------------|----------|--------|
| Ver Dashboard | ✅ | ✅ | ✅ | ✅ |
| Ver Productos | ✅ | ✅ | ✅ | ✅ |
| Crear/Editar Productos | ✅ | ✅ | ❌ | ❌ |
| Eliminar Productos | ✅ | ❌ | ❌ | ❌ |
| Gestionar Categorías | ✅ | ✅ | ❌ | ❌ |
| Gestionar Proveedores | ✅ | ✅ | ❌ | ❌ |
| Gestionar Bodegas | ✅ | ❌ | ❌ | ❌ |
| Registrar Compra | ✅ | ✅ | ❌ | ❌ |
| Registrar Venta | ✅ | ✅ | ✅ | ❌ |
| Transferir Stock | ✅ | ✅ | ❌ | ❌ |
| Gestionar Usuarios | ✅ | ❌ | ❌ | ❌ |

---

## 📚 **Documentación Adicional**

- [Backend README](./backend/README.md) - Arquitectura, patrones y deployment
- [Frontend README](./frontend/README.md) - Componentes, rutas y personalización

---

## 📄 **Licencia**

Este proyecto es de código abierto bajo la licencia MIT.

---

## 👨‍💻 **Autor**

**Josue Bolanos**

- GitHub: https://github.com/bolanosjosue
- LinkedIn: https://www.linkedin.com/in/josuebolanos-dev/
- Email: josuebolanos2004@gmail.com
- Portfolio:https://josuebolanos.netlify.app/

---

## 🔗 **Enlaces Útiles**

- [Documentación de .NET](https://docs.microsoft.com/en-us/dotnet/)
- [Documentación de SvelteKit](https://kit.svelte.dev/)
- [TailwindCSS](https://tailwindcss.com/)
- [PostgreSQL](https://www.postgresql.org/docs/)

---


**⭐ Si te gustó este proyecto, considera darle una estrella ⭐**


