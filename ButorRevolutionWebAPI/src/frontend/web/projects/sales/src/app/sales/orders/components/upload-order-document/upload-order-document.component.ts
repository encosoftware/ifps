import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FileUploaderService } from '../../services/file-uploader.service';
import { IUploadingFile } from '../../models/fileUpdate.model';
import { IFolderDropDownViewModel, IOrderUploadedDocumentItem, IOrderDocumentCreateViewModel } from '../../models/orders';

@Component({
    selector: 'butor-order-document-upload',
    templateUrl: './upload-order-document.component.html',
    styleUrls: ['./upload-order-document.component.scss']
})
export class OrderDocumentUploadComponent implements OnInit {

    folder: IFolderDropDownViewModel;

    files: IUploadingFile[] = [];

    fileResults: IOrderUploadedDocumentItem[] = [];

    uploadDocumentOrderForm: FormGroup;
    constructor(
        @Inject(MAT_DIALOG_DATA) public data: any,
        private dialogRef: MatDialogRef<any>,
        private formBuilder: FormBuilder,
        private uploadService: FileUploaderService
    ) { }

    ngOnInit(): void {
        this.uploadDocumentOrderForm = this.formBuilder.group({
            documentType: [undefined, Validators.required]
        });
        let tmp = this.data.dropdown.filter(r => r.folderId === this.data.folderId);
        this.folder = tmp[0];
        this.uploadDocumentOrderForm.get('documentType').setValue(this.folder.supportedTypes[0].typeId);
    }

    deleteFile(name: string) {
        this.files = this.files.filter(f => f.name !== name);
        this.fileResults = this.fileResults.filter(f => f.originalFileName !== name);
    }

    onFileSelected(event) {
        if (event.target.files[0]) {
            if (this.files.findIndex(i => i.name === event.target.files[0].name) !== -1) {
                window.alert('This file is already uploaded!');
            } else {
                this.uploadService.upload(event.target.files[0]).subscribe((res) => {
                    if (res) {
                        switch (res.status) {
                            case 'started':
                                this.files.push({
                                    currentSize: 0,
                                    totalSize: 0,
                                    percentage: 0,
                                    name: res.name
                                });
                                break;
                            case 'progress':
                                let index = this.files.findIndex(i => i.name === res.name);
                                this.files[index].currentSize = res.loaded;
                                this.files[index].totalSize = res.total;
                                this.files[index].percentage = res.percentage;
                                break;
                            case 'uploaded':
                                this.fileResults.push({
                                    containerName: res.data.item1,
                                    fileName: res.data.item2,
                                    originalFileName: event.target.files[0].name
                                });
                                break;
                        }
                    }
                });
            }
        }
    }

    cancel() {
        this.dialogRef.close();
    }

    save(): any {
        let model: IOrderDocumentCreateViewModel = {
            folderId: this.data.documentGroupId,
            versionId: this.data.versionId,
            typeId: this.uploadDocumentOrderForm.get('documentType').value,
            documents: []
        };
        for (let document of this.fileResults) {
            model.documents.push({
                containerName: document.containerName,
                fileName: document.fileName
            });
        }
        this.dialogRef.close(model);
    }

}
