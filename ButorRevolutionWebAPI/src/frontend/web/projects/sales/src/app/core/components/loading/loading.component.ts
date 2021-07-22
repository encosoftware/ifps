import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Observable } from 'rxjs';
import { LoadingService } from '../../services/loading.service';
import { delay, map } from 'rxjs/operators';

@Component({
  selector: 'butor-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.scss']
})
export class LoadingComponent implements OnInit {
  isLoading: boolean;
  constructor(private loaderService: LoadingService) {
    this.loaderService.isLoading$.pipe(delay(0), map(x => this.isLoading = x)).subscribe();
  }

  ngOnInit() {
  }

}
