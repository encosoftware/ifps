import { Routes, RouterModule } from '@angular/router';

import { NgModule } from '@angular/core';

const routes: Routes = [
    {
        path: 'order-scheduling',
        loadChildren: () => import('./order-scheduling/order-scheduling.module').then(m => m.OrderSchedulingModule)
    },
    {
        path: 'optimizations',
        loadChildren: () => import('./optimizations/optimization.module').then(m => m.OptimizationModule)
    },
    {
        path: 'assembly',
        loadChildren: () => import('./assembly/assembly.module').then(m => m.AssemblyModule)
    },
    {
        path: 'cnc',
        loadChildren: () => import('./cnc/cnc.module').then(m => m.CncModule)
    },
    {
        path: 'cuttings',
        loadChildren: () => import('./cuttings/cuttings.module').then(m => m.CuttingsModule)
    },
    {
        path: 'edgebanding',
        loadChildren: () => import('./edgebanding/edgebanding.module').then(m => m.EdgebandingModule)
    },
    {
        path: 'machines',
        loadChildren: () => import('./machines/machines.module').then(m => m.MachinesModule)
    },
    {
        path: 'cameras',
        loadChildren: () => import('./cameras/cameras.module').then(m => m.CamerasModule)
    },
    {
        path: 'workstations',
        loadChildren: () => import('./workstations/workstations.module').then(m => m.WorkstationsModule)
    },
    {
        path: 'workload',
        loadChildren: () => import('./workload/workload.module').then(m => m.WorkloadModule)
    },
    {
        path: 'sorting',
        loadChildren: () => import('./sorting/sorting.module').then(m => m.SortingModule)
    },
    {
        path: 'packing',
        loadChildren: () => import('./packing/packing.module').then(m => m.PackingModule)
    },
    {
        path: 'furniture-units',
        loadChildren: () => import('./furniture-units/furniture-units.module').then(m => m.FurnitureUnitsModule)
    }
];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ProductionRoutingModule { }
