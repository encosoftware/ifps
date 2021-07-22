import { Component, OnInit } from '@angular/core';
import { WorkloadService } from '../services/workload.service';
import { WorkStationsPlansListViewModel } from '../models/workload';
import { map, tap, finalize } from 'rxjs/operators';
import { differenceInHours, isAfter, isBefore, differenceInMinutes, differenceInDays, differenceInYears, startOfDay } from 'date-fns';

@Component({
  selector: 'factory-workload',
  templateUrl: './workload.component.html',
  styleUrls: ['./workload.component.scss']
})
export class WorkloadComponent implements OnInit {

  workLoad: WorkStationsPlansListViewModel;
  objectKeys = Object.keys;
  objectValues = Object.values;
  startDay = startOfDay(new Date());
  isLoading = false;


  constructor(private workloadService: WorkloadService) { }

  ngOnInit() {
    this.isLoading = true;

    this.workloadService.getWorkStationsPlans().pipe(
      map(res => this.workLoad = res),
      finalize(() => {this.isLoading = false}),
    ).subscribe();
  }

}
