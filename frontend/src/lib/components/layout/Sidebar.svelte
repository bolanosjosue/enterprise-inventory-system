<script>
  import { page } from '$app/stores';
  import { authStore } from '$lib/stores/auth';
  import { 
    LayoutDashboard, 
    Package, 
    FolderTree, 
    Users as UsersIcon, 
    Warehouse, 
    TrendingUp,
    X,
    UserCog
  } from 'lucide-svelte';

  export let isOpen = false;
  export let onClose = () => {};

  let currentUser = null;
  
  authStore.subscribe((state) => {
    currentUser = state.user;
  });

  const menuItems = [
    { href: '/dashboard', icon: LayoutDashboard, label: 'Dashboard' },
    { href: '/products', icon: Package, label: 'Productos' },
    { href: '/categories', icon: FolderTree, label: 'Categorías' },
    { href: '/suppliers', icon: UsersIcon, label: 'Proveedores' },
    { href: '/warehouses', icon: Warehouse, label: 'Bodegas' },
    { href: '/movements', icon: TrendingUp, label: 'Movimientos' },
    { href: '/users', icon: UserCog, label: 'Usuarios', adminOnly: true }
  ];

  $: currentPath = $page.url.pathname;
  $: filteredMenuItems = menuItems.filter(item => 
    !item.adminOnly || currentUser?.role === 'Admin'
  );

  function handleLinkClick() {
    onClose();
  }
</script>

<!-- Mobile backdrop -->
{#if isOpen}
  <div 
    class="fixed inset-0 bg-black bg-opacity-50 z-40 lg:hidden"
    on:click={onClose}
    role="button"
    tabindex="0"
    on:keydown={(e) => e.key === 'Escape' && onClose()}
  ></div>
{/if}

<!-- Sidebar -->
<aside 
  class="fixed left-0 top-0 h-full w-64 bg-white border-r border-gray-200 z-50 transform transition-transform duration-300 lg:translate-x-0 lg:top-16 lg:h-[calc(100vh-4rem)]"
  class:-translate-x-full={!isOpen}
>
  <!-- Mobile header -->
  <div class="lg:hidden flex items-center justify-between p-4 border-b border-gray-200">
    <span class="text-lg font-bold text-gray-900">Menú</span>
    <button 
      on:click={onClose}
      class="p-2 rounded-lg hover:bg-gray-100"
      aria-label="Cerrar menú"
    >
      <X class="w-5 h-5" />
    </button>
  </div>
  
  <!-- Navigation -->
  <nav class="flex-1 px-4 py-6 space-y-1">
    {#each menuItems as item}
      <a
        href={item.href}
        on:click={handleLinkClick}
        class="flex items-center gap-3 px-4 py-3 rounded-lg transition-colors"
        class:bg-primary-50={currentPath.startsWith(item.href)}
        class:text-primary-700={currentPath.startsWith(item.href)}
        class:font-medium={currentPath.startsWith(item.href)}
        class:text-gray-700={!currentPath.startsWith(item.href)}
        class:hover:bg-gray-50={!currentPath.startsWith(item.href)}
      >
        <svelte:component this={item.icon} class="w-5 h-5" />
        {item.label}
      </a>
    {/each}
  </nav>
</aside>