<script>
  import { onMount } from 'svelte';
  import { categoriesApi } from '$lib/api/categories.api';
  import Button from '$lib/components/ui/Button.svelte';
  import Input from '$lib/components/ui/Input.svelte';
  import Table from '$lib/components/ui/Table.svelte';
  import Alert from '$lib/components/ui/Alert.svelte';
  import Modal from '$lib/components/ui/Modal.svelte';
  import { Plus, Edit, Trash2 } from 'lucide-svelte';
  import { parseApiError } from '$lib/utils/errorParser';

  let categories = [];
  let loading = true;
  let error = '';
  let errorDetails = null;
  let success = '';
  
  let modalOpen = false;
  let modalMode = 'create';
  let editingCategory = null;
  let formLoading = false;
  
  let form = {
    name: '',
    description: ''
  };

  onMount(async () => {
    await loadCategories();
  });

  async function loadCategories() {
    try {
      loading = true;
      error = '';
      const response = await categoriesApi.getAll();
      categories = response.items || response;
    } catch (err) {
      error = err.response?.data?.error || 'Error al cargar categorías';
    } finally {
      loading = false;
    }
  }

  function openCreateModal() {
    modalMode = 'create';
    editingCategory = null;
    form = { name: '', description: '' };
    error = '';
    success = '';
    modalOpen = true;
  }

  function openEditModal(category) {
    modalMode = 'edit';
    editingCategory = category;
    form = {
      name: category.name,
      description: category.description || ''
    };
    error = '';
    success = '';
    modalOpen = true;
  }

  function closeModal() {
    modalOpen = false;
    editingCategory = null;
  }

  async function handleSubmit(event) {
    event.preventDefault();
    formLoading = true;
    error = '';
    errorDetails = null;
    success = '';

    try {
        if (modalMode === 'create') {
        await categoriesApi.create(form);
        success = 'Categoría creada exitosamente';
        } else {
        await categoriesApi.update(editingCategory.id, form);
        success = 'Categoría actualizada exitosamente';
        }
        
        await loadCategories();
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

  async function handleDelete(id) {
    if (!confirm('¿Estás seguro de eliminar esta categoría?')) return;

    try {
      await categoriesApi.delete(id);
      success = 'Categoría eliminada exitosamente';
      await loadCategories();
      setTimeout(() => { success = ''; }, 3000);
    } catch (err) {
      error = err.response?.data?.error || 'Error al eliminar categoría';
    }
  }
</script>

<svelte:head>
  <title>Categorías - Inventory System</title>
</svelte:head>

<div class="space-y-6">
  <!-- Header -->
  <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
    <div>
      <h1 class="text-3xl font-bold text-gray-900">Categorías</h1>
      <p class="text-gray-600 mt-1">Organiza tus productos por categorías</p>
    </div>
    <Button variant="primary" on:click={openCreateModal}>
      <Plus class="w-4 h-4 inline mr-2" />
      Nueva Categoría
    </Button>
  </div>

  <Alert type="error" message={error} errors={errorDetails} />
  <Alert type="success" message={success} />

  <!-- Table -->
  <div class="card">
    {#if loading}
      <div class="text-center py-12">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600 mx-auto"></div>
        <p class="mt-4 text-gray-600">Cargando categorías...</p>
      </div>
    {:else if categories.length === 0}
      <div class="text-center py-12">
        <p class="text-gray-500">No hay categorías disponibles</p>
      </div>
    {:else}
      <Table headers={['Nombre', 'Descripción', 'Productos', 'Acciones']}>
        {#each categories as category}
          <tr class="hover:bg-gray-50">
            <td class="font-medium">{category.name}</td>
            <td class="text-gray-600">{category.description || '-'}</td>
            <td>
              <span class="px-2 py-1 text-xs font-medium rounded-full bg-primary-100 text-primary-700">
                {category.productCount || 0}
              </span>
            </td>
            <td>
              <div class="flex gap-2">
                <button
                  on:click={() => openEditModal(category)}
                  class="text-primary-600 hover:text-primary-700 p-1"
                  title="Editar"
                >
                  <Edit class="w-4 h-4" />
                </button>
                <button
                  on:click={() => handleDelete(category.id)}
                  class="text-red-600 hover:text-red-700 p-1"
                  title="Eliminar"
                >
                  <Trash2 class="w-4 h-4" />
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
  title={modalMode === 'create' ? 'Nueva Categoría' : 'Editar Categoría'}
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
        placeholder="Electrónica"
        required
        disabled={formLoading}
      />
    </div>

    <div>
      <label for="description" class="label">Descripción</label>
      <textarea
        id="description"
        bind:value={form.description}
        placeholder="Descripción de la categoría"
        rows="3"
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