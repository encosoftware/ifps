import { Component, OnInit } from '@angular/core';
import { IimagesSliderModel } from '../../shared/models/image-slider';
import { HeaderSharedService } from '../../shared/services/header-shared.service';
import { IGroupingCategoryWebshopViewModel } from '../models/home';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  images: IimagesSliderModel[] = [];
  menuCategories: IGroupingCategoryWebshopViewModel[];
  row = 0;
  constructor(private headerSharedService: HeaderSharedService) { }

  ngOnInit() {


    this.headerSharedService.menu.subscribe(
      resp => this.menuCategories = resp
    );

  }

 

}
