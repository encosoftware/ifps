import { Injectable } from '@angular/core';
import {
  ApiVenuesCompaniesClient,
  MeetingRoomAppointmentsDto,
  ApiAppointmentsRangeMeetingroomsClient,
  AppointmentListDto
} from '../../../shared/clients';
import { Observable } from 'rxjs';
import { IMeetingRoomsViewModel } from '../models/meeting-rooms.model';
import { map } from 'rxjs/operators';
import { CalendarEventSubtitle } from '../pages/meetingrooms.component';

@Injectable({
  providedIn: 'root'
})
export class MeetingroomsService {

  constructor(
    private venuesClient: ApiVenuesCompaniesClient,
    private meetingRoomsClient: ApiAppointmentsRangeMeetingroomsClient,
  ) { }

  getMeetingRoomsList(companyId: number): Observable<IMeetingRoomsViewModel[]> {
    return this.venuesClient.getMeetingRoomsByCompany(companyId).pipe(
      map((dto: MeetingRoomAppointmentsDto[]): IMeetingRoomsViewModel[] => {
        return dto.map((x: MeetingRoomAppointmentsDto): IMeetingRoomsViewModel => ({
          id: x.id,
          name: x.name,
          status: x.currentlyBooked,
          venue: x.venue,
          address: x.address.city + ', ' + x.address.address
        }));
      })
    );
  }

  getAppointments(meetingRoomId: number, from: Date, to: Date): Observable<CalendarEventSubtitle[]> {
    return this.meetingRoomsClient.getAppointmentsByDateRangeAndMeetingRoom(meetingRoomId, from, to).pipe(
      map((dto: AppointmentListDto[]): CalendarEventSubtitle[] => {
        const retObj: CalendarEventSubtitle[] = [];
        dto.forEach((x: AppointmentListDto) => {
          const temp: CalendarEventSubtitle = {
            start: x.from,
            end: x.to,
            title: x.subject,
            subTitle: x.categoryName,
            draggable: false,
            resizable: {
              afterEnd: false,
              beforeStart: false
            },
            id: x.id,
            meta: {
              address: x.address,
              customerName: x.customerName,
              notes: x.notes
            }
          };
          retObj.push(temp);
        });
        return retObj;
      })
    );
  }
}
