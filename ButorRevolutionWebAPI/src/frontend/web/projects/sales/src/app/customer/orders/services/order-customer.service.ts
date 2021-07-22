import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  ICustomerOrderStateViewModel,
  ICustomerSelectSalesModel
} from '../models/customerOrders';
import { map } from 'rxjs/operators';
import {
  ApiOrderstatesClient,
  ApiUsersSearchClient,
  ApiOrdersClient,
  ApiOrdersDocumentsClient,
  TicketByOrderListDto,
  DocumentGroupDto,
  DivisionTypeEnum,
  ApiOrdersDocumentsVersionsClient,
  DocumentStateEnum,
  ApiDocumentsClient,
  ApiTicketsOwnOrderClient,
} from '../../../shared/clients';
import { format } from 'date-fns';
import { IorderSalesHeader } from '../models/ICustomerOrderViewModel';
import { IOrderTicketModel } from '../../../sales/orders/models/ticket.model';
import { IOrderDocumentViewModel } from '../../../sales/orders/models/orders';

@Injectable({
  providedIn: 'root'
})
export class OrderCustomerService {

  constructor(
    private orderStateClient: ApiOrderstatesClient,
    private customerPerson: ApiUsersSearchClient,
    private ordersClient: ApiOrdersClient,
    private ordersDocumentClient: ApiOrdersDocumentsClient,
    private approveClient: ApiOrdersDocumentsVersionsClient,
    private documentClient: ApiDocumentsClient,
    private customerTicketClient: ApiTicketsOwnOrderClient,
  ) { }

  getDayTypes(): Observable<ICustomerOrderStateViewModel[] | null> {
    return this.orderStateClient.getOrderStatuses().pipe(
      map(dto => dto.map(x => ({
        id: x.id,
        state: x.state,
        translation: x.translation,
      })
      )
      )
    );
  }
  searchCustomerPerson(name: string = '', take = 10): Observable<ICustomerSelectSalesModel[]> {
    return this.customerPerson.searchUser(name, DivisionTypeEnum.Customer, take).pipe(
      map(x => x.map(sales => ({
        id: sales.id,
        name: sales.name
      })))
    );
  }

  getById(id: string): Observable<IorderSalesHeader | null> {
    return this.ordersClient.getById(id).pipe(
      map(ord => ({
        orderId: ord.orderId,
        currentStatus: ord.currentStatus.translation,
        statusDeadline: ord.statusDeadline,
        responsible: ord.responsible.name,
        customer: ord.customer.name,
        sales: ord.sales.name,
        createdOn: ord.createdOn,
        deadline: ord.deadline
      }))
    );

  }

  getDocuments(id: string): Observable<IOrderDocumentViewModel> {
    return this.ordersDocumentClient.get(id).pipe(
      map(this.orderDocumentsToViewModel)
    );
  }

  private orderDocumentsToViewModel(dto: DocumentGroupDto[]): IOrderDocumentViewModel {
    let model: IOrderDocumentViewModel = {
      documents: [],
      versionable: []
    };
    let versionableIndex = 0;
    let nonversionableIndex = 0;
    for (let documentDto of dto) {
      if (documentDto.isVersionable) {
        model.versionable.push({
          documentGroupId: documentDto.documentGroupId,
          documentGroupDto: documentDto,
          folderId: documentDto.folderId,
          name: documentDto.folderName,
          fileCount: 0,
          status: undefined,
          translation: '',
          versions: []
        });
        let versionIndex = 0;
        for (let version of documentDto.versions) {
          model.versionable[versionableIndex].versions.push({
            versionId: version.id,
            date: version.lastModifiedOn,
            status: version.documentState.state,
            translation: version.documentState.currentTranslation,
            documents: []
          });
          model.versionable[versionableIndex].fileCount += version.documents.length;
          model.versionable[versionableIndex].status = version.documentState.state;

          for (let document of version.documents) {
            model.versionable[versionableIndex].versions[versionIndex].documents.push({
              id: document.id,
              name: document.displayName,
              type: document.fileExtensionType
            });
          }
          versionIndex = versionIndex + 1;
        }
        versionableIndex = versionableIndex + 1;
      } else {
        model.documents.push({
          documentGroupId: documentDto.documentGroupId,
          documentGroupDto: documentDto,
          folderId: documentDto.folderId,
          name: documentDto.folderName,
          status: undefined,
          translation: '',
          fileCount: 0,
          versions: []
        });
        let versionIndex = 0;
        for (let version of documentDto.versions) {
          model.documents[nonversionableIndex].versions.push({
            versionId: version.id,
            date: version.lastModifiedOn,
            status: version.documentState.state,
            translation: version.documentState.currentTranslation,
            documents: []
          });
          model.documents[nonversionableIndex].fileCount += version.documents.length;
          model.documents[nonversionableIndex].status = version.documentState.state;

          for (let document of version.documents) {
            model.documents[nonversionableIndex].versions[versionIndex].documents.push({
              id: document.id,
              name: document.displayName,
              type: document.fileExtensionType
            });
          }
          versionIndex = versionIndex + 1;
        }
        nonversionableIndex = nonversionableIndex + 1;
      }
    }
    return model;
  }

  getOrderTicket(orderId: string): Observable<IOrderTicketModel[]> {
    return this.customerTicketClient.getCustomerTicketsByOrder(orderId).pipe(
      map((dto: TicketByOrderListDto[]): IOrderTicketModel[] => {
        return dto.map((x: TicketByOrderListDto): IOrderTicketModel => ({
          assignedTo: x.assignedTo,
          closeOn: x.closedOn ? format(x.closedOn, 'yyyy. MM. dd.') : '',
          deadline: x.deadline ? format(x.deadline, 'yyyy. MM. dd.') : '',
          state: x.state
        }));
      })
    );
  }

  approveOrDeclineDocument(orderId: string, documentVersionId: number, result: DocumentStateEnum) {
    return this.approveClient.approveDocuments(orderId, documentVersionId, result);
  }

  getDocument(documentId: string): Observable<any> {
    return this.documentClient.getDocument(documentId);
  }

}
