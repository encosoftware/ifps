import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { OrderDocumentUploadComponent } from '../upload-order-document/upload-order-document.component';
import { IOrderDocumentViewModel } from '../../models/orders';
import { DocumentStateEnum, ClaimPolicyEnum, DocumentFolderTypeEnum, DivisionTypeEnum } from '../../../../shared/clients';
import { OrdersService } from '../../services/orders.service';
import { ActivatedRoute } from '@angular/router';
import { SnackbarService } from 'butor-shared-lib';
import { saveAs } from 'file-saver';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'butor-order-documents',
  templateUrl: './order-documents.component.html',
  styleUrls: ['./order-documents.component.scss']
})
export class OrderDocumentsComponent implements OnInit {

  @Input() documents: IOrderDocumentViewModel;
  @Input() divisions: any[];
  @Output() saved = new EventEmitter();


  versionOpenedIndex: number;
  openedIndex: number;
  selectedDocumentGroupId: number;
  selectedVersionId: number;
  selectedFolderId: number;
  claimPolicyEnum = ClaimPolicyEnum;
  constructor(
    public dialog: MatDialog,
    public orderService: OrdersService,
    private route: ActivatedRoute,
    public snackBar: SnackbarService,
    public translate: TranslateService
  ) { }

  ngOnInit(): void {
  }

  checkDivision(): boolean {
    let ind = this.documents.documents.findIndex(x => x.documentGroupId === this.selectedDocumentGroupId);
    if (ind < 0) {
      return true;
    } else {
      switch (this.documents.documents[ind].type.toString()) {
        case DocumentFolderTypeEnum[DocumentFolderTypeEnum.CustomerDocument].toString():
          if (this.divisions.includes(DivisionTypeEnum[DivisionTypeEnum.Customer])) {
            return true;
          } else {
            return false;
          }

        case DocumentFolderTypeEnum[DocumentFolderTypeEnum.PartnerDocument].toString():
          if (this.divisions.includes(DivisionTypeEnum[DivisionTypeEnum.Partner]) || this.divisions.includes(DivisionTypeEnum[DivisionTypeEnum.Admin])) {
            return true;
          } else {
            return false;
          }

        case DocumentFolderTypeEnum[DocumentFolderTypeEnum.ProductionDocument].toString():
          if (this.divisions.includes(DivisionTypeEnum[DivisionTypeEnum.Partner]) || this.divisions.includes(DivisionTypeEnum[DivisionTypeEnum.Customer])) {
            return false;
          } else {
            return true;
          }

        default:
          return true;
      }
    }
  }

  openNewDocumentDialog(): void {
    this.orderService.getDocumentFolderTypes().subscribe(res => {
      const dialogRef = this.dialog.open(OrderDocumentUploadComponent, {
        width: '48rem',
        data: {
          dropdown: res,
          documentGroupId: this.selectedDocumentGroupId,
          folderId: this.selectedFolderId,
          versionId: this.selectedVersionId
        }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result !== undefined) {
          this.orderService.postDocuments(result, this.route.snapshot.paramMap.get('id')).subscribe(() => {
            this.snackBar.customMessage(this.translate.instant('snackbar.success'));
            this.saved.emit();
          });
        }
      });
    });
  }

  onVersionClicked(index: number) {
    if (this.documents.versionable[this.versionOpenedIndex].status !== (DocumentStateEnum.Declined || DocumentStateEnum.Approved)) {
      this.selectedDocumentGroupId = this.documents.versionable[this.versionOpenedIndex].documentGroupId;
      this.selectedFolderId = this.documents.versionable[this.versionOpenedIndex].folderId;
      this.selectedVersionId = index;
    }
  }

  downloadDoc(id: string, name: string, event) {
    event.stopPropagation();
    this.orderService.getDocument(id).subscribe(res => {
      saveAs(res.data, name);
    });
  }

  expandPanel(matExpansionPanel, event, versionIndex, documentGroupId, folderId): void {
    this.openedIndex = -1;
    this.selectedVersionId = undefined;
    if (!this._isExpansionIndicator(event.target)) {
      matExpansionPanel.close();
      this.versionOpenedIndex = -1;
      this.selectedDocumentGroupId = documentGroupId;
      this.selectedFolderId = folderId;
    } else {
      if (this.documents.versionable[versionIndex].versions.length === 0) {
        matExpansionPanel.close();
        this.snackBar.customMessage(this.translate.instant('snackbar.notExpand'));
      } else {
        this.versionOpenedIndex = versionIndex;
        this.selectedDocumentGroupId = undefined;
        this.selectedFolderId = undefined;
      }
    }
  }

  private _isExpansionIndicator(target: EventTarget): boolean {
    const expansionIndicatorClass = 'mat-expansion-indicator';

    return (target['classList'] && target['classList'].contains(expansionIndicatorClass));
  }

}
