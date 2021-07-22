export interface IMessageViewModel {
    text?: string | undefined;
    messageCount: number;
    messageSeen: boolean;
    sent: string;
    sender: IMessageChannelParticipant;
}

export interface IMessageChannelParticipant {
    name: string;
    picture?: IPictureModel;
}

export interface IPictureModel {
    containerName: string;
    fileName: string;
}
