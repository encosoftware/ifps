import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CategoriesService } from '../../services/categories.service';
import { flattenObjArray } from '../../../../../utils/flatten';
import { tap, catchError } from 'rxjs/operators';
import { EditCategory } from '../../models/categories.models';
import { LanguageTypeEnum } from '../../../../shared/clients';

@Component({
  selector: 'butor-new-item',
  templateUrl: './new-item.component.html',
  styleUrls: ['./new-item.component.scss']
})
export class NewItemComponent implements OnInit {
  items: EditCategory = {
    id: null,
    translation: [{
      id: null,
      name: null,
      language: LanguageTypeEnum.EN
    }, {
      id: null,
      name: null,
      language: LanguageTypeEnum.HU
    }],
    parentId: null,
    type: null
  };

  parentSelect = [];

  error: string | null = null;

  languageTypeEnum = LanguageTypeEnum;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<any>,
    public categoriesService: CategoriesService
  ) { }

  ngOnInit() {
    if (this.data.action === 'edit') {
      // TODO: lehetséges megoldás a parent category-k listázására de nem teljesen jó
      // forkJoin([
      //   this.categoriesService.getById(this.data.id),
      //   this.categoriesService.getHierarchicalCategories(
      //     GroupingCategoryEnum[GroupingCategoryEnum[this.data.id]],
      //     LanguageTypeEnum.EN
      //   )
      // ]).pipe(
      //   map(([byId, categories]) => {
      //     const tempParents = flattenObjArray(categories, 'children');
      //     this.parentSelect = tempParents.filter(x => x.id === byId.parentId);
      //     this.items = byId;
      //     this.items.parentId = this.data.id;
      //   })
      // ).subscribe();

      this.parentSelect = [];
      this.categoriesService.getById(this.data.id).pipe(
        tap((resp: EditCategory) => {
          this.items = resp;
          this.items.parentId = this.items.parentId;
        }),
        catchError(() => this.error = 'Error: could not load data'),
      ).subscribe();
    } else if (this.data.action === 'add') {
      this.parentSelect = [{ id: this.data.id, name: this.data.name }];
      this.items.parentId = this.data.id;
    } else {
      this.categoriesService.getRootCategories(undefined, LanguageTypeEnum.EN, true).pipe(
        tap(valu => { this.parentSelect = flattenObjArray(valu, 'children'); })
      ).subscribe();
    }

  }

  cancel(): void {
    this.dialogRef.close();
  }

  save() {
    if (this.items.id) {
      this.categoriesService.updateGroupingCategory(this.items.id, this.items).pipe(
        tap(() => {
          this.dialogRef.close(this.items.parentId);
        })
      ).subscribe();
    } else {
      this.categoriesService.post(this.items).pipe(
        tap(() => {
          this.dialogRef.close(this.items.parentId);
        })
      ).subscribe();
    }
  }
}
