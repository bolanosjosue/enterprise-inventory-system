<script>
  import { onMount } from 'svelte';
  import { authStore } from '$lib/stores/auth';
  import '../app.css';

  onMount(() => {
    authStore.init();
  });
</script>

<slot />