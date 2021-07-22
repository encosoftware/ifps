export interface IOrderMessagesModel {
    messages: IMessagesModel[];
    assignedPeople: IAssignedPeopleModel[];
}

export interface IMessagesModel {
    text?: string | undefined;
    channelId?: number;
    messageCount: number;
    messageSeen: boolean;
    sent: Date;
    sender?: IMessageSenderModel | undefined;
}

export interface IAssignedPeopleModel {
    userId: number;
    name: string;
    fileName?: string;
    containerName?: string;
}

export interface IMessageSenderModel {
    name?: string;
    containerName?: string;
    fileName?: string;
}

export interface IMessageModel {
    userId: number;
    text: string;
    messageChannelId: number;
}
