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
  import { parseApiError } from '$lib/utils/errorParser';
  

  let products = [];
  let warehouses = [];
  let loading = false;
  let error = '';
  
  let success = '';
  let errorDetails = null;

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

  async function handleSubmit(event) {
    event.preventDefault();
    loading = true;
    error = '';
    errorDetails = null;
    success = '';

    try {
        await stockMovementsApi.processPurchase(form);
        success = 'Compra registrada exitosamente';
        
        form = {
        productId: '',
        warehouseId: '',
        quantity: 0,
        unitPrice: 0,
        reference: '',
        notes: ''
        };

        setTimeout(() => {
        goto('/movements');
        }, 1500);
    } catch (err) {
        const parsed = parseApiError(err);
        error = parsed.message;
        errorDetails = parsed.errors;
    } finally {
        loading = false;
    }
    }

  $: productOptions = products.map(p => ({ value: p.id, label: `${p.sku} - ${p.name}` }));
  $: warehouseOptions = warehouses.map(w => ({ value: w.id, label: w.name }));
</script>

<svelte:head>
  <title>Registrar Compra - Inventory System</title>
</svelte:head>

<div class="max-w-3xl mx-auto space-y-6">
  <!-- Header -->
  <div class="flex items-center gap-4">
    <button on:click={() => goto('/movements')} class="text-gray-600 hover:text-gray-900">
      <ArrowLeft class="w-6 h-6" />
    </button>
    <div>
      <h1 class="text-3xl font-bold text-gray-900">Registrar Compra</h1>
      <p class="text-gray-600 mt-1">Entrada de inventario</p>
    </div>
  </div>

  <!-- Form -->
  <div class="card">
    <form on:submit={handleSubmit} class="space-y-6">
      <Alert type="error" message={error} errors={errorDetails} />
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

      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div>
          <label for="quantity" class="label">Cantidad *</label>
          <Input
            type="number"
            id="quantity"
            bind:value={form.quantity}
            placeholder="0"
            min="1"
            required
            disabled={loading}
          />
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
          placeholder="PO-2024-001"
          required
          disabled={loading}
        />
        <p class="text-xs text-gray-500 mt-1">Número de orden de compra o factura</p>
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
          <p class="text-sm font-medium text-primary-900">Total a pagar:</p>
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
          disabled={loading}
        >
          {loading ? 'Registrando...' : 'Registrar Compra'}
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