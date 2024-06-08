<script>
  import { authStore } from '$lib/stores/auth';
  import { goto } from '$app/navigation';
  import { Menu, LogOut, User, X } from 'lucide-svelte';

  export let onToggleSidebar = () => {};

  let user = null;
  let showUserMenu = false;

  authStore.subscribe((state) => {
    user = state.user;
  });

  function handleLogout() {
    authStore.logout();
    goto('/login');
  }

  function toggleUserMenu() {
    showUserMenu = !showUserMenu;
  }
</script>

<nav class="bg-white border-b border-gray-200 fixed w-full z-30 top-0">
  <div class="px-4 lg:px-6 py-3">
    <div class="flex items-center justify-between">
      
      <!-- Left: Menu + Logo -->
      <div class="flex items-center gap-3">
        <button 
          on:click={onToggleSidebar}
          class="lg:hidden text-gray-600 hover:text-gray-900 p-2 rounded-lg hover:bg-gray-100"
        >
          <Menu class="w-6 h-6" />
        </button>
        
        <div class="flex items-center gap-2">
          <div class="p-2 bg-primary-600 rounded-lg">
            <svg class="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4" />
            </svg>
          </div>
          <span class="text-xl font-bold text-gray-900 hidden sm:block">InventoryPro</span>
        </div>
      </div>

      <!-- Right: User Menu -->
      <div class="relative">
        <button 
          on:click={toggleUserMenu}
          class="flex items-center gap-3 px-3 py-2 rounded-lg hover:bg-gray-100 transition-colors"
        >
          <div class="text-right hidden sm:block">
            <p class="text-sm font-medium text-gray-900">{user?.fullName || 'Usuario'}</p>
            <p class="text-xs text-gray-500">{user?.role || 'Operator'}</p>
          </div>
          <div class="w-10 h-10 bg-primary-100 rounded-full flex items-center justify-center">
            <User class="w-5 h-5 text-primary-700" />
          </div>
        </button>

        {#if showUserMenu}
          <div class="absolute right-0 mt-2 w-48 bg-white rounded-xl shadow-lg border border-gray-200 py-1">
            <div class="px-4 py-2 border-b border-gray-100 sm:hidden">
              <p class="text-sm font-medium text-gray-900">{user?.fullName}</p>
              <p class="text-xs text-gray-500">{user?.role}</p>
            </div>
            <button 
              on:click={handleLogout}
              class="w-full px-4 py-2 text-left text-sm text-red-600 hover:bg-red-50 flex items-center gap-2"
            >
              <LogOut class="w-4 h-4" />
              Cerrar Sesión
            </button>
          </div>
        {/if}
      </div>
    </div>
  </div>
</nav>