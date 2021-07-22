import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { UploadPicService } from '../../services/upload-pic.service';
import { of } from 'rxjs';
import { catchError, tap, switchMap } from 'rxjs/operators';

@Component({
  selector: 'factory-upload-pic',
  templateUrl: './upload-pic.component.html',
  styleUrls: ['./upload-pic.component.scss']
})
export class UploadPicComponent implements OnInit {

  selectedPic = null;
  uploadeLabel = 'There is no uploaded picture yet.';
  error = '';
  @Input() folder: string;
  @Input() name: string;
  @Output() nameOutput = new EventEmitter();
  @Output() folderOutput = new EventEmitter();

  previewUrl: string | ArrayBuffer;

  constructor(
    private service: UploadPicService,
  ) { }

  ngOnInit() {
    if (this.folder && this.name) {
      this.service.getThumbnail(this.folder, this.name).pipe(
        catchError((err) => of(err.message))
      ).subscribe(
        resp => this.previewUrl = resp
      );
    } else {
      this.previewUrl = '/assets/icons/photoplaceholder_.jpg';
    }
  }

  onFileSelected(event) {
    this.selectedPic = event.target.files[0] as File;
    if (this.selectedPic) {
      this.service.UploadFile(this.selectedPic, this.folder).pipe(
        tap((res) => {
          if (res.item2) {
            this.name = res.item2;
            this.nameOutput.emit(this.name);
          }
          if (res.item1) {
            this.folder = res.item1;
            this.folderOutput.emit(this.folder);
          }
        }),
        switchMap(res => this.service.getThumbnail(this.folder, this.name).pipe(
          tap((resp) => this.previewUrl = resp),
          catchError((err) => of(err.message))
        )),
        catchError((err) => of(err.message))
      ).subscribe();
    }
  }


}
