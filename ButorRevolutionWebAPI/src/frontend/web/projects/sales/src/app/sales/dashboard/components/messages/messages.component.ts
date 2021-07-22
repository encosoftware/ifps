import { Component, OnInit, Input } from '@angular/core';
import { IMessageViewModel } from '../../models/messages.model';

@Component({
  selector: 'butor-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit {

  @Input() messages: IMessageViewModel[] = [];

  constructor(
  ) { }

  ngOnInit() {
  }

}
