import { Component, OnInit, Input, EventEmitter, Output, OnDestroy, Inject } from '@angular/core';
import { ICfcInformationListViewModel } from '../../models/order-scheduling.model';
import { OrderSchedulingService } from '../../services/order-scheduling.service';
import * as html2pdf from 'html2pdf.js';

import pdfMake from 'pdfmake/build/pdfmake';
import pdfFonts from 'pdfmake/build/vfs_fonts';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'factory-print-codes',
  templateUrl: './print-codes.component.html',
  styleUrls: ['./print-codes.component.scss']
})
export class PrintCodesComponent implements OnInit, OnDestroy {
  @Output() deleteItem = new EventEmitter();
  cfcInformation: ICfcInformationListViewModel[] = [];
  pdfUp: File;
  pdf: any;

  constructor(
    private orderSchedulingService: OrderSchedulingService,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) { }

  ngOnInit() {
    this.orderSchedulingService.getCFCsForPrintByOrderId(this.data.orderId).subscribe(res => this.cfcInformation = res);
  }

  ngOnDestroy(): void {
    this.deleteItem.emit(this.data.orderId);
  }

  captureScreen() {
    const element = document.getElementById('ccc');

    const opt = {
      margin: [0, 0],
      filename: `${this.data.orderId}.pdf`,
      image: { type: 'jpeg', quality: 0.98 },
      pagebreak: { after: '.item-container:nth-child(4n)' },
      html2canvas: { scale: 1.2, logging: false },
      jsPDF: { unit: 'mm', format: 'a4', orientation: 'portrait' }
    };

    this.pdf = html2pdf().set(opt).from(element).toPdf().get('pdf');
    this.pdf.save();
  }

  printPdf() {
    this.captureScreen();
    this.pdf.output('blob').then((pdf) => {
      this.pdfUp = new File([pdf], `${this.data.orderId}.pdf`);
    });
  }

}
