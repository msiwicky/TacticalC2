export interface Unit {
	id: string;
	name: string;
	type: number;
	latitude: number;
	longitude: number;
	heading: number;
	speed: number;
	lastSeenUtc: string;
}
