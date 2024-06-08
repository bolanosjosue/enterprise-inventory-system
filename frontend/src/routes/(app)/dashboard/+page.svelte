<script>
  import { onMount } from 'svelte';
  import StatsCard from '$lib/components/features/dashboard/StatsCard.svelte';

  let stats = {
    totalProducts: 0,
    lowStockProducts: 0,
    totalValue: 0,
    recentMovements: 0
  };

  let loading = true;

  onMount(async () => {
    setTimeout(() => {
      stats = {
        totalProducts: 127,
        lowStockProducts: 8,
        totalValue: 45820,
        recentMovements: 24
      };
      loading = false;
    }, 500);
  });
</script>

<svelte:head>
  <title>Dashboard - Inventory System</title>
</svelte:head>

<div class="space-y-6">
  <!-- Header -->
  <div>
    <h1 class="text-3xl font-bold text-gray-900">Dashboard</h1>
    <p class="text-gray-600 mt-1">Resumen general del inventario</p>
  </div>

  {#if loading}
    <div class="text-center py-12">
      <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600 mx-auto"></div>
      <p class="mt-4 text-gray-600">Cargando...</p>
    </div>
  {:else}
    <!-- Stats Grid -->
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
      <StatsCard
        title="Total Productos"
        value={stats.totalProducts.toString()}
        align="center"
      />

      <StatsCard
        title="Stock Bajo"
        value={stats.lowStockProducts.toString()}
        align="center"
      />

      <StatsCard
        title="Valor Total"
        value={"$" + stats.totalValue.toLocaleString()}
        align="center"
      />

      <StatsCard
        title="Movimientos (Hoy)"
        value={stats.recentMovements.toString()}
        align="center"
      />
    </div>

    <!-- Quick Actions -->
    <div class="card">
      <h2 class="text-xl font-bold text-gray-900 mb-6 text-center">
        Acciones Rápidas
      </h2>

      <div class="flex flex-wrap justify-center gap-4">
        <a href="/products/create" class="btn btn-primary">
          Nuevo Producto
        </a>

        <a href="/movements/purchase" class="btn btn-secondary">
          Registrar Compra
        </a>

        <a href="/movements/sale" class="btn btn-secondary">
          Registrar Venta
        </a>
      </div>
    </div>

    <!-- Recent Activity -->
    <div class="card">
      <h2 class="text-xl font-bold text-gray-900 mb-4">Actividad Reciente</h2>
      <p class="text-gray-500 text-center py-8">
        No hay movimientos recientes
      </p>
    </div>
  {/if}
</div>
