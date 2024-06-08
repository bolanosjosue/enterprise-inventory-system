<script>
  export let title = '';
  export let value = '';
  export let icon = null;
  export let color = 'blue';
  export let trend = null;

  $: bgColorClass = {
    blue: 'bg-blue-100',
    green: 'bg-green-100',
    yellow: 'bg-yellow-100',
    red: 'bg-red-100'
  }[color] || 'bg-blue-100';

  $: iconColorClass = {
    blue: 'text-blue-600',
    green: 'text-green-600',
    yellow: 'text-yellow-600',
    red: 'text-red-600'
  }[color] || 'text-blue-600';
</script>

<div class="card flex flex-col items-center justify-center text-center">
  {#if icon}
    <div class="mb-4 p-3 rounded-xl {bgColorClass}">
      <svelte:component this={icon} class="w-8 h-8 {iconColorClass}" />
    </div>
  {/if}

  <p class="text-sm text-gray-600 mb-1">{title}</p>

  <p class="text-3xl font-bold text-gray-900">{value}</p>

  {#if trend}
    <p
      class="text-sm mt-2"
      class:text-green-600={trend.isPositive}
      class:text-red-600={!trend.isPositive}
    >
      {trend.value}
    </p>
  {/if}
</div>
