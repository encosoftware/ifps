import { Injectable } from '@angular/core';
import { ApiWorkloadsClient } from '../../../shared/clients';
import { map } from 'rxjs/operators';
import { WorkStationsPlansListViewModel } from '../models/workload';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WorkloadService {

  constructor(private workload: ApiWorkloadsClient) { }

  getWorkStationsPlans(): Observable<WorkStationsPlansListViewModel | null> {
    return this.workload.getWorkStationsPlans().pipe(
      map(res => ({
        cuttings: res.cuttings ? res.cuttings.map(ins => ({
          id: ins.id,
          name: ins.name ? ins.name : undefined,
          endTime: ins.endTime ? 
          new Date(Date.UTC(ins.endTime.getFullYear(), ins.endTime.getMonth(), ins.endTime.getDate(), ins.endTime.getHours(), ins.endTime.getMinutes())) 
          : undefined,
          backgroundColor:'rgba(255, 244, 209, 1)'
        })) : undefined,
        cncs: res.cncs ? res.cncs.map(ins => ({
          id: ins.id,
          name: ins.name ? ins.name : undefined,
          endTime: ins.endTime ? new Date(Date.UTC(ins.endTime.getFullYear(), ins.endTime.getMonth(), ins.endTime.getDate(), ins.endTime.getHours(), ins.endTime.getMinutes())) : undefined,
          backgroundColor:'rgba(255, 209, 209, 1)'
        })) : undefined,
        edgebandings: res.edgebandings ? res.edgebandings.map(ins => ({
          id: ins.id,
          name: ins.name ? ins.name : undefined,
          endTime: ins.endTime ? new Date(Date.UTC(ins.endTime.getFullYear(), ins.endTime.getMonth(), ins.endTime.getDate(), ins.endTime.getHours(), ins.endTime.getMinutes())) : undefined,
          backgroundColor:'rgba(235, 255, 209, 1)'
        })) : undefined,
        assemblies: res.assemblies ? res.assemblies.map(ins => ({
          id: ins.id,
          name: ins.name ? ins.name : undefined,
          endTime: ins.endTime ? new Date(Date.UTC(ins.endTime.getFullYear(), ins.endTime.getMonth(), ins.endTime.getDate(), ins.endTime.getHours(), ins.endTime.getMinutes())) : undefined,
          backgroundColor:'rgba(209, 255, 244, 1)'
        })) : undefined,
        sortings: res.sortings ? res.sortings.map(ins => ({
          id: ins.id,
          name: ins.name ? ins.name : undefined,
          endTime: ins.endTime ? new Date(Date.UTC(ins.endTime.getFullYear(), ins.endTime.getMonth(), ins.endTime.getDate(), ins.endTime.getHours(), ins.endTime.getMinutes())) : undefined,
          backgroundColor:'rgba(224, 86, 253,1)'
        })) : undefined,
        packings: res.packings ? res.packings.map(ins => ({
          id: ins.id,
          name: ins.name ? ins.name : undefined,
          endTime: ins.endTime ? new Date(Date.UTC(ins.endTime.getFullYear(), ins.endTime.getMonth(), ins.endTime.getDate(), ins.endTime.getHours(), ins.endTime.getMinutes())) : undefined,
          backgroundColor:'rgba(32, 191, 107,1)'
        })) : undefined,
      }))
    )
  }

}
