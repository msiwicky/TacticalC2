<script setup lang="ts">
import type { UnitHistoryEntry } from "~/composables/useApiClient";

const { units, isConnected, connect } = useSignalR();
const playbackEntry = ref<UnitHistoryEntry | null>(null);

onMounted(() => {
	connect();
});

function handlePlaybackPosition(entry: UnitHistoryEntry | null) {
	playbackEntry.value = entry;
}
</script>

<template>
	<div class="flex h-screen">
		<div class="flex flex-col">
			<UnitList :units="units" />
			<PlaybackPanel
				:units="units"
				@playback-position="handlePlaybackPosition"
			/>
		</div>
		<div class="flex-1 p-4">
			<h1 class="text-2xl font-bold mb-2">Tactical C2 Dashboard</h1>
			<p class="text-sm text-neutral-500 mb-4">
				Connected: {{ isConnected }}
			</p>
			<TacticalMap :units="units" :playback-entry="playbackEntry" />
		</div>
	</div>
</template>
