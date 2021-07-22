import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IOfferCabinetsModel, IOfferShippingInformationModel, IOfferFormPreviewModel } from '../../models/offer.models';
import { OfferService } from '../../services/offer.service';

import * as html2pdf from 'html2pdf.js';

import pdfMake from 'pdfmake/build/pdfmake';
import pdfFonts from 'pdfmake/build/vfs_fonts';
import {
  IOrderDocumentCreateViewModel,
  IOrderDocumentViewModel,
  DocumentTypeModel,
  IOrderDocumentFolderListViewModel
} from '../../models/orders';
import { finalize, map, switchMap, tap, filter } from 'rxjs/operators';
import { FileUploaderService } from '../../services/file-uploader.service';
import { OrdersService } from '../../services/orders.service';
import { SnackbarService } from 'butor-shared-lib';
import { DocumentTypeEnum, DocumentFolderTypeEnum } from '../../../../shared/clients';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

pdfMake.vfs = pdfFonts.pdfMake.vfs;

@Component({
  templateUrl: './offer-preview.component.html',
  styleUrls: ['./offer-preview.component.scss']
})
export class OfferPreviwComponent implements OnInit {

  isLoading = false;
  orderId: string;
  offerName: string;
  shippingInfo: IOfferShippingInformationModel;
  topCabinetDataSource: IOfferCabinetsModel[];
  baseCabinetDataSource: IOfferCabinetsModel[];
  tallCabinetDataSource: IOfferCabinetsModel[];
  appliancesDataSource: IOfferCabinetsModel[];
  accessoriesDataSource: IOfferCabinetsModel[];
  preview: IOfferFormPreviewModel;
  orderDocument: IOrderDocumentViewModel;
  documentType: DocumentTypeModel;
  documentFolderType: IOrderDocumentFolderListViewModel;
  pdfUp: File;
  pdf: any;

  constructor(
    private dialogRef: MatDialogRef<any>,
    private service: OfferService,
    private uploadService: FileUploaderService,
    public orderService: OrdersService,
    public snackBar: SnackbarService,
    private translate: TranslateService,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) { }

  ngOnInit(): void {
    this.isLoading = true;
    this.orderId = this.data.id;
    this.service.getOfferFormPreview(this.orderId).pipe(
      finalize(() => {
        this.isLoading = false;
      })
    ).subscribe(res => {
      this.preview = res;
    });

    this.uploadService.getDocumentTypes().pipe(
      map(res => res.filter(ins => DocumentTypeEnum[DocumentTypeEnum[ins.documentType]] === DocumentTypeEnum[DocumentTypeEnum.Offer])),
    ).subscribe(res => res.map(ins => this.documentType = ins));

    this.uploadService.getDocumentFolders().pipe(
      map(res => res.filter(ins => DocumentTypeEnum[DocumentTypeEnum[ins.folderName]] === DocumentTypeEnum[DocumentTypeEnum.Offer])),
    ).subscribe();

    this.orderService.getOrderDocuments(this.orderId).pipe(
      map(res => res.versionable),
      map(res => res.filter(ins => ins.type.toString() === DocumentFolderTypeEnum[DocumentFolderTypeEnum.QuotationDocument])),
    ).subscribe(res => res.map(ins => this.documentFolderType = ins));

  }

  cancel(): void {
    this.dialogRef.close();
  }

  captureScreen() {
    const element = document.getElementById('ccc');

    const opt = {
      margin: [0, 0],
      filename: `${this.preview.offerName}.pdf`,
      image: { type: 'jpeg', quality: 0.98 },
      pagebreak: { mode: ['css', 'legacy'] },
      html2canvas: { scale: 1.2, logging: false },
      jsPDF: { unit: 'mm', format: 'a4', orientation: 'landscape' }
    };

    this.pdf = html2pdf().set(opt).from(element).toPdf().get('pdf').then((pdfObject) => {
      pdfObject.setPage(1);
      pdfObject.setFontSize(22);
      pdfObject.text(this.preview.offerName, 5, 10);

    });
    this.pdf.save();
  }

  sendToCustomer() {
    this.captureScreen();
    this.pdf.output('blob').then((pdf) => {
      this.pdfUp = new File([pdf], `${this.preview.offerName}.pdf`);
      this.uploadService.upload(this.pdfUp).pipe(
        map(res => {
          if (res) {
            let model: IOrderDocumentCreateViewModel = {
              folderId: this.documentFolderType.documentGroupId,
              versionId: null,
              typeId: this.documentType.id,
              documents: []
            };
            switch (res.status) {
              case 'started':
                break;
              case 'progress':
                break;
              case 'uploaded':
                model.documents.push({
                  containerName: res.data.item1,
                  fileName: res.data.item2
                });
                return model;
            }
          }
        }),
        filter(res => !!res),
        switchMap((ins) => {
          return this.orderService.postDocuments(ins, this.orderId);
        }
        ),
        finalize(() => {
          this.snackBar.customMessage(this.translate.instant('snackbar.success'));
          this.dialogRef.close();
          this.router.navigate([`sales/orders/${this.orderId}`]);
        })
      ).subscribe();
    });
  }
}
