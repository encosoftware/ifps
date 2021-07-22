import { Component } from '@angular/core';
import { ImageCroppedEvent } from 'ngx-image-cropper';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
    selector: 'butor-update-profile-picture',
    templateUrl: './update-profile-picture.component.html',
    styleUrls: ['update-profile-picture.component.scss']
})
export class UpdateProfilePictureComponent {

    imageChangedEvent: any = '';
    croppedImage: any = '';

    name: string;

    constructor(
        public dialogRef: MatDialogRef<any>
    ) {
    }

    imageCropped(event: ImageCroppedEvent) {
        this.croppedImage = event.base64;
    }

    fileChangeListener($event) {
        this.name = $event.target.files[0].name;
        this.imageChangedEvent = $event;
    }

    saveImage() {
        this.dialogRef.close({
            image: this.croppedImage,
            name: this.name
        });
    }

}
