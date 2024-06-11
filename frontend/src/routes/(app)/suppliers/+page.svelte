<script>
  import { onMount } from 'svelte';
  import { suppliersApi } from '$lib/api/suppliers.api';
  import Button from '$lib/components/ui/Button.svelte';
  import Input from '$lib/components/ui/Input.svelte';
  import Table from '$lib/components/ui/Table.svelte';
  import Alert from '$lib/components/ui/Alert.svelte';
  import Modal from '$lib/components/ui/Modal.svelte';
  import { Plus, Edit } from 'lucide-svelte';
  import { parseApiError } from '$lib/utils/errorParser';

  let suppliers = [];
  let loading = true;
  let error = '';
  let success = '';
  let errorDetails = null;
  
  let modalOpen = false;
  let modalMode = 'create';
  let editingSupplier = null;
  let formLoading = false;
  
  let form = {
    name: '',
    taxId: '',
    email: '',
    phone: '',
    address: ''
  };

  onMount(async () => {
    await loadSuppliers();
  });

  async function loadSuppliers() {
    try {
      loading = true;
      error = '';
      const response = await suppliersApi.getAll();
      suppliers = response.items || response;
    } catch (err) {
      error = err.response?.data?.error || 'Error al cargar proveedores';
    } finally {
      loading = false;
    }
  }

  function openCreateModal() {
    modalMode = 'create';
    editingSupplier = null;
    form = { name: '', taxId: '', email: '', phone: '', address: '' };
    error = '';
    success = '';
    modalOpen = true;
  }

  function openEditModal(supplier) {
    modalMode = 'edit';
    editingSupplier = supplier;
    form = {
      name: supplier.name,
      taxId: supplier.taxId,
      email: supplier.email || '',
      phone: supplier.phone || '',
      address: supplier.address || ''
    };
    error = '';
    success = '';
    modalOpen = true;
  }

  function closeModal() {
    modalOpen = false;
    editingSupplier = null;
  }

  async function handleSubmit(event) {
    event.preventDefault();
    formLoading = true;
    error = '';
    errorDetails = null;
    success = '';

    try {
        if (modalMode === 'create') {
        await suppliersApi.create(form);
        success = 'Proveedor creado exitosamente';
        } else {
        await suppliersApi.update(editingSupplier.id, form);
        success = 'Proveedor actualizado exitosamente';
        }
        
        await loadSuppliers();
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
  <title>Proveedores - Inventory System</title>
</svelte:head>

<div class="space-y-6">
  <!-- Header -->
  <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
    <div>
      <h1 class="text-3xl font-bold text-gray-900">Proveedores</h1>
      <p class="text-gray-600 mt-1">Gestiona tus proveedores</p>
    </div>
    <Button variant="primary" on:click={openCreateModal}>
      <Plus class="w-4 h-4 inline mr-2" />
      Nuevo Proveedor
    </Button>
  </div>

  <Alert type="error" message={error} errors={errorDetails} />
  <Alert type="success" message={success} />

  <!-- Table -->
  <div class="card">
    {#if loading}
      <div class="text-center py-12">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600 mx-auto"></div>
        <p class="mt-4 text-gray-600">Cargando proveedores...</p>
      </div>
    {:else if suppliers.length === 0}
      <div class="text-center py-12">
        <p class="text-gray-500">No hay proveedores disponibles</p>
      </div>
    {:else}
      <Table headers={['Nombre', 'ID Fiscal', 'Email', 'Teléfono', 'Productos', 'Estado', 'Acciones']}>
        {#each suppliers as supplier}
          <tr class="hover:bg-gray-50">
            <td class="font-medium">{supplier.name}</td>
            <td class="font-mono text-sm">{supplier.taxId}</td>
            <td class="text-sm text-gray-600">{supplier.email || '-'}</td>
            <td class="text-sm text-gray-600">{supplier.phone || '-'}</td>
            <td>
              <span class="px-2 py-1 text-xs font-medium rounded-full bg-primary-100 text-primary-700">
                {supplier.productCount || 0}
              </span>
            </td>
            <td>
              <span 
                class="px-2 py-1 text-xs font-medium rounded-full"
                class:bg-green-100={supplier.isActive}
                class:text-green-700={supplier.isActive}
                class:bg-gray-100={!supplier.isActive}
                class:text-gray-700={!supplier.isActive}
              >
                {supplier.isActive ? 'Activo' : 'Inactivo'}
              </span>
            </td>
            <td>
              <button
                on:click={() => openEditModal(supplier)}
                class="text-primary-600 hover:text-primary-700 p-1"
                title="Editar"
              >
                <Edit class="w-4 h-4" />
              </button>
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
  title={modalMode === 'create' ? 'Nuevo Proveedor' : 'Editar Proveedor'}
  onClose={closeModal}
>
  <form on:submit={handleSubmit} class="space-y-4">
    <Alert type="error" message={error} errors={errorDetails} />
    <Alert type="success" message={success} />

    <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
      <div>
        <label for="name" class="label">Nombre *</label>
        <Input
          type="text"
          id="name"
          bind:value={form.name}
          placeholder="Distribuidora XYZ"
          required
          disabled={formLoading}
        />
      </div>

      <div>
        <label for="taxId" class="label">ID Fiscal *</label>
        <Input
          type="text"
          id="taxId"
          bind:value={form.taxId}
          placeholder="3-101-123456"
          required
          disabled={formLoading || modalMode === 'edit'}
        />
      </div>
    </div>

    <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
      <div>
        <label for="email" class="label">Email</label>
        <Input
          type="email"
          id="email"
          bind:value={form.email}
          placeholder="contacto@proveedor.com"
          disabled={formLoading}
        />
      </div>

      <div>
        <label for="phone" class="label">Teléfono</label>
        <Input
          type="tel"
          id="phone"
          bind:value={form.phone}
          placeholder="2222-3333"
          disabled={formLoading}
        />
      </div>
    </div>

    <div>
      <label for="address" class="label">Dirección</label>
      <textarea
        id="address"
        bind:value={form.address}
        placeholder="Dirección completa"
        rows="2"
        class="input"
        disabled={formLoading}
      ></textarea>
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