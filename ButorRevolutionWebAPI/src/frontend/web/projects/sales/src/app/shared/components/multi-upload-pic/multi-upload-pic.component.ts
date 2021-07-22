import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { of, forkJoin } from 'rxjs';
import { catchError, tap, map, concatMap } from 'rxjs/operators';
import { UploadPicService } from '../../services/upload-pic.service';
import { ImageDetailsViewModel } from '../../../core/models/auth';

@Component({
  selector: 'butor-multi-upload-pic',
  templateUrl: './multi-upload-pic.component.html',
  styleUrls: ['./multi-upload-pic.component.scss']
})
export class MultiUploadPicComponent implements OnInit {
  selectedPic: File[] = [];
  error = '';
  imagesAll: ImageDetailsViewModel[] = [];
  @Input() set images(images: ImageDetailsViewModel[]) {
    this.ImagesView = [];
    if (images) {
      this.imagesAll = images.map(res => res);
      images.map(res => {
        this.service.getThumbnail(res.containerName, res.fileName).pipe(
          map( res => this.ImagesView = [...this.ImagesView, res]),
          catchError((err) => of(err.message))
        ).subscribe();
      });
    }
  }
  @Input() folder: string;
  @Output() multiPics = new EventEmitter();
  ImagesView: (string | ArrayBuffer)[] = [];


  constructor(
    private service: UploadPicService,
  ) { }

  ngOnInit() {

  }

  onFileSelected(event) {
    this.selectedPic = event.target.files as File[];
    if (this.selectedPic) {
      this.service.createImages(this.selectedPic, this.folder).pipe(
        map(res => res.map(ins => ({
          containerName: ins.item1,
          fileName: ins.item2
        }))),
        tap((res) => {
          if (res) {
            this.imagesAll = [...this.imagesAll,...res];
            this.multiPics.emit(this.imagesAll);
          }
        }),
        concatMap(res => {
          const all = res.map(inp =>
            this.service.getThumbnail(inp.containerName, inp.fileName).pipe(
              tap((resp) => this.ImagesView = [...this.ImagesView, resp]),
              catchError((err) => of(err.message))
            ));
          return forkJoin(...all);
        }
        ),
        catchError((err) => of(err.message))
      ).subscribe();
    }
  }

  deletePic(index: number) {
    this.ImagesView.splice(index, 1);
    this.imagesAll.splice(index, 1);
    this.multiPics.emit(this.imagesAll);
  }
}
