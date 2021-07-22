import { Injectable, AfterViewInit } from '@angular/core';
import { Observable, concat, of, BehaviorSubject } from 'rxjs';
import { share, reduce, map, delay } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class LoadingService {
  
  private load = new BehaviorSubject<number>(0);

  isLoading$: Observable<boolean>;

  loadStarted() {
    this.load.next(+1);
  }
  loadEnded() {
    this.load.next(-1);
  }
  constructor() {
    const source = this.load.pipe(
      share(),
    );
    const totalOb = source.pipe(
      reduce((acc, curr: number) =>  curr + acc, 0),
    );
    this.isLoading$ = concat(source, totalOb).pipe(
      // reduce((acc, curr: number) => curr + acc, 0),
      delay(0),
      map(x => x > 0),
    );
  }
}
