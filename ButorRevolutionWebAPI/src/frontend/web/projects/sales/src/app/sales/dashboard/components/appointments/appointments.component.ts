import { Component, OnInit, Input } from '@angular/core';
import { IAppointmentsModel } from '../../models/appointments.model';

@Component({
  selector: 'butor-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.scss']
})
export class AppointmentsComponent implements OnInit {

  @Input() appointmentsDataSource: IAppointmentsModel[] = [];

  constructor() { }

  ngOnInit() {
  }

}
