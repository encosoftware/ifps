import { Component, OnInit } from '@angular/core';
import { FlatTreeControl } from '@angular/cdk/tree';
import { MatDialog } from '@angular/material/dialog';
import { MatTreeFlattener, MatTreeFlatDataSource } from '@angular/material/tree';
import { CategoriesService } from '../services/categories.service';
import { tap, catchError } from 'rxjs/operators';
import { SnackbarService } from 'butor-shared-lib';
import { NewItemComponent } from '../components/new-item/new-item.component';
import { ExampleFlatNode, IViewCategory } from '../models/categories.models';
import { GroupingCategoryEnum, ClaimPolicyEnum } from '../../../shared/clients';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'butor-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit {

  selectedMainCat: number;
  selectSubCat: number;
  treeControl = new FlatTreeControl<ExampleFlatNode>(
    node => node.level, node => node.expandable);
  mainCategoriesName = [];
  error: string | null = null;
  type: GroupingCategoryEnum;
  claimPolicyEnum = ClaimPolicyEnum;

  private transformer = (node: IViewCategory, level: number) => {
    return {
      expandable: !!node.children && node.children.length > 0,
      name: node.name,
      level,
      id: node.id,
      parentId: node.parentId,
      type: node.groupingCategoryType
    };
  }

  // tslint:disable-next-line: member-ordering
  treeFlattener = new MatTreeFlattener(
    this.transformer, node => node.level, node => node.expandable, node => node.children);
  // tslint:disable-next-line: member-ordering
  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);
  // tslint:disable-next-line: member-ordering
  subDataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

  constructor(
    private categoriesService: CategoriesService,
    public dialog: MatDialog,
    public snackBar: SnackbarService,
    private translate: TranslateService,
  ) { }

  ngOnInit(): void {
    this.categoriesService.getRootCategories().pipe(
      tap(val => { this.mainCategoriesName = val; })
    ).subscribe();
  }

  hasChild = (_: number, node: ExampleFlatNode) => node.expandable;

  selectMainCat(type: GroupingCategoryEnum, idx: number) {
    this.categoriesService.getHierarchicalCategories(type).pipe(
      tap(val => {
        this.subDataSource.data = val[0].children;
        this.selectedMainCat = idx;
      })
    ).subscribe();
  }

  newItem(id?: number, action?: string, type?: any, name?: string): void {
    const dialogRef = this.dialog.open(NewItemComponent, {
      width: '60rem',
      data: {
        id: id || null,
        action: action || null,
        type: type || null,
        name: name || null
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        let index;
        if (type) {
          index = this.mainCategoriesName.findIndex(val => val.groupingCategoryType === type);
          if (index >= 0) {
            this.type = type;
            this.selectedMainCat = index;
          }
        } else {
          index = this.mainCategoriesName.findIndex(val => val.id === result);
          this.type = this.mainCategoriesName[index].groupingCategoryType;
          this.selectedMainCat = index;
        }
        this.selectMainCat(this.type, this.selectedMainCat);
      }
    });
  }

  deleteItem(index: number, type: GroupingCategoryEnum) {
    this.categoriesService.deleteGroupingCategory(index).pipe(
      catchError((err) => {
        const error = JSON.parse(err.response);
        this.error = error.children[0];
        this.snackBar.customMessage((this.error) ? this.error : this.translate.instant('snackbar.success'));
        return err;
      })
    ).subscribe(() => {
      let i = this.mainCategoriesName.findIndex(val => val.groupingCategoryType === type);
      this.snackBar.customMessage((this.error) ? this.error : this.translate.instant('snackbar.success'));
      this.selectMainCat(type, i);
    }
    );
  }
}
