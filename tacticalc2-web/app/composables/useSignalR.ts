import * as signalR from "@microsoft/signalr";
import type { Unit } from "../types/Unit";

export interface PredictedPosition {
	unitId: string;
	latitude: number;
	longitude: number;
	status: string;
}

export interface Alert {
	id: string;
	unitId: string;
	zoneId: string;
	severity: number;
	message: string;
	timestampUtc: string;
}

export function useSignalR() {
	const units = ref<Map<string, Unit>>(new Map());
	const predictedPositions = ref<Map<string, PredictedPosition>>(new Map());
	const isConnected = ref(false);
	const alerts = ref<Alert[]>([]);

	let connection: signalR.HubConnection | null = null;

	async function connect() {
		connection = new signalR.HubConnectionBuilder()
			.withUrl("http://localhost:5136/hubs/units")
			.withAutomaticReconnect()
			.build();

		connection.on("UnitPositionUpdated", (unit: Unit) => {
			units.value.set(unit.id, unit);
			predictedPositions.value.delete(unit.id);
		});

		connection.on(
			"UnitPositionPredicted",
			(prediction: PredictedPosition) => {
				predictedPositions.value.set(prediction.unitId, prediction);
			},
		);

		connection.on("AlertRaised", (alert: Alert) => {
			alerts.value.unshift(alert);
		});

		connection.onreconnected(() => {
			isConnected.value = true;
		});

		connection.onclose(() => {
			isConnected.value = false;
		});

		await connection.start();
		await connection.invoke("Subscribe");

		isConnected.value = true;
	}

	async function disconnect() {
		await connection?.stop();
	}

	return {
		units,
		predictedPositions,
		alerts,
		isConnected,
		connect,
		disconnect,
	};
}
