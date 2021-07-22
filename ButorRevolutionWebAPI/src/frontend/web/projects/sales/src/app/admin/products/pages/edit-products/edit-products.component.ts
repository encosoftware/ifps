import { Component, OnInit, OnDestroy } from '@angular/core';
import { SnackbarService } from 'butor-shared-lib';
import { ProductsService } from '../../services/products.service';
import {
  IProductsListViewModel,
  IProductDetailsModel,
  IFurnitureUnitCategoryModel
} from '../../models/products.models';
import { map, takeUntil, catchError } from 'rxjs/operators';
import { FrontComponent } from '../../components/front/front.component';
import { MatDialog } from '@angular/material/dialog';
import { CorpusComponent } from '../../components/corpus/corpus.component';
import { AccessoryComponent } from '../../components/accessory/accessory.component';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, Observable } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { IProductsFrontViewModel } from '../../models/front.model';
import { IProductsCorpusViewModel } from '../../models/corpus.model';
import { IProductsAccessoriesViewModel } from '../../models/accessory.model';
import { DialogService } from '../../../../core/services/dialog.service';
import { TranslateService } from '@ngx-translate/core';
import { FurnitureComponentTypeEnum } from '../../../../shared/clients';

@Component({
  selector: 'butor-edit-products',
  templateUrl: './edit-products.component.html',
  styleUrls: ['./edit-products.component.scss']
})
export class EditProductsComponent implements OnInit, OnDestroy {

  destroy$ = new Subject();
  error: string | null = null;
  product: IProductDetailsModel;
  productId: string;
  selectedCategory: number;
  productCategories: IFurnitureUnitCategoryModel[];

  isLoading = false;
  submitted = false;

  editProductForm: FormGroup;

  onChange: (obj: IProductsListViewModel) => void;
  onTouched: () => void;

  frontDataSource: IProductsFrontViewModel[] = [];
  corpusDataSource: IProductsCorpusViewModel[] = [];
  accessoriesDataSource: IProductsAccessoriesViewModel[] = [];

  constructor(
    private productsService: ProductsService,
    private dialogService: DialogService,
    public dialog: MatDialog,
    private route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder,
    private translate: TranslateService,
    private snackBar: SnackbarService
  ) { }

  ngOnInit() {
    this.loadData();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.unsubscribe();
  }

  saveProduct(): void {
    this.isLoading = true;
    this.productsService.putProducts(this.productId, this.editProductForm.value, this.product.picture)
      .subscribe(() => {
        this.isLoading = false;
        this.snackBar.customMessage(this.translate.instant('snackbar.success'));
        this.editProductForm.markAsPristine();
      });
  }

  get hasError(): boolean {
    return !!this.error;
  }

  deleteButton(id: string) {
    this.productsService.deleteFurnitureComponent(id).subscribe(() => this.loadData());
  }

  deleteAccessoryButton(id: number) {
    this.productsService.deleteAccessoryFurniture(id).subscribe(() => this.loadData());
  }

  openFront(frontId?: number): void {
    const dialogRef = this.dialog.open(FrontComponent, {
      width: '98rem',
      data: {
        unitId: this.productId,
        componentId: frontId,
        frontTypeId: FurnitureComponentTypeEnum.Front
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.loadData();
      }
    });
  }

  openCorpus(corpusId?: number): void {
    const dialogRef = this.dialog.open(CorpusComponent, {
      width: '98rem',
      data: {
        unitId: this.productId,
        componentId: corpusId,
        corpusTypeId: FurnitureComponentTypeEnum.Corpus
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.loadData();
      }
    });
  }

  openAccessory(accessoryId?: number): void {
    const dialogRef = this.dialog.open(AccessoryComponent, {
      width: '48rem',
      data: {
        unitId: this.productId,
        componentId: accessoryId
      }

    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.loadData();
      }
    });
  }

  getFileName(event): void {
    this.product.picture.fileName = event;
  }

  getFolderName(event): void {
    this.product.picture.containerName = event;
  }

  cancel() {
    this.router.navigate(['/admin/products/']);
  }

  get f() { return this.editProductForm.controls; }

  onSubmit() {
    this.submitted = true;

    if (this.editProductForm.invalid) {
      return;
    }

    this.saveProduct();
  }

  loadData(): void {
    this.editProductForm = this.formBuilder.group({
      code: ['', Validators.required],
      description: ['', Validators.required],
      size: this.formBuilder.group({
        depth: ['', Validators.required],
        height: ['', Validators.required],
        width: ['', Validators.required]
      }),
      categoryId: [null, Validators.required],
      picture: ['', Validators.required]
    });

    this.product = {
      category: null,
      categoryId: 1,
      description: '',
      code: '',
      size: {
        height: null,
        depth: null,
        width: null
      },
      picture: {
        containerName: 'MaterialPictures',
        fileName: ''
      },
      front: [],
      accessories: [],
      corpus: []
    };
    this.productId = this.route.snapshot.paramMap.get('id');
    this.isLoading = true;
    this.productsService.getProductsEdit(this.productId).pipe(
      map(val => {
        this.product.picture.containerName = val.picture.containerName;
        this.product.picture.fileName = val.picture.fileName;
        this.frontDataSource = val.front;
        this.corpusDataSource = val.corpus;
        this.accessoriesDataSource = val.accessories;
        this.selectedCategory = val.categoryId;
        this.editProductForm.patchValue(val);
        this.frontDataSource.forEach(front => {
          front.edging.all = null;
          if (
            front.edging.top === front.edging.right
            && front.edging.top === front.edging.left
            && front.edging.top === front.edging.bottom
          ) {
            front.edging.all = front.edging.top;
          }
        });
        this.corpusDataSource.forEach(corpus => {
          corpus.edging.all = null;
          if (
            corpus.edging.top === corpus.edging.right
            && corpus.edging.top === corpus.edging.left
            && corpus.edging.top === corpus.edging.bottom
          ) {
            corpus.edging.all = corpus.edging.top;
          }
        });
      }),
      catchError(() => this.error = 'Error: could not load data'),
      takeUntil(this.destroy$)
    ).subscribe(res => this.isLoading = false);

    this.productsService.getFurnitureUnitCategories().subscribe(res => this.productCategories = res);

    this.editProductForm.valueChanges.pipe(
      takeUntil(this.destroy$)
    ).subscribe();
  }

  canDeactivate(): Observable<boolean> | boolean {
    if (this.editProductForm.pristine) {
      return true;
    }

    // tslint:disable-next-line:max-line-length
    return this.dialogService.confirm('It looks like you have been editing something. If you leave before saving, your changes will be lost. Leave anyway?');
  }
}
