<script>
  export let headers = [];
</script>

<div class="overflow-x-auto">
  <table class="table">
    <thead>
      <tr>
        {#each headers as header}
          <th>{header}</th>
        {/each}
      </tr>
    </thead>
    <tbody>
      <slot />
    </tbody>
  </table>
</div>