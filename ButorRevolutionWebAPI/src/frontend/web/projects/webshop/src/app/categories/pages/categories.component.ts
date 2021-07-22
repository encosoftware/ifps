import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoriesService } from '../services/categories.service';
import { IGroupingSubCategoryWebshopViewModel } from '../../home/models/home';
import { tap, map } from 'rxjs/operators';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit {
  id: number;
  name: string;
  categories: IGroupingSubCategoryWebshopViewModel[];
  constructor(
    private route: ActivatedRoute,
    private categoriesService: CategoriesService,
    ) {}

  ngOnInit() {
    this.id = +this.route.snapshot.queryParamMap.get('id');
    this.name = this.route.snapshot.paramMap.get('name');
    this.categoriesService.getSubcategeroiesByCategory(this.id).pipe(
    ).subscribe(
      categories => this.categories = categories
    );
  }

}
