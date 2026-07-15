<script setup lang="ts">
import type { Unit } from "~/types/Unit";
import type { UnitHistoryEntry } from "~/composables/useApiClient";

const props = defineProps<{
	units: Map<string, Unit>;
}>();

const emit = defineEmits<{
	playbackPosition: [entry: UnitHistoryEntry | null];
}>();

const { getUnitHistory } = useApiClient();

const selectedUnitId = ref<string>("");
const history = ref<UnitHistoryEntry[]>([]);
const currentIndex = ref(0);
const isLoading = ref(false);

const unitsList = computed(() => Array.from(props.units.values()));

const currentEntry = computed(() => history.value[currentIndex.value]);

async function loadHistory() {
	if (!selectedUnitId.value) return;

	isLoading.value = true;
	const to = new Date();
	const from = new Date(to.getTime() - 10 * 60 * 1000); // ostatnie 10 minut

	history.value = await getUnitHistory(selectedUnitId.value, from, to);
	currentIndex.value = 0;
	isLoading.value = false;

	emitCurrentPosition();
}

function emitCurrentPosition() {
	emit("playbackPosition", currentEntry.value ?? null);
}

watch(currentIndex, emitCurrentPosition);

const isPlaying = ref(false);
const playbackSpeed = ref(1); // mnożnik: 1x, 2x, 4x
let playbackInterval: ReturnType<typeof setInterval> | null = null;

function togglePlay() {
	if (isPlaying.value) {
		stopPlayback();
	} else {
		startPlayback();
	}
}

function startPlayback() {
	if (history.value.length === 0) return;

	isPlaying.value = true;
	playbackInterval = setInterval(() => {
		if (currentIndex.value >= history.value.length - 1) {
			stopPlayback();
			return;
		}
		currentIndex.value++;
	}, 1000 / playbackSpeed.value);
}

function stopPlayback() {
	isPlaying.value = false;
	if (playbackInterval) {
		clearInterval(playbackInterval);
		playbackInterval = null;
	}
}

watch(selectedUnitId, stopPlayback);

watch(playbackSpeed, () => {
	if (isPlaying.value) {
		stopPlayback();
		startPlayback();
	}
});

onUnmounted(stopPlayback);
</script>

<template>
	<div class="bg-neutral-900 text-white p-4">
		<h3 class="font-semibold mb-2">Playback</h3>

		<select
			v-model="selectedUnitId"
			@change="loadHistory"
			class="bg-neutral-800 text-white p-2 rounded w-full mb-3"
		>
			<option value="" disabled>Select unit...</option>
			<option v-for="unit in unitsList" :key="unit.id" :value="unit.id">
				{{ unit.name }}
			</option>
		</select>

		<div v-if="isLoading">Loading...</div>

		<div v-else-if="history.length > 0">
			<input
				type="range"
				:min="0"
				:max="history.length - 1"
				v-model.number="currentIndex"
				class="w-full"
			/>

			<div class="flex items-center gap-2 mt-2">
				<button
					@click="togglePlay"
					class="bg-blue-600 hover:bg-blue-700 px-3 py-1 rounded text-sm"
				>
					{{ isPlaying ? "Pause" : "Play" }}
				</button>

				<select
					v-model.number="playbackSpeed"
					class="bg-neutral-800 text-white p-1 rounded text-sm"
				>
					<option :value="1">1x</option>
					<option :value="2">2x</option>
					<option :value="4">4x</option>
				</select>

				<p class="text-sm text-neutral-400" v-if="currentEntry">
					{{
						new Date(currentEntry.timestampUtc).toLocaleTimeString()
					}}
					({{ currentIndex + 1 }} / {{ history.length }})
				</p>
			</div>
		</div>

		<div v-else-if="selectedUnitId" class="text-sm text-neutral-500">
			No history available
		</div>
	</div>
</template>
