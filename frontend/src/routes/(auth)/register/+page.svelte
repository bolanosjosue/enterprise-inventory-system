<script>
  import { goto } from '$app/navigation';
  import { authStore } from '$lib/stores/auth';
  import { authApi } from '$lib/api/auth.api';
  import Button from '$lib/components/ui/Button.svelte';
  import Input from '$lib/components/ui/Input.svelte';
  import Alert from '$lib/components/ui/Alert.svelte';

  let email = '';
  let password = '';
  let fullName = '';
  let loading = false;
  let error = '';

  async function handleRegister(event) {
    event.preventDefault();
    loading = true;
    error = '';

    try {
      const data = await authApi.register(email, password, fullName);
      
      authStore.login({
        userId: data.userId,
        email: data.email,
        fullName: data.fullName,
        role: data.role
      }, data.token);
      
      goto('/dashboard');
    } catch (err) {
      error = err.response?.data?.error || 'Error al registrar la cuenta';
    } finally {
      loading = false;
    }
  }
</script>

<svelte:head>
  <title>Registro - Sistema de Inventario</title>
</svelte:head>

<div class="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
  <div class="max-w-md w-full space-y-8">
    <!-- Header -->
    <div>
      <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">
        Crear cuenta
      </h2>
      <p class="mt-2 text-center text-sm text-gray-600">
        Comienza a gestionar tu inventario
      </p>
    </div>

    <!-- Form -->
    <form class="mt-8 space-y-6" on:submit={handleRegister}>
      <Alert type="error" message={error} />

      <div class="rounded-md shadow-sm space-y-4">
        <div>
          <label for="fullName" class="label">Nombre completo</label>
          <Input
            type="text"
            id="fullName"
            bind:value={fullName}
            placeholder="Juan Pérez"
            required
            disabled={loading}
          />
        </div>

        <div>
          <label for="email" class="label">Correo electrónico</label>
          <Input
            type="email"
            id="email"
            bind:value={email}
            placeholder="juan@ejemplo.com"
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
          <p class="mt-1 text-xs text-gray-500">
            Mínimo 6 caracteres
          </p>
        </div>
      </div>

      <div>
        <Button
          type="submit"
          variant="primary"
          fullWidth
          disabled={loading}
        >
          {loading ? 'Creando cuenta...' : 'Crear cuenta'}
        </Button>
      </div>

      <div class="text-center">
        <a href="/login" class="text-sm text-primary-600 hover:text-primary-500">
          ¿Ya tienes una cuenta? Inicia sesión
        </a>
      </div>
    </form>
  </div>
</div>
