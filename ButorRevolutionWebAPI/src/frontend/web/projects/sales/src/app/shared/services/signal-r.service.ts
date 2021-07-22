import { Injectable, Inject } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { IOrderCurrentMessageViewModel } from '../../sales/orders/models/orders';
import { API_BASE_URL } from '../clients';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private messageSource = new BehaviorSubject<IOrderCurrentMessageViewModel>({ senderId: 1, text: '' });
  actualMessage = this.messageSource.asObservable();

  message: IOrderCurrentMessageViewModel;
  hubConnection: signalR.HubConnection;
  private retryInterval: number;

  constructor(@Inject(API_BASE_URL) private baseUrl?: string) { }

  public startConnection(userId: any) {
    this.retryInterval = Math.random() * 2000;
    const url = this.baseUrl + '/api/notify';
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(url, { accessTokenFactory: () => userId})
      .build();
    this.start(this.retryInterval);

    this.hubConnection.on('ReceiveMessage', (senderId, messages) => {
      const newMessage: IOrderCurrentMessageViewModel = {
        senderId,
        text: messages
      };
      this.messageSource.next(newMessage);
    });

    // this.hubConnection.onclose(() => {
    //   this.start(this.retryInterval);
    // });
  }


  start(retryInterval: number) {
    this.hubConnection.start().catch(err => {
      setTimeout(() => this.start(retryInterval + this.retryInterval), retryInterval);
    });
  }

  close() {
    this.hubConnection.onclose(() => {
      this.start(this.retryInterval);
    });
  }

}
