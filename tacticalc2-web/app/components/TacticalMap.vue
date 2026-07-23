<script setup lang="ts">
import maplibregl from "maplibre-gl";
import "maplibre-gl/dist/maplibre-gl.css";
import type { PredictedPosition } from "~/composables/useSignalR";
import type { Unit } from "~/types/Unit";

const props = defineProps<{
	units: Map<string, Unit>;
	predictedPositions: Map<string, PredictedPosition>;
	playbackEntry?: UnitHistoryEntry | null;
}>();

const predictionMarkers = new Map<string, maplibregl.Marker>();

watch(
	() => props.predictedPositions,
	(predictions) => {
		if (!map) return;

		for (const [unitId, marker] of predictionMarkers) {
			if (!predictions.has(unitId)) {
				marker.remove();
				predictionMarkers.delete(unitId);
			}
		}

		for (const prediction of predictions.values()) {
			let marker = predictionMarkers.get(prediction.unitId);

			const el = document.createElement("div");
			el.style.width = "16px";
			el.style.height = "16px";
			el.style.borderRadius = "50%";
			el.style.border = "2px dashed #ef4444";
			el.style.backgroundColor = "rgba(239, 68, 68, 0.2)";

			if (!marker) {
				marker = new maplibregl.Marker({ element: el })
					.setLngLat([prediction.longitude, prediction.latitude])
					.addTo(map);
				predictionMarkers.set(prediction.unitId, marker);
			} else {
				marker.setLngLat([prediction.longitude, prediction.latitude]);
			}
		}
	},
	{ deep: true },
);

let playbackMarker: maplibregl.Marker | null = null;

watch(
	() => props.playbackEntry,
	(entry) => {
		if (!map) return;

		if (!entry) {
			playbackMarker?.remove();
			playbackMarker = null;
			return;
		}

		if (!playbackMarker) {
			playbackMarker = new maplibregl.Marker({ color: "#a855f7" }) // fioletowy, żeby odróżnić od żywych jednostek
				.setLngLat([entry.longitude, entry.latitude])
				.addTo(map);
		} else {
			playbackMarker.setLngLat([entry.longitude, entry.latitude]);
		}
	},
);

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
