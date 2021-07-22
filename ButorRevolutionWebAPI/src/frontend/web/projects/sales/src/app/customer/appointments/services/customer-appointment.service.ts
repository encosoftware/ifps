import { Injectable } from '@angular/core';
import {
  ApiAppointmentsOwnClient,
  AppointmentListDto,
  AppointmentDetailsDto,
  ApiAppointmentsRangeCustomerClient,
  ApiAppointmentsRangePartnerClient,
  ApiAppointmentsPartnerClient
} from '../../../shared/clients';
import { Observable } from 'rxjs';
import { IAppointmentListViewModel, IAppointmentDetailViewModel } from '../../../sales/appointments/models/appointments.model';
import { map } from 'rxjs/operators';
import { format } from 'date-fns';

@Injectable({
  providedIn: 'root'
})
export class CustomerAppointmentService {

  constructor(
    private appointmentsOwnClient: ApiAppointmentsOwnClient,
    private appointmentsOwnRangeClient: ApiAppointmentsRangeCustomerClient,
    private partnerAppointmentsOwnRangeClient: ApiAppointmentsRangePartnerClient,
    private partnerAppointmentClient: ApiAppointmentsPartnerClient
  ) { }

  getAppointments(startDate: Date, endDate: Date): Observable<IAppointmentListViewModel[]> {
    return this.appointmentsOwnRangeClient.getCustomerAppointmentsByDateRange(startDate, endDate)
      .pipe(map(res => res.map(this.appointmentsToViewModel)));
  }

  getPartnerAppointments(startDate: Date, endDate: Date): Observable<IAppointmentListViewModel[]> {
    return this.partnerAppointmentsOwnRangeClient.getPartnerAppointmentsByDateRange(startDate, endDate)
      .pipe(map(res => res.map(this.appointmentsToViewModel)));
  }

  private appointmentsToViewModel(dto: AppointmentListDto): IAppointmentListViewModel {
    return {
      id: dto.id,
      subject: dto.subject,
      from: dto.from,
      to: dto.to,
      notes: dto.notes,
      customerName: dto.customerName,
      categoryName: dto.categoryName,
      address: {
        postCode: dto.address.postCode,
        address: dto.address.address,
        city: dto.address.city
      }
    };
  }

  getAppointment(id: number): Observable<IAppointmentDetailViewModel> {
    return this.appointmentsOwnClient.getCustomerAppointmentById(id).pipe(map(this.appointmentToViewModel));
  }

  getPartnerAppointment(id: number): Observable<IAppointmentDetailViewModel> {
    return this.partnerAppointmentClient.getPartnerAppointmentById(id).pipe(map(this.appointmentToViewModel));
  }

  private appointmentToViewModel(dto: AppointmentDetailsDto): IAppointmentDetailViewModel {
    return {
      subject: dto.subject,
      date: dto.from,
      from: format(dto.from, 'HH:mm'),
      to: format(dto.to, 'HH:mm'),
      notes: dto.notes,
      address: dto.meetingRoomId === 0 ? {
        countryId: dto.address.countryId,
        postCode: dto.address.postCode,
        address: dto.address.address,
        city: dto.address.city
      } : {
          countryId: undefined,
          postCode: undefined,
          address: '',
          city: ''
        },
      categoryId: dto.categoryId,
      customerId: dto.customerId,
      customerName: dto.customerName,
      isNewAddress: dto.meetingRoomId === 0 ? true : false,
      meetingRoomId: dto.meetingRoomId,
      orderId: dto.orderId,
      venueId: dto.venueId,
      partnerId: dto.partnerId
    };
  }
}
