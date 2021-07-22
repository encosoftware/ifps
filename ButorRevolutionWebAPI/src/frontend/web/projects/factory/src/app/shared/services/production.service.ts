import { Injectable } from '@angular/core';
import { ApiProductionprocessClient } from '../clients';
import { Observable } from 'rxjs';
import { Store, select } from '@ngrx/store';
import { coreLoginT } from '../../core/store/selectors/core.selector';
import { take, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProductionService {

  userId: string;

  constructor(
    private processClient: ApiProductionprocessClient,
    private store: Store<any>,
  ) {
    this.store.pipe(
      select(coreLoginT),
      take(1),
      tap((resp) => {
        this.userId = resp.UserId;
      })
    ).subscribe();
  }

  setProcessStatus(productionId: number): Observable<void> {
    return this.processClient.setProcessStatus(productionId);
  }
}
