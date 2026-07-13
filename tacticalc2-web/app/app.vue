<script setup lang="ts">
import { useSignalR } from "./composables/useSignalR";

const { units, isConnected, connect } = useSignalR();

onMounted(() => {
	connect();
});
</script>

<template>
	<div>
		<h1>Tactical C2 Dashboard</h1>
		<p>Connected: {{ isConnected }}</p>
		<ul>
			<li v-for="unit in units.values()" :key="unit.id">
				{{ unit.name }}: {{ unit.latitude.toFixed(5) }},
				{{ unit.longitude.toFixed(5) }}
			</li>
		</ul>
	</div>
</template>
