<script setup lang="ts">
const { units, isConnected, connect } = useSignalR();

onMounted(() => {
	connect();
});
</script>

<template>
	<div>
		<h1>Tactical C2 Dashboard</h1>
		<p>Connected: {{ isConnected }}</p>
		<TacticalMap :units="units" />
	</div>
</template>
