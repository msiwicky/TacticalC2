import * as signalR from "@microsoft/signalr";
import type { Unit } from "../types/Unit";

export function useSignalR() {
	const units = ref<Map<string, Unit>>(new Map());
	const isConnected = ref(false);

	let connection: signalR.HubConnection | null = null;

	async function connect() {
		connection = new signalR.HubConnectionBuilder()
			.withUrl("http://localhost:5136/hubs/units")
			.withAutomaticReconnect()
			.build();

		connection.on("UnitPositionUpdated", (unit: Unit) => {
			units.value.set(unit.id, unit);
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
		isConnected,
		connect,
		disconnect,
	};
}
