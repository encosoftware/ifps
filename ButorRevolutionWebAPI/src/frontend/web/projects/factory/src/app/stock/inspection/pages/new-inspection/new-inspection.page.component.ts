import { Component, OnInit } from '@angular/core';
import { InspectionService } from '../../services/inspection.service';
import { IInspectionViewModel } from '../../models/inspection.model';
import { ActivatedRoute } from '@angular/router';

@Component({
    templateUrl: './new-inspection.page.component.html',
    styleUrls: ['./new-inspection.page.component.scss']
})
export class NewInspectionPageComponent implements OnInit {

    isLoading = false;

    inspection: IInspectionViewModel;

    id: number;

    constructor(
        private route: ActivatedRoute,
        private inspectionService: InspectionService
    ) { }

    ngOnInit(): void {
        this.id = +this.route.snapshot.paramMap.get('id');
        this.isLoading = true;
        this.inspectionService.getInspectionReport(this.id).subscribe(res => {
            this.inspection = res;
        },
            () => { },
            () => this.isLoading = false);
    }

}
