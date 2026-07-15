<script setup lang="ts">
import type { Unit } from "~/types/Unit";
import { calculateUnitStatus } from "~/composables/useUnitStatus";

const props = defineProps<{
	units: Map<string, Unit>;
}>();

const unitsList = computed(() => Array.from(props.units.values()));

function statusColorClass(status: string): string {
	switch (status) {
		case "Active":
			return "bg-emerald-500";
		case "Stale":
			return "bg-yellow-500";
		case "Offline":
			return "bg-gray-500";
		default:
			return "bg-gray-500";
	}
}

function typeName(type: number): string {
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
</script>

<template>
	<div class="w-72 bg-neutral-900 text-white p-4">
		<h2 class="text-lg font-semibold mb-3">
			Units ({{ unitsList.length }})
		</h2>
		<ul class="space-y-1">
			<li
				v-for="unit in unitsList"
				:key="unit.id"
				class="flex items-center gap-2 py-2 border-b border-neutral-700"
			>
				<span
					class="w-2.5 h-2.5 rounded-full flex-shrink-0"
					:class="
						statusColorClass(calculateUnitStatus(unit.lastSeenUtc))
					"
				></span>
				<strong class="font-medium">{{ unit.name }}</strong>
				<span class="text-neutral-400 text-sm">{{
					typeName(unit.type)
				}}</span>
				<span class="ml-auto text-sm">{{
					calculateUnitStatus(unit.lastSeenUtc)
				}}</span>
			</li>
		</ul>
	</div>
</template>
