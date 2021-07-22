import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { LoadingService } from '../../services/loading.service';

@Component({
  selector: 'webshop-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.scss']
})
export class LoadingComponent {
  isLoading: Observable<boolean>;
  constructor(private loaderService: LoadingService) {
    this.isLoading = this.loaderService.isLoading$;
  }

}
