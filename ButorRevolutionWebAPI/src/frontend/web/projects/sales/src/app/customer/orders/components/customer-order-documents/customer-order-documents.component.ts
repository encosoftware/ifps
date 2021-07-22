import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FileExtensionTypeEnum, DocumentStateEnum } from '../../../../shared/clients';
import { IOrderDocumentViewModel } from '../../../../sales/orders/models/orders';
import { OrderCustomerService } from '../../services/order-customer.service';
import { saveAs } from 'file-saver';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'butor-customer-order-documents',
  templateUrl: './customer-order-documents.component.html',
  styleUrls: ['./customer-order-documents.component.scss']
})
export class CustomerOrderDocumentsComponent implements OnInit {
  fileExtensionTypeEnum = FileExtensionTypeEnum;
  documentStateEnum = DocumentStateEnum;
  @Input() documents: IOrderDocumentViewModel;
  @Output() refresh = new EventEmitter();
  id: string;

  constructor(
    public dialog: MatDialog,
    private orderService: OrderCustomerService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');
  }

  approve(versionId: number) {
    this.orderService.approveOrDeclineDocument(this.id, versionId, DocumentStateEnum.Approved).subscribe(res => this.refresh.emit());
  }

  decline(versionId: number) {
    this.orderService.approveOrDeclineDocument(this.id, versionId, DocumentStateEnum.Declined).subscribe(res => this.refresh.emit());
  }

  downloadDoc(id: string, name: string, event) {
    event.stopPropagation();
    this.orderService.getDocument(id).subscribe(res => {
      saveAs(res.data, name);
    });
  }

}
