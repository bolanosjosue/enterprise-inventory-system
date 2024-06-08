<script>
  import { onMount } from 'svelte';
  import { goto } from '$app/navigation';
  import { authStore } from '$lib/stores/auth';
  import Navbar from '$lib/components/layout/Navbar.svelte';
  import Sidebar from '$lib/components/layout/Sidebar.svelte';

  let isAuthenticated = false;
  let loading = true;
  let sidebarOpen = false;

  const unsubscribe = authStore.subscribe((state) => {
    isAuthenticated = state.isAuthenticated;
    loading = state.loading;
  });

  onMount(() => {
    if (!loading && !isAuthenticated) {
      goto('/login');
    }
  });

  $: if (!loading && !isAuthenticated) {
    goto('/login');
  }

  function toggleSidebar() {
    sidebarOpen = !sidebarOpen;
  }

  function closeSidebar() {
    sidebarOpen = false;
  }
</script>

{#if loading}
  <div class="min-h-screen flex items-center justify-center">
    <div class="text-center">
      <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600 mx-auto"></div>
      <p class="mt-4 text-gray-600">Cargando...</p>
    </div>
  </div>
{:else if isAuthenticated}
  <div class="min-h-screen bg-gray-50">
    <Navbar onToggleSidebar={toggleSidebar} />
    <Sidebar isOpen={sidebarOpen} onClose={closeSidebar} />
    
    <main class="pt-16 lg:pl-64 min-h-screen">
      <div class="p-6">
        <slot />
      </div>
    </main>
  </div>
{/if}