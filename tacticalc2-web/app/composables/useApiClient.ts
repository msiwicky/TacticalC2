export interface UnitHistoryEntry {
	id: string;
	unitId: string;
	latitude: number;
	longitude: number;
	heading: number;
	speed: number;
	timestampUtc: string;
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

	return { getUnitHistory };
}
