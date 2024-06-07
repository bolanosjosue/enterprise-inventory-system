<script>
  export let variant = 'primary';
  export let type = 'button';
  export let disabled = false;
  export let fullWidth = false;
</script>

<button
  {type}
  {disabled}
  class="btn btn-{variant}"
  class:w-full={fullWidth}
  on:click
>
  <slot />
</button>