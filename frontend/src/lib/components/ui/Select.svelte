<script>
  export let value = '';
  export let options = [];
  export let placeholder = 'Seleccionar...';
  export let required = false;
  export let disabled = false;
</script>

<select
  bind:value
  {required}
  {disabled}
  class="input"
>
  <option value="" disabled selected>{placeholder}</option>
  {#each options as option}
    <option value={option.value}>
      {option.label}
    </option>
  {/each}
</select>