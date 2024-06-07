<script>
  import { goto } from '$app/navigation';
  import { authStore } from '$lib/stores/auth';
  import { authApi } from '$lib/api/auth.api';
  import Button from '$lib/components/ui/Button.svelte';
  import Input from '$lib/components/ui/Input.svelte';
  import Alert from '$lib/components/ui/Alert.svelte';

  let email = '';
  let password = '';
  let loading = false;
  let error = '';

  async function handleLogin(event) {
    event.preventDefault();
    loading = true;
    error = '';

    try {
      const data = await authApi.login(email, password);
      
      authStore.login({
        userId: data.userId,
        email: data.email,
        fullName: data.fullName,
        role: data.role
      }, data.token);
      
      goto('/dashboard');
    } catch (err) {
      error = err.response?.data?.error || 'Credenciales inválidas';
    } finally {
      loading = false;
    }
  }
</script>

<svelte:head>
  <title>Iniciar sesión - Sistema de Inventario</title>
</svelte:head>

<div class="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
  <div class="max-w-md w-full space-y-8">
    <!-- Header -->
    <div>
      <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">
        Inventory Management System
      </h2>
      <p class="mt-2 text-center text-sm text-gray-600">
        Inicia sesión en tu cuenta
      </p>
    </div>

    <!-- Form -->
    <form class="mt-8 space-y-6" on:submit={handleLogin}>
      <Alert type="error" message={error} />

      <div class="rounded-md shadow-sm space-y-4">
        <div>
          <label for="email" class="label">Correo electrónico</label>
          <Input
            type="email"
            id="email"
            bind:value={email}
            placeholder="admin@inventory.com"
            required
            disabled={loading}
          />
        </div>

        <div>
          <label for="password" class="label">Contraseña</label>
          <Input
            type="password"
            id="password"
            bind:value={password}
            placeholder="••••••••"
            required
            disabled={loading}
          />
        </div>
      </div>

      <div>
        <Button
          type="submit"
          variant="primary"
          fullWidth
          disabled={loading}
        >
          {loading ? 'Iniciando sesión...' : 'Iniciar sesión'}
        </Button>
      </div>

      <div class="text-center">
        <a href="/register" class="text-sm text-primary-600 hover:text-primary-500">
          ¿No tienes una cuenta? Regístrate
        </a>
      </div>
    </form>

    <!-- Demo Credentials -->
    <div class="mt-6 p-4 bg-blue-50 rounded-lg">
      <p class="text-xs text-blue-800 font-semibold mb-2">Credenciales de prueba:</p>
      <p class="text-xs text-blue-700">Correo: admin@inventory.com</p>
      <p class="text-xs text-blue-700">Contraseña: Admin123!</p>
    </div>
  </div>
</div>
