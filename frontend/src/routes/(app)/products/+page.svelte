<script>
  import { onMount } from 'svelte';
  import { productsApi } from '$lib/api/products.api';
  import { categoriesApi } from '$lib/api/categories.api';
  import { suppliersApi } from '$lib/api/suppliers.api';
  import Button from '$lib/components/ui/Button.svelte';
  import Input from '$lib/components/ui/Input.svelte';
  import Select from '$lib/components/ui/Select.svelte';
  import Table from '$lib/components/ui/Table.svelte';
  import Alert from '$lib/components/ui/Alert.svelte';
  import Modal from '$lib/components/ui/Modal.svelte';
  import { Plus, Edit, Trash2, Search } from 'lucide-svelte';

  let products = [];
  let categories = [];
  let suppliers = [];
  let loading = true;
  let error = '';
  let success = '';
  let search = '';
  
  // Modal state
  let modalOpen = false;
  let modalMode = 'create'; // 'create' o 'edit'
  let editingProduct = null;
  let formLoading = false;
  
  // Form data
  let form = {
    sku: '',
    name: '',
    description: '',
    price: 0,
    cost: 0,
    minimumStock: 0,
    maximumStock: 0,
    categoryId: '',
    supplierId: ''
  };

  onMount(async () => {
    await loadProducts();
    await loadCategories();
    await loadSuppliers();
  });

  async function loadProducts() {
    try {
      loading = true;
      error = '';
      const response = await productsApi.getAll({ search });
      products = response.items || response;
    } catch (err) {
      error = err.response?.data?.error || 'Error al cargar productos';
    } finally {
      loading = false;
    }
  }

  async function loadCategories() {
    try {
      const response = await categoriesApi.getAll();
      categories = Array.isArray(response) ? response : response.items || [];
    } catch (err) {
      console.error('Error loading categories:', err);
    }
  }

  async function loadSuppliers() {
    try {
      const response = await suppliersApi.getAll();
      suppliers = Array.isArray(response) ? response : response.items || [];
    } catch (err) {
      console.error('Error loading suppliers:', err);
    }
  }

  async function handleSearch() {
    await loadProducts();
  }

  function openCreateModal() {
    modalMode = 'create';
    editingProduct = null;
    form = {
      sku: '',
      name: '',
      description: '',
      price: 0,
      cost: 0,
      minimumStock: 0,
      maximumStock: 0,
      categoryId: '',
      supplierId: ''
    };
    error = '';
    success = '';
    modalOpen = true;
  }

  function openEditModal(product) {
    modalMode = 'edit';
    editingProduct = product;
    form = {
      sku: product.sku,
      name: product.name,
      description: product.description || '',
      price: product.price,
      cost: product.cost,
      minimumStock: product.minimumStock,
      maximumStock: product.maximumStock,
      categoryId: product.categoryId,
      supplierId: product.supplierId || ''
    };
    error = '';
    success = '';
    modalOpen = true;
  }

  function closeModal() {
    modalOpen = false;
    editingProduct = null;
  }

  async function handleSubmit(event) {
    event.preventDefault();
    formLoading = true;
    error = '';
    success = '';

    try {
      if (modalMode === 'create') {
        await productsApi.create(form);
        success = 'Producto creado exitosamente';
      } else {
        await productsApi.update(editingProduct.id, form);
        success = 'Producto actualizado exitosamente';
      }
      
      await loadProducts();
      setTimeout(() => {
        closeModal();
      }, 1000);
    } catch (err) {
      error = err.response?.data?.error || 'Error al guardar producto';
    } finally {
      formLoading = false;
    }
  }

  async function handleDelete(id) {
    if (!confirm('¿Estás seguro de eliminar este producto?')) return;

    try {
      await productsApi.delete(id);
      success = 'Producto eliminado exitosamente';
      await loadProducts();
      setTimeout(() => { success = ''; }, 3000);
    } catch (err) {
      error = err.response?.data?.error || 'Error al eliminar producto';
    }
  }

  function formatPrice(price) {
    return new Intl.NumberFormat('es-CR', {
      style: 'currency',
      currency: 'CRC'
    }).format(price);
  }

  $: categoryOptions = categories.map(c => ({ value: c.id, label: c.name }));
  $: supplierOptions = suppliers.map(s => ({ value: s.id, label: s.name }));
</script>

<svelte:head>
  <title>Productos - Inventory System</title>
</svelte:head>

<div class="space-y-6">
  <!-- Header -->
  <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
    <div>
      <h1 class="text-3xl font-bold text-gray-900">Productos</h1>
      <p class="text-gray-600 mt-1">Gestiona tu catálogo de productos</p>
    </div>
    <Button variant="primary" on:click={openCreateModal}>
      <Plus class="w-4 h-4 inline mr-2" />
      Nuevo Producto
    </Button>
  </div>

  <Alert type="error" message={error} />
  <Alert type="success" message={success} />

  <!-- Filters -->
  <div class="card">
    <div class="flex gap-4">
      <div class="flex-1">
        <Input
          type="text"
          bind:value={search}
          placeholder="Buscar por nombre o SKU..."
          on:input={handleSearch}
        />
      </div>
      <Button variant="secondary" on:click={handleSearch}>
        <Search class="w-4 h-4 inline mr-2" />
        Buscar
      </Button>
    </div>
  </div>

  <!-- Table -->
  <div class="card">
    {#if loading}
      <div class="text-center py-12">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600 mx-auto"></div>
        <p class="mt-4 text-gray-600">Cargando productos...</p>
      </div>
    {:else if products.length === 0}
      <div class="text-center py-12">
        <p class="text-gray-500">No hay productos disponibles</p>
      </div>
    {:else}
      <Table headers={['SKU', 'Nombre', 'Categoría', 'Precio', 'Stock', 'Estado', 'Acciones']}>
        {#each products as product}
          <tr class="hover:bg-gray-50">
            <td class="font-mono text-sm">{product.sku}</td>
            <td class="font-medium">{product.name}</td>
            <td>{product.categoryName}</td>
            <td>{formatPrice(product.price)}</td>
            <td>
              <span 
                class="px-2 py-1 text-xs font-medium rounded-full"
                class:bg-green-100={product.currentStock > product.minimumStock}
                class:text-green-700={product.currentStock > product.minimumStock}
                class:bg-yellow-100={product.currentStock <= product.minimumStock && product.currentStock > 0}
                class:text-yellow-700={product.currentStock <= product.minimumStock && product.currentStock > 0}
                class:bg-red-100={product.currentStock === 0}
                class:text-red-700={product.currentStock === 0}
              >
                {product.currentStock}
              </span>
            </td>
            <td>
              <span 
                class="px-2 py-1 text-xs font-medium rounded-full"
                class:bg-green-100={product.status === 'Active'}
                class:text-green-700={product.status === 'Active'}
                class:bg-gray-100={product.status === 'Inactive'}
                class:text-gray-700={product.status === 'Inactive'}
              >
                {product.status === 'Active' ? 'Activo' : 'Inactivo'}
              </span>
            </td>
            <td>
              <div class="flex gap-2">
                <button
                  on:click={() => openEditModal(product)}
                  class="text-primary-600 hover:text-primary-700 p-1"
                  title="Editar"
                >
                  <Edit class="w-4 h-4" />
                </button>
                <button
                  on:click={() => handleDelete(product.id)}
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

<!-- Modal Create/Edit -->
<Modal 
  isOpen={modalOpen} 
  title={modalMode === 'create' ? 'Nuevo Producto' : 'Editar Producto'}
  onClose={closeModal}
>
  <form on:submit={handleSubmit} class="space-y-4">
    <Alert type="error" message={error} />
    <Alert type="success" message={success} />

    <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
      <div>
        <label for="sku" class="label">SKU *</label>
        <Input
          type="text"
          id="sku"
          bind:value={form.sku}
          placeholder="PROD-001"
          required
          disabled={formLoading || modalMode === 'edit'}
        />
      </div>

      <div>
        <label for="name" class="label">Nombre *</label>
        <Input
          type="text"
          id="name"
          bind:value={form.name}
          placeholder="Nombre del producto"
          required
          disabled={formLoading}
        />
      </div>
    </div>

    <div>
      <label for="description" class="label">Descripción</label>
      <textarea
        id="description"
        bind:value={form.description}
        placeholder="Descripción del producto"
        rows="3"
        class="input"
        disabled={formLoading}
      ></textarea>
    </div>

    <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
      <div>
        <label for="price" class="label">Precio *</label>
        <Input
          type="number"
          id="price"
          bind:value={form.price}
          placeholder="0.00"
          step="0.01"
          required
          disabled={formLoading}
        />
      </div>

      <div>
        <label for="cost" class="label">Costo *</label>
        <Input
          type="number"
          id="cost"
          bind:value={form.cost}
          placeholder="0.00"
          step="0.01"
          required
          disabled={formLoading}
        />
      </div>
    </div>

    <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
      <div>
        <label for="minimumStock" class="label">Stock Mínimo *</label>
        <Input
          type="number"
          id="minimumStock"
          bind:value={form.minimumStock}
          placeholder="10"
          required
          disabled={formLoading}
        />
      </div>

      <div>
        <label for="maximumStock" class="label">Stock Máximo *</label>
        <Input
          type="number"
          id="maximumStock"
          bind:value={form.maximumStock}
          placeholder="100"
          required
          disabled={formLoading}
        />
      </div>
    </div>

    <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
      <div>
        <label for="categoryId" class="label">Categoría *</label>
        <Select
          bind:value={form.categoryId}
          options={categoryOptions}
          placeholder="Seleccionar categoría"
          required
          disabled={formLoading}
        />
      </div>

      <div>
        <label for="supplierId" class="label">Proveedor</label>
        <Select
          bind:value={form.supplierId}
          options={supplierOptions}
          placeholder="Seleccionar proveedor"
          disabled={formLoading}
        />
      </div>
    </div>

    <div class="flex gap-4 pt-4">
      <Button
        type="submit"
        variant="primary"
        fullWidth
        disabled={formLoading}
      >
        {formLoading ? 'Guardando...' : (modalMode === 'create' ? 'Crear Producto' : 'Actualizar Producto')}
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