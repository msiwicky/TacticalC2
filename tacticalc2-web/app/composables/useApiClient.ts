export interface UnitHistoryEntry {
	id: string;
	unitId: string;
	latitude: number;
	longitude: number;
	heading: number;
	speed: number;
	timestampUtc: string;
}

export interface Zone {
	id: string;
	name: string;
	boundaryPoints: { latitude: number; longitude: number }[];
}

export function useApiClient() {
	const baseUrl = "http://localhost:5136";

	async function getUnitHistory(
		unitId: string,
		from: Date,
		to: Date,
	): Promise<UnitHistoryEntry[]> {
		const params = new URLSearchParams({
			from: from.toISOString(),
			to: to.toISOString(),
		});

		const response = await fetch(
			`${baseUrl}/api/units/${unitId}/history?${params}`,
		);

		if (!response.ok) {
			throw new Error(`Failed to fetch history: ${response.status}`);
		}

		return response.json();
	}

	async function getZones(): Promise<Zone[]> {
		const response = await fetch(`${baseUrl}/api/zones`);
		if (!response.ok)
			throw new Error(`Failed to fetch zones: ${response.status}`);
		return response.json();
	}

	async function getAlerts(): Promise<Alert[]> {
		const response = await fetch(`${baseUrl}/api/alerts`);
		if (!response.ok)
			throw new Error(`Failed to fetch alerts: ${response.status}`);
		return response.json();
	}

	return { getUnitHistory, getZones, getAlerts };
}
