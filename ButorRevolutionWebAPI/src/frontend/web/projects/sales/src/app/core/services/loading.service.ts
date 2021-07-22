import { Injectable } from '@angular/core';
import { Subject, BehaviorSubject, merge, Observable, concat, of } from 'rxjs';
import { map, reduce, tap, share } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {

  private load = new BehaviorSubject<number>(0);

  isLoading$: Observable<boolean> = of(false);

  loadStarted() {
    this.load.next(this.load.value + 1);
  }
  loadEnded() {
    this.load.next(this.load.value - 1);
  }
  constructor() {
    const source = this.load.pipe(
      share(),
    );
    const totalOb = source.pipe(
      reduce((acc, curr: number) => {return curr + acc;}, 0),
    );

    this.isLoading$ = concat(source, totalOb).pipe(
      map(x => x > 0),
    );
  }
}
