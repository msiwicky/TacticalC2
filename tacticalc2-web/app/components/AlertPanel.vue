<script setup lang="ts">
import type { Alert } from "~/composables/useSignalR";

defineProps<{
	alerts: Alert[];
}>();
</script>

<template>
	<div class="bg-neutral-900 text-white p-4 max-h-64 overflow-y-auto">
		<h3 class="font-semibold mb-2">Alerts ({{ alerts.length }})</h3>
		<ul class="space-y-1">
			<li
				v-for="alert in alerts"
				:key="alert.id"
				class="text-sm border-b border-neutral-700 py-1"
			>
				<span class="text-yellow-500">⚠</span> {{ alert.message }}
				<span class="text-neutral-500 text-xs block">{{
					new Date(alert.timestampUtc).toLocaleTimeString()
				}}</span>
			</li>
		</ul>
	</div>
</template>
