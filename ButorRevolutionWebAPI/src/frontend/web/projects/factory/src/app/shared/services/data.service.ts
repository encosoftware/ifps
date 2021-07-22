import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  private idsSource = new BehaviorSubject([]);
  actualIds = this.idsSource.asObservable();
  supplyerID: number;

  constructor() { }

  changeIds(ids: number[]): void {
    this.idsSource.next(ids);
  }
}
