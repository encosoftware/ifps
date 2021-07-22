import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import pdfMake from 'pdfmake/build/pdfmake';
import pdfFonts from 'pdfmake/build/vfs_fonts';
import * as html2pdf from 'html2pdf.js';
import {
    IOrderDocumentCreateViewModel,
    IOrderDocumentViewModel,
    DocumentTypeModel,
    IOrderDocumentFolderListViewModel,
    IContractViewModel
} from '../../models/orders';
import { finalize, map, switchMap, tap, filter } from 'rxjs/operators';
import { FileUploaderService } from '../../services/file-uploader.service';
import { OrdersService } from '../../services/orders.service';
import { SnackbarService } from 'butor-shared-lib';
import { DocumentTypeEnum, DocumentFolderTypeEnum } from '../../../../shared/clients';
import { ContractService } from '../../services/contract.service';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

pdfMake.vfs = pdfFonts.pdfMake.vfs;

@Component({
    templateUrl: './contract-preview.component.html',
    styleUrls: ['./contract-preview.component.scss']
})
export class ContractPreviewComponent implements OnInit {

    isLoading = false;
    orderId: string;
    offerName: string;
    contract: IContractViewModel;
    orderDocument: IOrderDocumentViewModel;
    documentType: DocumentTypeModel;
    documentFolderType: IOrderDocumentFolderListViewModel;
    pdfUp: File;
    pdf: any;

    constructor(
        private dialogRef: MatDialogRef<any>,
        private contractService: ContractService,
        public snackBar: SnackbarService,
        private uploadService: FileUploaderService,
        public orderService: OrdersService,
        private router: Router,
        private translate: TranslateService,
        @Inject(MAT_DIALOG_DATA) public data: any,
    ) { }

    ngOnInit(): void {
        this.isLoading = true;
        this.orderId = this.data.id;
        this.orderService.getOrderBasics(this.orderId).pipe().subscribe(res => {
            this.offerName = res.orderName;
        });
        this.contractService.getContract(this.orderId).pipe(
            finalize(() => {
                this.isLoading = false;
            })
        ).subscribe(res => {
            this.contract = res;
        });

        this.uploadService.getDocumentTypes().pipe(
            map(res => res.filter(ins => DocumentTypeEnum[DocumentTypeEnum[ins.documentType]] === DocumentTypeEnum[DocumentTypeEnum.Contract])),
        ).subscribe(res => res.map(ins => this.documentType = ins));

        this.uploadService.getDocumentFolders().pipe(
            map(res => res.filter(ins => DocumentTypeEnum[DocumentTypeEnum[ins.folderName]] === DocumentTypeEnum[DocumentTypeEnum.Offer])),
        ).subscribe();

        this.orderService.getOrderDocuments(this.orderId).pipe(
            map(res => res.versionable),
            map(res => res.filter(ins => ins.type.toString() === DocumentFolderTypeEnum[DocumentFolderTypeEnum.ContractDocument])),
        ).subscribe(res => res.map(ins => this.documentFolderType = ins));
    }

    cancel(): void {
        this.dialogRef.close();
    }

    captureScreen() {
        const element = document.getElementById('ccc');
        const opt = {
            margin: [0, 0],
            filename: `${this.offerName}.pdf`,
            image: { type: 'jpeg', quality: 0.98 },
            pagebreak: { before: '.contract-container' },
            html2canvas: { scale: 1.2, logging: false },
            jsPDF: { unit: 'mm', format: 'a4', orientation: 'landscape' }
        };

        this.pdf = html2pdf().set(opt).from(element).toPdf().get('pdf').then((pdfObject) => {
            pdfObject.setPage(1);
            pdfObject.setFontSize(22);
            pdfObject.text(this.offerName, 5, 10);
        });
        this.pdf.save();
    }

    sendToCustomer() {
        this.captureScreen();
        this.pdf.output('blob').then((pdf) => {
            this.pdfUp = new File([pdf], `${this.offerName}.pdf`);
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
