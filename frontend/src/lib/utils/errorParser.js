/**
 * Parsea errores de la API en un formato consistente
 * @param {Error} err - Error de axios
 * @returns {Object} { message: string, errors: string|array|object|null }
 */
export function parseApiError(err) {
  const response = err.response?.data;
  const status = err.response?.status;

  // Sin conexión
  if (!response && !status) {
    return {
      message: 'Error de conexión con el servidor',
      errors: null
    };
  }

  // Errores por status code
  if (status === 401) {
    return {
      message: '🔒 No tienes autorización. Por favor inicia sesión nuevamente.',
      errors: null
    };
  }

  if (status === 403) {
    return {
      message: '⛔ No tienes permisos para realizar esta acción',
      errors: null
    };
  }

  if (status === 404) {
    return {
      message: '❌ Recurso no encontrado',
      errors: null
    };
  }

  if (status === 500) {
    return {
      message: '💥 Error interno del servidor',
      errors: null
    };
  }

  // Caso 1: Mensaje simple { error: "mensaje" }
  if (response?.error) {
    return {
      message: response.error,
      errors: null
    };
  }

  // Caso 2: FluentValidation { errors: { Field: ["error1", "error2"] } }
  if (response?.errors && typeof response.errors === 'object') {
    return {
      message: 'Errores de validación:',
      errors: response.errors
    };
  }

  // Caso 3: Array de errores
  if (Array.isArray(response)) {
    return {
      message: 'Se encontraron los siguientes errores:',
      errors: response
    };
  }

  // Caso 4: Mensaje genérico del backend
  if (response?.title) {
    return {
      message: response.title,
      errors: response.detail ? [response.detail] : null
    };
  }

  // Default
  return {
    message: 'Ha ocurrido un error inesperado',
    errors: null
  };
}