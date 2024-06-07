<script>
  export let type = 'text';
  export let value = '';
  export let placeholder = '';
  export let required = false;
  export let disabled = false;
</script>

<input
  {type}
  {placeholder}
  {required}
  {disabled}
  bind:value
  class="input"
  on:input
/>