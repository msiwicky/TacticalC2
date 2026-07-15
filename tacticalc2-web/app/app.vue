<script setup lang="ts">
const { units, isConnected, connect } = useSignalR();

onMounted(() => {
	connect();
});
</script>

<template>
	<div class="flex h-screen">
		<UnitList :units="units" />
		<div class="flex-1 p-4">
			<h1 class="text-2xl font-bold mb-2">Tactical C2 Dashboard</h1>
			<p class="text-sm text-neutral-500 mb-4">
				Connected: {{ isConnected }}
			</p>
			<TacticalMap :units="units" />
		</div>
	</div>
</template>
