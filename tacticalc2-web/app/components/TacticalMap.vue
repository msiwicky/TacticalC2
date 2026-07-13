<script setup lang="ts">
import maplibregl from "maplibre-gl";
import "maplibre-gl/dist/maplibre-gl.css";
import type { Unit } from "~/types/Unit";

const props = defineProps<{
	units: Map<string, Unit>;
}>();

const mapContainer = ref<HTMLDivElement | null>(null);
let map: maplibregl.Map | null = null;
const markers = new Map<string, maplibregl.Marker>();

onMounted(() => {
	map = new maplibregl.Map({
		container: mapContainer.value!,
		style: "https://demotiles.maplibre.org/style.json",
		center: [19.94, 50.06],
		zoom: 10,
	});
});

watch(
	() => props.units,
	(units) => {
		if (!map) return;

		for (const unit of units.values()) {
			let marker = markers.get(unit.id);

			if (!marker) {
				marker = new maplibregl.Marker({
					color: getColorForType(unit.type),
				})
					.setLngLat([unit.longitude, unit.latitude])
					.addTo(map);
				markers.set(unit.id, marker);
			} else {
				marker.setLngLat([unit.longitude, unit.latitude]);
			}
		}
	},
	{ deep: true },
);

function getColorForType(type: number): string {
	switch (type) {
		case 0:
			return "#3b82f6"; // Drone - niebieski
		case 1:
			return "#22c55e"; // Vehicle - zielony
		case 2:
			return "#f97316"; // Infantry - pomarańczowy
		default:
			return "#6b7280";
	}
}
</script>

<template>
	<div ref="mapContainer" style="width: 100%; height: 600px"></div>
</template>
