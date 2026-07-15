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

function createPopup(unit: Unit): maplibregl.Popup {
	return new maplibregl.Popup({ offset: 25, closeButton: false }).setHTML(
		popupContent(unit),
	);
}

function popupContent(unit: Unit): string {
	return `
    <div style="font-family: sans-serif; font-size: 13px;">
      <strong>${unit.name}</strong><br/>
      Type: ${getTypeName(unit.type)}<br/>
      Heading: ${unit.heading.toFixed(0)}°<br/>
      Speed: ${unit.speed.toFixed(1)} m/s<br/>
      Last seen: ${new Date(unit.lastSeenUtc).toLocaleTimeString()}
    </div>
  `;
}

function getTypeName(type: number): string {
	switch (type) {
		case 0:
			return "Drone";
		case 1:
			return "Vehicle";
		case 2:
			return "Infantry";
		default:
			return "Unknown";
	}
}

const config = useRuntimeConfig();

onMounted(() => {
	map = new maplibregl.Map({
		container: mapContainer.value!,
		style: `https://api.maptiler.com/maps/streets-v2/style.json?key=${config.public.maptilerKey}`,
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
				const popup = createPopup(unit);
				marker = new maplibregl.Marker({
					color: getColorForType(unit.type),
				})
					.setLngLat([unit.longitude, unit.latitude])
					.setPopup(popup)
					.addTo(map);
				markers.set(unit.id, marker);
			} else {
				marker.setLngLat([unit.longitude, unit.latitude]);

				const popup = marker.getPopup();
				if (popup) {
					popup.setHTML(popupContent(unit));
				}
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
