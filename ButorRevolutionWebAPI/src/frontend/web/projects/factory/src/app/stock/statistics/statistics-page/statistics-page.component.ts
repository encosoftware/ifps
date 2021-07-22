import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { Label, Color, BaseChartDirective } from 'ng2-charts';
import * as pluginAnnotations from 'chartjs-plugin-annotation';
import { StatisticsService } from '../services/statistics.service';
import { IMaterialsDropdown, IGroupingMaterialsDropdown } from '../models/material.model';
import { TranslateService } from '@ngx-translate/core';
@Component({
  selector: 'factory-statistics-page',
  templateUrl: './statistics-page.component.html',
  styleUrls: ['./statistics-page.component.scss']
})
export class StatisticsPageComponent implements OnInit {

  @ViewChild(BaseChartDirective) chart: BaseChartDirective;

  public lineChartData: ChartDataSets[] = [
    { data: [], label: '' }
  ];
  public lineChartLabels: Label[] = [];
  public lineChartOptions: (ChartOptions & { annotation: any }) = {
    responsive: true,
    scales: {
      // We use this empty structure as a placeholder for dynamic theming.
      xAxes: [{
        scaleLabel: {
          display: true,
          labelString: this.translate.instant('StockStatistics.date'),
          fontColor: '#4D41FF',
          fontFamily: 'Poppins',
          fontStyle: '600',
          fontSize: 13
        },
        ticks: {
          beginAtZero: true,
          fontSize: 13
        },
        gridLines: {
          display: false
        }
      }],
      yAxes: [
        {
          id: 'y-axis-0',
          position: 'left',
          ticks: {
            beginAtZero: true,
            fontSize: 13
          },
          scaleLabel: {
            display: true,
            labelString: this.translate.instant('StockStatistics.sheet'),
            fontColor: '#4D41FF',
            fontFamily: 'Poppins',
            fontStyle: '600',
            fontSize: 13
          }
        }
      ]
    },
    annotation: {
      annotations: [
      ],
    },
  };
  public lineChartColors: Color[] = [
    {
      backgroundColor: 'transparent',
      borderColor: '#4D41FF',
      pointBackgroundColor: '#4D41FF',
      pointRadius: 7,
      pointBorderColor: '#E6E4FFBF',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)',
    }
  ];
  public lineChartLegend = false;
  public lineChartType = 'line';
  public lineChartPlugins = [pluginAnnotations];
  dates = [];

  materials: IMaterialsDropdown[] = [];
  materialGroup: IGroupingMaterialsDropdown[];
  selectedGroupId: number;
  statisticsForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private translate: TranslateService,
    private service: StatisticsService
  ) { }

  ngOnInit() {
    this.service.getOldestStatisticsYear().subscribe(year => {
      let currentYear = new Date().getFullYear();
      for (let i = currentYear; i >= year; i--) {
        let temp = {
          name: i,
          year: i
        };
        this.dates = [...this.dates, temp];
      }
    });

    this.statisticsForm = this.formBuilder.group({
      materialGroup: null,
      material: null,
      date: null,
    });

    this.service.getMaterialGroup().subscribe(res => {
      this.materialGroup = res;
    });

    this.statisticsForm.controls.materialGroup.valueChanges.subscribe(res => {
      this.service.getMaterials(res).subscribe(val => {
        this.materials = val;
        this.statisticsForm.controls.material.setValue(val[0].id);
      });
    });

    this.statisticsForm.valueChanges.subscribe(res => {
      if (res.materialGroup !== null && res.material !== null && res.date !== null) {
        this.service.getStatistics(res.material, res.date).subscribe(val => {
          this.lineChartData[0].data = Array<number>();
          this.lineChartLabels = [];
          const tempData = Array<number>();
          val.data.forEach(x => {
            this.lineChartLabels.push(x.weekNumber.toString());
            tempData.push(x.quantity);
          });
          this.lineChartData[0].data = tempData;
        });
      }
    });
  }

}
