import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class UserPicUpdateService {

    private dataSource = new BehaviorSubject<{ folder: string, pic: string }>(null);
    data = this.dataSource.asObservable();

    constructor() { }

    updateUserPic(imgFolder: string, imgName: string) {
        this.dataSource.next({ folder: imgFolder, pic: imgName });
    }
}
