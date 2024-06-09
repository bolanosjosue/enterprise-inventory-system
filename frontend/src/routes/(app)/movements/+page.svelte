<script>
  import { onMount } from 'svelte';
  import { goto } from '$app/navigation';
  import { stockMovementsApi } from '$lib/api/stockMovements.api';
  import Button from '$lib/components/ui/Button.svelte';
  import Table from '$lib/components/ui/Table.svelte';
  import Alert from '$lib/components/ui/Alert.svelte';
  import { Plus, ShoppingCart, TrendingUp, ArrowRightLeft } from 'lucide-svelte';
  import { format } from 'date-fns';
  import { es } from 'date-fns/locale';

  let movements = [];
  let loading = true;
  let error = '';

  onMount(async () => {
    await loadMovements();
  });

  async function loadMovements() {
    try {
      loading = true;
      error = '';
      const response = await stockMovementsApi.getAll();
      movements = response.items || response;
    } catch (err) {
      error = err.response?.data?.error || 'Error al cargar movimientos';
    } finally {
      loading = false;
    }
  }

  function formatDate(dateString) {
    return format(new Date(dateString), "dd MMM yyyy HH:mm", { locale: es });
  }

  function formatPrice(price) {
    return new Intl.NumberFormat('es-CR', {
      style: 'currency',
      currency: 'CRC'
    }).format(price);
  }

  function getTypeLabel(type) {
    const types = {
      'Entry': 'Entrada',
      'Exit': 'Salida',
      'Transfer': 'Transferencia',
      'Adjustment': 'Ajuste'
    };
    return types[type] || type;
  }

  function getTypeColor(type) {
    const colors = {
      'Entry': 'bg-green-100 text-green-700',
      'Exit': 'bg-red-100 text-red-700',
      'Transfer': 'bg-blue-100 text-blue-700',
      'Adjustment': 'bg-yellow-100 text-yellow-700'
    };
    return colors[type] || 'bg-gray-100 text-gray-700';
  }
</script>

<svelte:head>
  <title>Movimientos - Inventory System</title>
</svelte:head>

<div class="space-y-6">
  <!-- Header -->
  <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
    <div>
      <h1 class="text-3xl font-bold text-gray-900">Movimientos de Inventario</h1>
      <p class="text-gray-600 mt-1">Historial de entradas, salidas y transferencias</p>
    </div>
    
    <div class="flex flex-wrap gap-2">
      <Button variant="primary" on:click={() => goto('/movements/purchase')}>
        <ShoppingCart class="w-4 h-4 inline mr-2" />
        Compra
      </Button>
      <Button variant="secondary" on:click={() => goto('/movements/sale')}>
        <TrendingUp class="w-4 h-4 inline mr-2" />
        Venta
      </Button>
      <Button variant="secondary" on:click={() => goto('/movements/transfer')}>
        <ArrowRightLeft class="w-4 h-4 inline mr-2" />
        Transferir
      </Button>
    </div>
  </div>

  <Alert type="error" message={error} />

  <!-- Table -->
  <div class="card">
    {#if loading}
      <div class="text-center py-12">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600 mx-auto"></div>
        <p class="mt-4 text-gray-600">Cargando movimientos...</p>
      </div>
    {:else if movements.length === 0}
      <div class="text-center py-12">
        <p class="text-gray-500">No hay movimientos registrados</p>
      </div>
    {:else}
      <Table headers={['Tipo', 'Producto', 'Bodega', 'Cantidad', 'Precio Unit.', 'Total', 'Referencia', 'Fecha']}>
        {#each movements as movement}
          <tr class="hover:bg-gray-50">
            <td>
              <span class="px-2 py-1 text-xs font-medium rounded-full {getTypeColor(movement.type)}">
                {getTypeLabel(movement.type)}
              </span>
            </td>
            <td class="font-medium">
              <div>{movement.productName}</div>
              <div class="text-xs text-gray-500">{movement.productSku}</div>
            </td>
            <td>
              <div>{movement.warehouseName}</div>
              {#if movement.destinationWarehouseName}
                <div class="text-xs text-gray-500">→ {movement.destinationWarehouseName}</div>
              {/if}
            </td>
            <td class="font-mono">{movement.quantity}</td>
            <td>{formatPrice(movement.unitPrice)}</td>
            <td class="font-semibold">{formatPrice(movement.totalAmount)}</td>
            <td class="text-sm text-gray-600">{movement.reference}</td>
            <td class="text-sm text-gray-500">{formatDate(movement.createdAt)}</td>
          </tr>
        {/each}
      </Table>
    {/if}
  </div>
</div>