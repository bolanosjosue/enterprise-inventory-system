<script>
  import { onMount } from 'svelte';
  import { goto } from '$app/navigation';
  import { stockMovementsApi } from '$lib/api/stockMovements.api';
  import { productsApi } from '$lib/api/products.api';
  import { warehousesApi } from '$lib/api/warehouses.api';
  import Button from '$lib/components/ui/Button.svelte';
  import Input from '$lib/components/ui/Input.svelte';
  import Select from '$lib/components/ui/Select.svelte';
  import Alert from '$lib/components/ui/Alert.svelte';
  import { ArrowLeft } from 'lucide-svelte';

  let products = [];
  let warehouses = [];
  let loading = false;
  let error = '';
  let success = '';
  let availableStock = null;

  let form = {
    productId: '',
    warehouseId: '',
    quantity: 0,
    unitPrice: 0,
    reference: '',
    notes: ''
  };

  onMount(async () => {
    await loadData();
  });

  async function loadData() {
    try {
      const [productsRes, warehousesRes] = await Promise.all([
        productsApi.getAll(),
        warehousesApi.getAll()
      ]);
      
      products = productsRes.items || productsRes;
      warehouses = warehousesRes.items || warehousesRes;
    } catch (err) {
      error = 'Error al cargar datos';
    }
  }

  async function checkStock() {
    if (form.productId && form.warehouseId) {
      try {
        const stockData = await stockMovementsApi.getStockByWarehouse(form.warehouseId);
        const productStock = stockData.find(s => s.productId === form.productId);
        availableStock = productStock ? productStock.currentStock : 0;
      } catch (err) {
        availableStock = 0;
      }
    }
  }

  async function handleSubmit(event) {
    event.preventDefault();
    loading = true;
    error = '';
    success = '';

    try {
      await stockMovementsApi.processSale(form);
      success = 'Venta registrada exitosamente';
      
      form = {
        productId: '',
        warehouseId: '',
        quantity: 0,
        unitPrice: 0,
        reference: '',
        notes: ''
      };
      availableStock = null;

      setTimeout(() => {
        goto('/movements');
      }, 1500);
    } catch (err) {
      error = err.response?.data?.error || 'Error al registrar venta';
    } finally {
      loading = false;
    }
  }

  $: productOptions = products.map(p => ({ value: p.id, label: `${p.sku} - ${p.name}` }));
  $: warehouseOptions = warehouses.map(w => ({ value: w.id, label: w.name }));
  $: if (form.productId && form.warehouseId) checkStock();
</script>

<svelte:head>
  <title>Registrar Venta - Inventory System</title>
</svelte:head>

<div class="max-w-3xl mx-auto space-y-6">
  <!-- Header -->
  <div class="flex items-center gap-4">
    <button on:click={() => goto('/movements')} class="text-gray-600 hover:text-gray-900">
      <ArrowLeft class="w-6 h-6" />
    </button>
    <div>
      <h1 class="text-3xl font-bold text-gray-900">Registrar Venta</h1>
      <p class="text-gray-600 mt-1">Salida de inventario</p>
    </div>
  </div>

  <!-- Form -->
  <div class="card">
    <form on:submit={handleSubmit} class="space-y-6">
      <Alert type="error" message={error} />
      <Alert type="success" message={success} />

      <div>
        <label for="productId" class="label">Producto *</label>
        <Select
          bind:value={form.productId}
          options={productOptions}
          placeholder="Seleccionar producto"
          required
          disabled={loading}
        />
      </div>

      <div>
        <label for="warehouseId" class="label">Bodega *</label>
        <Select
          bind:value={form.warehouseId}
          options={warehouseOptions}
          placeholder="Seleccionar bodega"
          required
          disabled={loading}
        />
      </div>

      {#if availableStock !== null}
        <div class="p-4 rounded-lg border-2"
             class:bg-green-50={availableStock > 0}
             class:border-green-200={availableStock > 0}
             class:bg-red-50={availableStock === 0}
             class:border-red-200={availableStock === 0}
        >
          <p class="text-sm font-medium"
             class:text-green-900={availableStock > 0}
             class:text-red-900={availableStock === 0}
          >
            Stock disponible: <span class="text-2xl font-bold">{availableStock}</span> unidades
          </p>
        </div>
      {/if}

      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div>
          <label for="quantity" class="label">Cantidad *</label>
          <Input
            type="number"
            id="quantity"
            bind:value={form.quantity}
            placeholder="0"
            min="1"
            max={availableStock || undefined}
            required
            disabled={loading || availableStock === 0}
          />
          {#if availableStock === 0}
            <p class="text-xs text-red-600 mt-1">Sin stock disponible</p>
          {/if}
        </div>

        <div>
          <label for="unitPrice" class="label">Precio Unitario *</label>
          <Input
            type="number"
            id="unitPrice"
            bind:value={form.unitPrice}
            placeholder="0.00"
            step="0.01"
            min="0"
            required
            disabled={loading}
          />
        </div>
      </div>

      <div>
        <label for="reference" class="label">Referencia *</label>
        <Input
          type="text"
          id="reference"
          bind:value={form.reference}
          placeholder="INV-2024-001"
          required
          disabled={loading}
        />
        <p class="text-xs text-gray-500 mt-1">Número de factura o ticket</p>
      </div>

      <div>
        <label for="notes" class="label">Notas</label>
        <textarea
          id="notes"
          bind:value={form.notes}
          placeholder="Notas adicionales..."
          rows="3"
          class="input"
          disabled={loading}
        ></textarea>
      </div>

      {#if form.quantity > 0 && form.unitPrice > 0}
        <div class="p-4 bg-primary-50 rounded-lg border border-primary-200">
          <p class="text-sm font-medium text-primary-900">Total de venta:</p>
          <p class="text-3xl font-bold text-primary-700">
            {new Intl.NumberFormat('es-CR', { style: 'currency', currency: 'CRC' }).format(form.quantity * form.unitPrice)}
          </p>
        </div>
      {/if}

      <div class="flex gap-4">
        <Button
          type="submit"
          variant="primary"
          fullWidth
          disabled={loading || availableStock === 0}
        >
          {loading ? 'Registrando...' : 'Registrar Venta'}
        </Button>
        <Button
          type="button"
          variant="secondary"
          fullWidth
          on:click={() => goto('/movements')}
          disabled={loading}
        >
          Cancelar
        </Button>
      </div>
    </form>
  </div>
</div>