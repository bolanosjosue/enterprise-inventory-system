<script>
  import { onMount } from 'svelte';
  import { warehousesApi } from '$lib/api/warehouses.api';
  import Button from '$lib/components/ui/Button.svelte';
  import Input from '$lib/components/ui/Input.svelte';
  import Table from '$lib/components/ui/Table.svelte';
  import Alert from '$lib/components/ui/Alert.svelte';
  import Modal from '$lib/components/ui/Modal.svelte';
  import { Plus, Edit, Warehouse } from 'lucide-svelte';
  import { parseApiError } from '$lib/utils/errorParser';

  let warehouses = [];
  let loading = true;
  let error = '';
  let success = '';
  let errorDetails = null;

  let modalOpen = false;
  let modalMode = 'create';
  let editingWarehouse = null;
  let formLoading = false;
  
  let form = {
    name: '',
    address: '',
    maxCapacity: null
  };

  onMount(async () => {
    await loadWarehouses();
  });

  async function loadWarehouses() {
    try {
      loading = true;
      error = '';
      const response = await warehousesApi.getAll();
      warehouses = response.items || response;
    } catch (err) {
      error = err.response?.data?.error || 'Error al cargar bodegas';
    } finally {
      loading = false;
    }
  }

  function openCreateModal() {
    modalMode = 'create';
    editingWarehouse = null;
    form = { name: '', address: '', maxCapacity: null };
    error = '';
    success = '';
    modalOpen = true;
  }

  function openEditModal(warehouse) {
    modalMode = 'edit';
    editingWarehouse = warehouse;
    form = {
      name: warehouse.name,
      address: warehouse.address,
      maxCapacity: warehouse.maxCapacity
    };
    error = '';
    success = '';
    modalOpen = true;
  }

  function closeModal() {
    modalOpen = false;
    editingWarehouse = null;
  }

  async function handleSubmit(event) {
    event.preventDefault();
    formLoading = true;
    error = '';
    errorDetails = null;
    success = '';

    try {
        if (modalMode === 'create') {
        await warehousesApi.create(form);
        success = 'Bodega creada exitosamente';
        } else {
        await warehousesApi.update(editingWarehouse.id, form);
        success = 'Bodega actualizada exitosamente';
        }
        
        await loadWarehouses();
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
</script>

<svelte:head>
  <title>Bodegas - Inventory System</title>
</svelte:head>

<div class="space-y-6">
  <!-- Header -->
  <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
    <div>
      <h1 class="text-3xl font-bold text-gray-900">Bodegas</h1>
      <p class="text-gray-600 mt-1">Gestiona tus almacenes</p>
    </div>
    <Button variant="primary" on:click={openCreateModal}>
      <Plus class="w-4 h-4 inline mr-2" />
      Nueva Bodega
    </Button>
  </div>

  <Alert type="error" message={error} errors={errorDetails} />
  <Alert type="success" message={success} />

  <!-- Cards Grid -->
  <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
    {#if loading}
      <div class="col-span-full text-center py-12">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600 mx-auto"></div>
        <p class="mt-4 text-gray-600">Cargando bodegas...</p>
      </div>
    {:else if warehouses.length === 0}
      <div class="col-span-full text-center py-12">
        <Warehouse class="w-16 h-16 mx-auto text-gray-300 mb-4" />
        <p class="text-gray-500">No hay bodegas disponibles</p>
        <Button variant="primary" on:click={openCreateModal} class="mt-4">
          Crear primera bodega
        </Button>
      </div>
    {:else}
      {#each warehouses as warehouse}
        <div class="card hover:shadow-lg transition-shadow">
          <div class="flex items-start justify-between mb-4">
            <div class="flex items-center gap-3">
              <div class="p-3 bg-primary-100 rounded-xl">
                <Warehouse class="w-6 h-6 text-primary-700" />
              </div>
              <div>
                <h3 class="font-bold text-lg text-gray-900">{warehouse.name}</h3>
                <span 
                  class="text-xs px-2 py-1 rounded-full"
                  class:bg-green-100={warehouse.isActive}
                  class:text-green-700={warehouse.isActive}
                  class:bg-gray-100={!warehouse.isActive}
                  class:text-gray-700={!warehouse.isActive}
                >
                  {warehouse.isActive ? 'Activa' : 'Inactiva'}
                </span>
              </div>
            </div>
            <button
              on:click={() => openEditModal(warehouse)}
              class="text-primary-600 hover:text-primary-700 p-2"
              title="Editar"
            >
              <Edit class="w-5 h-5" />
            </button>
          </div>

          <div class="space-y-2">
            <div>
              <p class="text-xs text-gray-500 mb-1">Dirección</p>
              <p class="text-sm text-gray-700">{warehouse.address}</p>
            </div>

            {#if warehouse.maxCapacity}
              <div>
                <p class="text-xs text-gray-500 mb-1">Capacidad Máxima</p>
                <p class="text-sm font-semibold text-gray-900">
                  {warehouse.maxCapacity.toLocaleString()} unidades
                </p>
              </div>
            {/if}

            <div class="pt-3 border-t border-gray-200">
              <p class="text-xs text-gray-500">Movimientos totales</p>
              <p class="text-2xl font-bold text-primary-700">
                {warehouse.totalMovements || 0}
              </p>
            </div>
          </div>
        </div>
      {/each}
    {/if}
  </div>
</div>

<!-- Modal -->
<Modal 
  isOpen={modalOpen} 
  title={modalMode === 'create' ? 'Nueva Bodega' : 'Editar Bodega'}
  onClose={closeModal}
>
  <form on:submit={handleSubmit} class="space-y-4">
    <Alert type="error" message={error} errors={errorDetails} />
    <Alert type="success" message={success} />

    <div>
      <label for="name" class="label">Nombre *</label>
      <Input
        type="text"
        id="name"
        bind:value={form.name}
        placeholder="Bodega Central"
        required
        disabled={formLoading}
      />
    </div>

    <div>
      <label for="address" class="label">Dirección *</label>
      <textarea
        id="address"
        bind:value={form.address}
        placeholder="Dirección completa de la bodega"
        rows="3"
        class="input"
        required
        disabled={formLoading}
      ></textarea>
    </div>

    <div>
      <label for="maxCapacity" class="label">Capacidad Máxima</label>
      <Input
        type="number"
        id="maxCapacity"
        bind:value={form.maxCapacity}
        placeholder="10000"
        min="1"
        disabled={formLoading}
      />
      <p class="text-xs text-gray-500 mt-1">Número máximo de unidades (opcional)</p>
    </div>

    <div class="flex gap-4 pt-4">
      <Button
        type="submit"
        variant="primary"
        fullWidth
        disabled={formLoading}
      >
        {formLoading ? 'Guardando...' : (modalMode === 'create' ? 'Crear' : 'Actualizar')}
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
  </form>
</Modal>