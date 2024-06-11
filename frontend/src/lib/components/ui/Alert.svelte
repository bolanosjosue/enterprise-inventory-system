<script>
  export let type = 'info'; // info, success, warning, error
  export let message = '';
  export let errors = null; // Puede ser string, array, u objeto
</script>

{#if message || errors}
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
    {#if message}
      <p class="font-medium">{message}</p>
    {/if}

    {#if errors}
      {#if typeof errors === 'string'}
        <p class="font-medium">{errors}</p>
      {:else if Array.isArray(errors)}
        <ul class="list-disc list-inside space-y-1 mt-2">
          {#each errors as error}
            <li class="text-sm">{error}</li>
          {/each}
        </ul>
      {:else if typeof errors === 'object'}
        <ul class="list-disc list-inside space-y-1 mt-2">
          {#each Object.entries(errors) as [field, messages]}
            {#if Array.isArray(messages)}
              {#each messages as msg}
                <li class="text-sm"><strong>{field}:</strong> {msg}</li>
              {/each}
            {:else}
              <li class="text-sm"><strong>{field}:</strong> {messages}</li>
            {/if}
          {/each}
        </ul>
      {/if}
    {/if}
  </div>
{/if}