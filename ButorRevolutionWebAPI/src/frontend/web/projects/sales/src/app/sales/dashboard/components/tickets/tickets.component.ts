import { Component, OnInit, Input } from '@angular/core';
import { ITicketsModel } from '../../models/tickets.model';

@Component({
  selector: 'butor-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.scss']
})
export class TicketsComponent implements OnInit {

  @Input() ticketsDataSource: ITicketsModel[] = [];

  constructor() { }

  ngOnInit() {
  }

}
