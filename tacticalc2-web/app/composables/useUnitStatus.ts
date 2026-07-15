export type UnitStatus = "Active" | "Stale" | "Offline";

export function calculateUnitStatus(lastSeenUtc: string): UnitStatus {
	const elapsed = Date.now() - new Date(lastSeenUtc).getTime();

	if (elapsed < 5000) return "Active";
	if (elapsed < 15000) return "Stale";
	return "Offline";
}
