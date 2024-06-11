<script>
  import { onMount } from 'svelte';
  import { authStore } from '$lib/stores/auth';
  import { goto } from '$app/navigation';
  import { usersApi } from '$lib/api/users.api';
  import Button from '$lib/components/ui/Button.svelte';
  import Table from '$lib/components/ui/Table.svelte';
  import Alert from '$lib/components/ui/Alert.svelte';
  import Modal from '$lib/components/ui/Modal.svelte';
  import Select from '$lib/components/ui/Select.svelte';
  import { Edit, UserCheck, UserX } from 'lucide-svelte';
  import { format } from 'date-fns';
  import { es } from 'date-fns/locale';
  import { parseApiError } from '$lib/utils/errorParser';

  let currentUser = null;
  let users = [];
  let loading = true;
  let error = '';
  let success = '';
  let errorDetails = null;
  
  let modalOpen = false;
  let editingUser = null;
  let formLoading = false;
  let selectedRole = '';

  const roleOptions = [
    { value: 'Admin', label: 'Administrador' },
    { value: 'Supervisor', label: 'Supervisor' },
    { value: 'Operator', label: 'Operador' },
    { value: 'Viewer', label: 'Visor' }
  ];

  authStore.subscribe((state) => {
    currentUser = state.user;
  });

  onMount(async () => {
    if (currentUser?.role !== 'Admin') {
      goto('/dashboard');
      return;
    }
    await loadUsers();
  });

  async function loadUsers() {
    try {
      loading = true;
      error = '';
      const response = await usersApi.getAll();
      users = Array.isArray(response) ? response : response.items || [];
    } catch (err) {
      error = err.response?.data?.error || 'Error al cargar usuarios';
    } finally {
      loading = false;
    }
  }

  function openEditModal(user) {
    editingUser = user;
    selectedRole = user.role;
    error = '';
    success = '';
    modalOpen = true;
  }

  function closeModal() {
    modalOpen = false;
    editingUser = null;
    selectedRole = '';
  }

  async function handleUpdateRole(event) {
    event.preventDefault();
    formLoading = true;
    error = '';
    errorDetails = null;
    success = '';

    try {
        await usersApi.updateRole(editingUser.id, selectedRole);
        success = 'Rol actualizado exitosamente';
        await loadUsers();
        setTimeout(() => {
        closeModal();
        }, 1000);
    } catch (err) {
        const parsed = parseApiError(err);
        error = parsed.message;
        errorDetails = parsed.errors;
    } finally {
        formLoading = false;
    }
    }

  async function handleToggleStatus(userId) {
    try {
      await usersApi.toggleStatus(userId);
      success = 'Estado actualizado exitosamente';
      await loadUsers();
      setTimeout(() => { success = ''; }, 3000);
    } catch (err) {
      error = err.response?.data?.error || 'Error al cambiar estado';
    }
  }

  function formatDate(dateString) {
    if (!dateString) return 'Nunca';
    return format(new Date(dateString), "dd MMM yyyy", { locale: es });
  }

  function getRoleLabel(role) {
    const labels = {
      'Admin': 'Administrador',
      'Supervisor': 'Supervisor',
      'Operator': 'Operador',
      'Viewer': 'Visor'
    };
    return labels[role] || role;
  }

  function getRoleBadgeColor(role) {
    const colors = {
      'Admin': 'bg-red-100 text-red-700',
      'Supervisor': 'bg-blue-100 text-blue-700',
      'Operator': 'bg-green-100 text-green-700',
      'Viewer': 'bg-gray-100 text-gray-700'
    };
    return colors[role] || 'bg-gray-100 text-gray-700';
  }
</script>

<svelte:head>
  <title>Usuarios - Inventory System</title>
</svelte:head>

<div class="space-y-6">
  <!-- Header -->
  <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
    <div>
      <h1 class="text-3xl font-bold text-gray-900">Usuarios</h1>
      <p class="text-gray-600 mt-1">Gestiona roles y permisos</p>
    </div>
  </div>

  <Alert type="error" message={error} errors={errorDetails} />
  <Alert type="success" message={success} />

  <!-- Info Card -->
  <div class="bg-blue-50 border-2 border-blue-200 rounded-xl p-4">
    <div class="flex items-start gap-3">
      <div class="p-2 bg-blue-100 rounded-lg">
        <svg class="w-5 h-5 text-blue-700" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
        </svg>
      </div>
      <div>
        <p class="text-sm font-medium text-blue-900">Roles del Sistema</p>
        <ul class="text-xs text-blue-700 mt-2 space-y-1">
          <li>• <strong>Admin:</strong> Acceso total al sistema</li>
          <li>• <strong>Supervisor:</strong> CRUD productos, categorías, proveedores + movimientos</li>
          <li>• <strong>Operador:</strong> Ver todo + registrar ventas</li>
          <li>• <strong>Visor:</strong> Solo lectura</li>
        </ul>
      </div>
    </div>
  </div>

  <!-- Table -->
  <div class="card">
    {#if loading}
      <div class="text-center py-12">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600 mx-auto"></div>
        <p class="mt-4 text-gray-600">Cargando usuarios...</p>
      </div>
    {:else if users.length === 0}
      <div class="text-center py-12">
        <p class="text-gray-500">No hay usuarios disponibles</p>
      </div>
    {:else}
      <Table headers={['Usuario', 'Email', 'Rol', 'Estado', 'Último Acceso', 'Acciones']}>
        {#each users as user}
          <tr class="hover:bg-gray-50">
            <td class="font-medium">{user.fullName}</td>
            <td class="text-sm text-gray-600">{user.email}</td>
            <td>
              <span class="px-2 py-1 text-xs font-medium rounded-full {getRoleBadgeColor(user.role)}">
                {getRoleLabel(user.role)}
              </span>
            </td>
            <td>
              <span 
                class="px-2 py-1 text-xs font-medium rounded-full"
                class:bg-green-100={user.isActive}
                class:text-green-700={user.isActive}
                class:bg-red-100={!user.isActive}
                class:text-red-700={!user.isActive}
              >
                {user.isActive ? 'Activo' : 'Inactivo'}
              </span>
            </td>
            <td class="text-sm text-gray-500">
              {formatDate(user.lastLoginAt)}
            </td>
            <td>
              <div class="flex gap-2">
                <button
                  on:click={() => openEditModal(user)}
                  class="text-primary-600 hover:text-primary-700 p-1"
                  title="Cambiar rol"
                  disabled={user.id === currentUser?.userId}
                >
                  <Edit class="w-4 h-4" />
                </button>
                <button
                  on:click={() => handleToggleStatus(user.id)}
                  class="p-1"
                  class:text-red-600={user.isActive}
                  class:hover:text-red-700={user.isActive}
                  class:text-green-600={!user.isActive}
                  class:hover:text-green-700={!user.isActive}
                  title={user.isActive ? 'Desactivar' : 'Activar'}
                  disabled={user.id === currentUser?.userId}
                >
                  {#if user.isActive}
                    <UserX class="w-4 h-4" />
                  {:else}
                    <UserCheck class="w-4 h-4" />
                  {/if}
                </button>
              </div>
            </td>
          </tr>
        {/each}
      </Table>
    {/if}
  </div>
</div>

<!-- Modal -->
<Modal 
  isOpen={modalOpen} 
  title="Cambiar Rol de Usuario"
  onClose={closeModal}
>
  <form on:submit={handleUpdateRole} class="space-y-4">
    <Alert type="error" message={error} errors={errorDetails} />
    <Alert type="success" message={success} />

    {#if editingUser}
      <div class="p-4 bg-gray-50 rounded-lg">
        <p class="text-sm text-gray-600 mb-1">Usuario</p>
        <p class="font-bold text-gray-900">{editingUser.fullName}</p>
        <p class="text-sm text-gray-500">{editingUser.email}</p>
      </div>

      <div>
        <label for="role" class="label">Nuevo Rol *</label>
        <Select
          bind:value={selectedRole}
          options={roleOptions}
          placeholder="Seleccionar rol"
          required
          disabled={formLoading}
        />
      </div>

      <div class="p-4 bg-yellow-50 border border-yellow-200 rounded-lg">
        <p class="text-xs text-yellow-800">
          ⚠️ Cambiar el rol afectará los permisos de este usuario inmediatamente.
        </p>
      </div>

      <div class="flex gap-4 pt-4">
        <Button
          type="submit"
          variant="primary"
          fullWidth
          disabled={formLoading || selectedRole === editingUser.role}
        >
          {formLoading ? 'Actualizando...' : 'Actualizar Rol'}
        </Button>
        <Button
          type="button"
          variant="secondary"
          fullWidth
          on:click={closeModal}
          disabled={formLoading}
        >
          Cancelar
        </Button>
      </div>
    {/if}
  </form>
</Modal>