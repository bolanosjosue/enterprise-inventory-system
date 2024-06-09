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
  import { ArrowLeft, ArrowRight } from 'lucide-svelte';

  let products = [];
  let warehouses = [];
  let loading = false;
  let error = '';
  let success = '';
  let availableStock = null;

  let form = {
    productId: '',
    sourceWarehouseId: '',
    destinationWarehouseId: '',
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
    if (form.productId && form.sourceWarehouseId) {
      try {
        const stockData = await stockMovementsApi.getStockByWarehouse(form.sourceWarehouseId);
        const productStock = stockData.find(s => s.productId === form.productId);
        availableStock = productStock ? productStock.currentStock : 0;
      } catch (err) {
        availableStock = 0;
      }
    }
  }

  async function handleSubmit(event) {
    event.preventDefault();
    
    if (form.sourceWarehouseId === form.destinationWarehouseId) {
      error = 'Las bodegas de origen y destino deben ser diferentes';
      return;
    }

    loading = true;
    error = '';
    success = '';

    try {
      await stockMovementsApi.transferStock(form);
      success = 'Transferencia registrada exitosamente';
      
      form = {
        productId: '',
        sourceWarehouseId: '',
        destinationWarehouseId: '',
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
      error = err.response?.data?.error || 'Error al registrar transferencia';
    } finally {
      loading = false;
    }
  }

  $: productOptions = products.map(p => ({ value: p.id, label: `${p.sku} - ${p.name}` }));
  $: warehouseOptions = warehouses.map(w => ({ value: w.id, label: w.name }));
  $: destinationWarehouseOptions = warehouses
    .filter(w => w.id !== form.sourceWarehouseId)
    .map(w => ({ value: w.id, label: w.name }));
  $: if (form.productId && form.sourceWarehouseId) checkStock();
</script>

<svelte:head>
  <title>Transferir Stock - Inventory System</title>
</svelte:head>

<div class="max-w-3xl mx-auto space-y-6">
  <!-- Header -->
  <div class="flex items-center gap-4">
    <button on:click={() => goto('/movements')} class="text-gray-600 hover:text-gray-900">
      <ArrowLeft class="w-6 h-6" />
    </button>
    <div>
      <h1 class="text-3xl font-bold text-gray-900">Transferir Stock</h1>
      <p class="text-gray-600 mt-1">Entre bodegas</p>
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

      <!-- Warehouses Flow -->
      <div class="grid grid-cols-1 md:grid-cols-[1fr,auto,1fr] gap-4 items-end">
        <div>
          <label for="sourceWarehouseId" class="label">Bodega Origen *</label>
          <Select
            bind:value={form.sourceWarehouseId}
            options={warehouseOptions}
            placeholder="Seleccionar origen"
            required
            disabled={loading}
          />
        </div>

        <div class="hidden md:flex items-center justify-center pb-2">
          <ArrowRight class="w-8 h-8 text-primary-600" />
        </div>

        <div>
          <label for="destinationWarehouseId" class="label">Bodega Destino *</label>
          <Select
            bind:value={form.destinationWarehouseId}
            options={destinationWarehouseOptions}
            placeholder="Seleccionar destino"
            required
            disabled={loading || !form.sourceWarehouseId}
          />
        </div>
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
            Stock disponible en bodega origen: <span class="text-2xl font-bold">{availableStock}</span> unidades
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
          <p class="text-xs text-gray-500 mt-1">Valor de referencia</p>
        </div>
      </div>

      <div>
        <label for="reference" class="label">Referencia *</label>
        <Input
          type="text"
          id="reference"
          bind:value={form.reference}
          placeholder="TRANS-2024-001"
          required
          disabled={loading}
        />
        <p class="text-xs text-gray-500 mt-1">Código de transferencia</p>
      </div>

      <div>
        <label for="notes" class="label">Notas</label>
        <textarea
          id="notes"
          bind:value={form.notes}
          placeholder="Motivo de la transferencia..."
          rows="3"
          class="input"
          disabled={loading}
        ></textarea>
      </div>

      <div class="flex gap-4">
        <Button
          type="submit"
          variant="primary"
          fullWidth
          disabled={loading || availableStock === 0}
        >
          {loading ? 'Transfiriendo...' : 'Transferir Stock'}
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