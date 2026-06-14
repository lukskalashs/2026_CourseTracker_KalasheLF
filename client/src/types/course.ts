export type Course = {
    id: number;
    date: string; 
    title: string;
    platformId: number;
    platformName: string;
    topicId: number;
    topicName: string;
    isCompleted: boolean;
    rating?: number;
    notes?: string;
}