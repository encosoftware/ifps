import { Component, OnInit, Input, OnDestroy, ViewChild } from '@angular/core';
import { IOrderCurrentMessageViewModel } from '../../models/orders';
import { IOrderMessagesModel, IMessageModel, IMessagesModel, IAssignedPeopleModel } from '../../models/messages.model';
import { MessagesService } from '../../services/messages.service';
import { Store, select } from '@ngrx/store';
import { take, tap } from 'rxjs/operators';
import { coreLoginT } from '../../../../core/store/selectors/core.selector';
import { SignalRService } from '../../../../shared/services/signal-r.service';
import { PerfectScrollbarComponent } from 'ngx-perfect-scrollbar';

@Component({
  selector: 'butor-order-messages',
  templateUrl: './order-messages.component.html',
  styleUrls: ['./order-messages.component.scss']
})
export class OrderMessagesComponent implements OnInit, OnDestroy {
  @Input() orderMessages: IOrderMessagesModel;
  @Input() orderId: string;
  @ViewChild('scrollableElement', {static: true}) scrollContainer: PerfectScrollbarComponent;

  selectedMessageId: number;
  selectedMessage: IMessagesModel;
  isLoading = false;
  userId: number;
  actualMessage = '';
  currentMessage: IOrderCurrentMessageViewModel[];
  selectedChannel: string;
  userToken: any;

  constructor(
    private messageService: MessagesService,
    private signalRService: SignalRService,
    private store: Store<any>
  ) { }

  ngOnInit(): void {
    this.store.pipe(
      select(coreLoginT),
      take(1),
      tap(res => {
        this.userId = +res.UserId;
        this.userToken = res;
      })
    ).subscribe();
    if (this.orderMessages.messages.length > 0) {
      this.selectMessage(this.orderMessages.messages[0]);
    }
    this.signalRService.actualMessage.subscribe(newMessageArrived => {
      if (newMessageArrived.text !== '') {
        this.currentMessage.push(newMessageArrived);
        this.scrollToBottom();
      }
    });
  }

  selectMessage(mess: IMessagesModel) {
    // this.signalRService.close();
    this.signalRService.startConnection(this.userId);
    this.selectedMessageId = mess.channelId;
    this.selectedMessage = mess;
    this.selectedChannel = mess.sender.name;
    this.isLoading = true;
    this.messageService.getMessageChannels(mess.channelId).subscribe(res => {
      this.currentMessage = res;
    },
      () => { },
      () => this.scrollToBottom()
    );
  }

  createChannel(assigned: IAssignedPeopleModel, name: string): void {
    this.selectedChannel = name;
    this.messageService.createChannel(this.orderId, +this.userId, assigned.userId).subscribe(res => {
      let newMessageChannel: IMessagesModel = {
        channelId: res,
        messageCount: 0,
        messageSeen: true,
        sender: {
          name: assigned.name
        },
        sent: new Date()
      };
      this.orderMessages.messages.push(newMessageChannel);
      this.orderMessages.assignedPeople = this.orderMessages.assignedPeople.filter(x => x !== assigned);
      this.selectMessage(newMessageChannel);
    });
  }

  onKeydown(event): void {
    if (event.key === 'Enter' && this.actualMessage.length > 0) {
      const newMessage: IOrderCurrentMessageViewModel = {
        senderId: +this.userId,
        sent: new Date(),
        text: this.actualMessage
      };
      const tempMessage: IMessageModel = {
        messageChannelId: this.selectedMessageId,
        text: this.actualMessage,
        userId: this.userId
      };
      this.messageService.sendMessage(tempMessage).subscribe(() => this.scrollToBottom());
      this.currentMessage.push(newMessage);
      this.actualMessage = '';
    }
  }

  scrollToBottom(): void {
    this.scrollContainer.directiveRef.scrollToBottom();
  }

  ngOnDestroy(): void {
    // this.signalRService.close();
  }
}
