import { Injectable } from '@angular/core';
import {
  ApiMessagesClient,
  UnansweredMessageListDto,
  OrderMessagesDto,
  ApiMessagesChannelsClient,
  MessageChannelDetailsDto,
  MessageListDto,
  MessageCreateDto,
  MessageChannelCreateDto,
  MessageChannelParticipantListDto
} from '../../../shared/clients';
import { Observable } from 'rxjs';
import { IMessagesModel, IOrderMessagesModel, IAssignedPeopleModel, IMessageModel } from '../models/messages.model';
import { map, take } from 'rxjs/operators';
import { IOrderCurrentMessageViewModel } from '../models/orders';
import { Store, select } from '@ngrx/store';
import { coreLoginT } from '../../../core/store/selectors/core.selector';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {

  userId: string;

  constructor(
    private messagesClient: ApiMessagesClient,
    private channelsClient: ApiMessagesChannelsClient,
    private store: Store<any>
  ) {
    this.store.pipe(
      select(coreLoginT),
      take(1)
    ).subscribe(res => this.userId = res.UserId);
  }

  getOrderAndUserMessages(orderId: string): Observable<IOrderMessagesModel> {
    return this.messagesClient.getMessagesByOrderAndUserList(orderId).pipe(
      map((dto: OrderMessagesDto): IOrderMessagesModel => ({
        messages: dto.messageChannels.map((m: UnansweredMessageListDto): IMessagesModel => ({
          channelId: m.messageChannelId,
          messageCount: m.messageCount,
          messageSeen: m.messageSeen,
          sent: m.sent,
          text: m.text,
          sender: {
            containerName: '', // m.sender.image.containerName,
            fileName: '', // m.sender.image.fileName,
            name: m.sender.name
          }
        })),
        assignedPeople: dto.participants.map((p: MessageChannelParticipantListDto): IAssignedPeopleModel => ({
          containerName: p.image.containerName,
          fileName: p.image.fileName,
          name: p.name,
          userId: p.userId
        }))
      }))
    );
  }

  getMessageChannels(channelId: number): Observable<IOrderCurrentMessageViewModel[]> {
    return this.channelsClient.getMessageChannelList(channelId).pipe(
      map((dto: MessageChannelDetailsDto): IOrderCurrentMessageViewModel[] => {
        return dto.messages.map((x: MessageListDto): IOrderCurrentMessageViewModel => ({
          senderId: x.sender.userId,
          sent: x.sent,
          text: x.text
        }));
      })
    );
  }

  sendMessage(message: IMessageModel): Observable<void> {
    const dto = new MessageCreateDto({
      messageChannelId: message.messageChannelId,
      senderId: message.userId,
      text: message.text
    });
    return this.messagesClient.sendMessage(dto);
  }

  createChannel(orderId: string, senderId: number, targetId: number): Observable<number> {
    const dto = new MessageChannelCreateDto({
      orderId,
      participantIds: [
        senderId,
        targetId
      ]
    });
    return this.channelsClient.createMessageChannel(dto);
  }
}
