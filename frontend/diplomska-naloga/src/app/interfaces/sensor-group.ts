export interface SensorGroup {
    id:        string;
    userId:    string;
    createdAt: Date;
    updatedAt: Date;
    name:      string;
    sensorHash: string;
    showHash:boolean;
    description: string;
    lat: number;
    long: number;
}
