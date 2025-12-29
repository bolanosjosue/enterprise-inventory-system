# 🎨 Enterprise Inventory System - Frontend

Aplicación web moderna construida con **SvelteKit** y **TailwindCSS** para gestión de inventario empresarial.

---

## ✨ **Características**

### **Gestión Completa**
- ✅ **Dashboard**: Estadísticas y acciones rápidas
- ✅ **Productos**: CRUD completo con búsqueda y filtros
- ✅ **Categorías**: Organización de productos
- ✅ **Proveedores**: Gestión de información y contacto
- ✅ **Bodegas**: Control multi-almacén con vista de cards
- ✅ **Movimientos**: Compra, Venta, Transferencia con validación de stock en tiempo real
- ✅ **Usuarios**: Gestión de roles y permisos (Admin only)

### **Autenticación y Seguridad**
- ✅ JWT Authentication con interceptors
- ✅ Protección de rutas por rol (Admin, Supervisor, Operator, Viewer)
- ✅ Sesión persistente en localStorage
- ✅ Logout automático en token expirado (401)
- ✅ Mensajes claros de permisos insuficientes (403)

### **UX/UI Moderna**
- ✅ Interfaz responsive (Desktop, Tablet, Mobile)
- ✅ Modales para crear/editar (sin cambio de página)
- ✅ Validación en tiempo real
- ✅ Mensajes de error detallados con FluentValidation
- ✅ Loading states y spinners
- ✅ Sidebar responsive con menú hamburguesa
- ✅ Badges de estado y stock
- ✅ Notificaciones de éxito temporales

---

## 🛠️ **Stack Tecnológico**

- **SvelteKit 2.0** - Framework web moderno y rápido
- **TailwindCSS 3.4** - Utility-first CSS
- **Axios** - HTTP client con interceptors
- **Lucide Svelte** - Iconos modernos
- **date-fns** - Manejo de fechas y formato

---

## 📂 **Estructura del Proyecto**
```
frontend/
├── src/
│   ├── lib/
│   │   ├── api/                    # API clients
│   │   │   ├── client.js           # Axios con interceptors JWT
│   │   │   ├── auth.api.js
│   │   │   ├── products.api.js
│   │   │   ├── categories.api.js
│   │   │   ├── suppliers.api.js
│   │   │   ├── warehouses.api.js
│   │   │   ├── stockMovements.api.js
│   │   │   └── users.api.js
│   │   │
│   │   ├── stores/                 # Estado global (Svelte stores)
│   │   │   └── auth.js             # Autenticación y usuario actual
│   │   │
│   │   ├── components/
│   │   │   ├── ui/                 # Componentes reutilizables
│   │   │   │   ├── Button.svelte
│   │   │   │   ├── Input.svelte
│   │   │   │   ├── Select.svelte
│   │   │   │   ├── Table.svelte
│   │   │   │   ├── Modal.svelte
│   │   │   │   └── Alert.svelte
│   │   │   │
│   │   │   ├── layout/             # Layout components
│   │   │   │   ├── Navbar.svelte
│   │   │   │   └── Sidebar.svelte
│   │   │   │
│   │   │   └── features/           # Componentes específicos
│   │   │       └── dashboard/
│   │   │           └── StatsCard.svelte
│   │   │
│   │   └── utils/                  # Utilidades
│   │       └── errorParser.js      # Parser de errores de API
│   │
│   └── routes/                     # Páginas (File-based routing)
│       ├── (auth)/                 # Grupo sin layout
│       │   ├── login/
│       │   │   └── +page.svelte
│       │   └── register/
│       │       └── +page.svelte
│       │
│       ├── (app)/                  # Grupo con layout (requiere auth)
│       │   ├── +layout.svelte      # Layout con Navbar + Sidebar
│       │   ├── dashboard/
│       │   ├── products/
│       │   ├── categories/
│       │   ├── suppliers/
│       │   ├── warehouses/
│       │   ├── movements/
│       │   │   ├── purchase/
│       │   │   ├── sale/
│       │   │   └── transfer/
│       │   └── users/
│       │
│       └── +layout.svelte          # Layout raíz
│
├── static/                         # Archivos estáticos
├── .env                            # Variables de entorno
├── .env.example                    # Ejemplo de variables
├── package.json
├── svelte.config.js
├── tailwind.config.js
└── vite.config.js
```

---

## 🚀 **Instalación y Ejecución**

### **Prerrequisitos**
- Node.js 18+
- npm o pnpm
- Backend corriendo en `http://localhost:5094`

### **1. Clonar el repositorio**
```bash
git clone https://github.com/bolanosjosue/enterprise-inventory-system.git
cd enterprise-inventory-system/frontend
```

### **2. Instalar dependencias**
```bash
npm install
```

### **3. Configurar variables de entorno**

Crea el archivo `.env`:
```bash
cp .env.example .env
```

Edita `.env`:
```env
VITE_API_URL=http://localhost:5094/api
```

### **4. Ejecutar en desarrollo**
```bash
npm run dev
```

La aplicación estará disponible en: `http://localhost:5173`

### **5. Build para producción**
```bash
npm run build
npm run preview
```

---

## 🔐 **Credenciales de Prueba**
```
Email: admin@inventory.com
Password: Admin123!
Rol: Admin (acceso total)
```

Para probar otros roles, registra usuarios y cambia su rol desde la página de Usuarios.

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

## 📱 **Responsive Design**

La aplicación está optimizada para:
- 📱 **Mobile**: 320px - 767px (Menú hamburguesa)
- 📱 **Tablet**: 768px - 1023px (Menú colapsable)
- 💻 **Desktop**: 1024px+ (Sidebar fijo)

---

## 🎨 **Personalización de Colores**

Los colores principales se configuran en `tailwind.config.js`:
```javascript
colors: {
  primary: {
    50: '#f0f9ff',
    100: '#e0f2fe',
    200: '#bae6fd',
    300: '#7dd3fc',
    400: '#38bdf8',
    500: '#0ea5e9',  // Color principal
    600: '#0284c7',
    700: '#0369a1',
    800: '#075985',
    900: '#0c4a6e',
  }
}
```

---

## 🧪 **Scripts Disponibles**
```bash
# Desarrollo
npm run dev

# Build
npm run build

# Preview de producción
npm run preview

# Linting
npm run lint

# Formateo
npm run format
```

---

## 🔧 **Características Técnicas**

### **API Client (Axios)**
- Interceptor de request: Agrega token JWT automáticamente
- Interceptor de response: Maneja 401 y redirige a login
- Base URL configurable por entorno

### **Error Handling**
- Parser de errores centralizado
- Soporte para FluentValidation
- Mensajes específicos por status code (401, 403, 404, 500)
- Alertas visuales con detalle de errores

### **Estado Global**
- Auth store con `localStorage` persistence
- Inicialización automática al cargar app
- Métodos: `init()`, `login()`, `logout()`

### **Validación de Stock**
- Compra: Sin límites
- Venta: Valida stock disponible en tiempo real
- Transferencia: Valida stock en bodega origen

---

## 🚧 **Mejoras Futuras**

- Tests unitarios con Vitest
- Tests E2E con Playwright
- Paginación en tablas grandes
- Exportar a Excel/PDF
- Gráficas de movimientos (Chart.js)
- Notificaciones push
- Dark mode
- Búsqueda avanzada con filtros múltiples
- Caché de datos con SWR

---

## 📄 **Licencia**

Este proyecto es de código abierto bajo la licencia MIT.

---

## 👨‍💻 **Autor**

**Josué Bolaños Urbina**  
Software Engineer

- GitHub: https://github.com/bolanosjosue  
- Portafolio: https://josuebolanos.netlify.app


⭐ **Si este proyecto te fue útil, considera darle una estrella en GitHub**