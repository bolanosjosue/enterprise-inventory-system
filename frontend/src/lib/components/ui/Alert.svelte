<script>
  export let type = 'info'; // info, success, warning, error
  export let message = '';
</script>

{#if message}
  <div
    class="p-4 rounded-lg mb-4"
    class:bg-blue-50={type === 'info'}
    class:bg-green-50={type === 'success'}
    class:bg-yellow-50={type === 'warning'}
    class:bg-red-50={type === 'error'}
    class:text-blue-800={type === 'info'}
    class:text-green-800={type === 'success'}
    class:text-yellow-800={type === 'warning'}
    class:text-red-800={type === 'error'}
  >
    {message}
  </div>
{/if}