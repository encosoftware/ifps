import { Injectable } from '@angular/core';
import {
  ApiTicketsClient,
  TicketListDto,
  ApiTicketsOwnClient,
  AppointmentListDto,
  ApiAppointmentsRangeClient,
  ApiAppointmentsRangePartnerClient
} from '../../../shared/clients';
import { Observable } from 'rxjs';
import { ITicketsModel } from '../models/tickets.model';
import { map } from 'rxjs/operators';
import { IAppointmentsModel } from '../models/appointments.model';
import { getHours, getMinutes, getDate, format } from 'date-fns';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  constructor(
    private ticketClient: ApiTicketsClient,
    private ownTicketClient: ApiTicketsOwnClient,
    private appointmentRangeClient: ApiAppointmentsRangeClient,
    private partnerAppointmentRangeClient: ApiAppointmentsRangePartnerClient,
  ) { }

  getTickets(): Observable<ITicketsModel[]> {
    return this.ticketClient.getTicketList().pipe(
      map((dto: TicketListDto[]): ITicketsModel[] => {
        return dto.map((item: TicketListDto): ITicketsModel => ({
          id: item.orderId,
          customer: item.customerName,
          isExpired: item.isExpired,
          orderName: item.orderName,
          status: item.orderStateEnum
        }));
      })
    );
  }

  getTicketsByUserId(): Observable<ITicketsModel[]> {
    return this.ownTicketClient.getOwnTicketList().pipe(
      map((dto: TicketListDto[]): ITicketsModel[] => {
        return dto.map((item: TicketListDto): ITicketsModel => ({
          id: item.orderId,
          customer: item.customerName,
          isExpired: item.isExpired,
          orderName: item.orderName,
          status: item.orderStateEnum
        }));
      })
    );
  }

  getAppointmentsByDate(from: Date, to: Date, userId: number): Observable<IAppointmentsModel[]> {
    return this.appointmentRangeClient.getAppointmentsByDateRange(userId, from, to).pipe(
      map((dto: AppointmentListDto[]): IAppointmentsModel[] => {
        return dto.map((item: AppointmentListDto): IAppointmentsModel => ({
          id: item.id,
          customerName: item.customerName,
          notes: item.notes,
          subject: item.subject,
          from: item.from,
          fromHour: getHours(item.from),
          fromMinute: getMinutes(item.from),
          to: item.to,
          toHour: getHours(item.to),
          toMinute: getMinutes(item.to),
          currentDay: getDate(item.from),
          currentMonth: format(item.from, 'MMMM'),
          address: {
            ...item.address
          }
        }));
      })
    );
  }

  getPartnerAppointmentsByDate(from: Date, to: Date): Observable<IAppointmentsModel[]> {
    return this.partnerAppointmentRangeClient.getPartnerAppointmentsByDateRange(from, to).pipe(
      map((dto: AppointmentListDto[]): IAppointmentsModel[] => {
        return dto.map((item: AppointmentListDto): IAppointmentsModel => ({
          id: item.id,
          customerName: item.customerName,
          notes: item.notes,
          subject: item.subject,
          from: item.from,
          fromHour: getHours(item.from),
          fromMinute: getMinutes(item.from),
          to: item.to,
          toHour: getHours(item.to),
          toMinute: getMinutes(item.to),
          currentDay: getDate(item.from),
          currentMonth: format(item.from, 'MMMM'),
          address: {
            ...item.address
          }
        }));
      })
    );
  }

}
