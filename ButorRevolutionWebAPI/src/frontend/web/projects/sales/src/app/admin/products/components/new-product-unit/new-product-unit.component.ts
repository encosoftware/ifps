import { Component, OnInit } from '@angular/core';
import { IProductCreateModel, IFurnitureUnitCategoryModel } from '../../models/products.models';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ProductsService } from '../../services/products.service';
import { Router } from '@angular/router';
import { SnackbarService } from 'butor-shared-lib';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'butor-new-product-unit',
  templateUrl: './new-product-unit.component.html'
})
export class NewProductUnitComponent implements OnInit {

  isLoading = false;
  submitted = false;
  product: IProductCreateModel = {
    categoryId: null,
    furnitureUnitTypeId: null,
    code: '',
    description: '',
    picture: {
      fileName: '',
      containerName: 'MaterialPictures'
    },
    size: {
      depth: null,
      height: null,
      width: null,
    }
  };
  productId: number;
  selectedCategory: number;
  productCategories: IFurnitureUnitCategoryModel[];
  furnitureTypes: IFurnitureUnitCategoryModel[];

  productUnitForm: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    private service: ProductsService,
    private router: Router,
    public snackBar: SnackbarService,
    public translate: TranslateService
  ) { }

  ngOnInit() {
    this.productUnitForm = this.formBuilder.group({
      code: ['', Validators.required],
      description: [''],
      depth: ['', Validators.required],
      height: ['', Validators.required],
      width: ['', Validators.required],
      category: [null, Validators.required],
      picture: ['', Validators.required],
      furnitureUnitTypeId: [null, Validators.required]
    });

    this.service.getFurnitureUnitCategories().subscribe(res => this.productCategories = res);
    this.service.getFurnitureUnitIds().subscribe(res => this.furnitureTypes = res);
  }

  getFileName(event): void {
    this.productUnitForm.controls.picture.setValue(event);
    this.product.picture.fileName = event;
  }

  getFolderName(event): void {
    this.product.picture.containerName = event;
  }

  addNewProductUnit(): void {
    this.isLoading = true;
    this.product.categoryId = this.productUnitForm.controls.category.value;
    this.product.code = this.productUnitForm.controls.code.value;
    this.product.description = this.productUnitForm.controls.description.value;
    this.product.size.depth = this.productUnitForm.controls.depth.value;
    this.product.size.height = this.productUnitForm.controls.height.value;
    this.product.size.width = this.productUnitForm.controls.width.value;
    this.product.furnitureUnitTypeId = this.productUnitForm.controls.furnitureUnitTypeId.value;
    this.service.postProducts(this.product).subscribe(id => {
      this.snackBar.customMessage(this.translate.instant('snackbar.success'));
      this.dialogRef.close();
      this.router.navigate(['/admin/products/' + id]);
    },
      () => { },
      () => this.isLoading = false);
  }

  get f() { return this.productUnitForm.controls; }

  cancel(): void {
    this.dialogRef.close();
  }

  onSubmit() {
    this.submitted = true;

    if (this.productUnitForm.invalid) {
      return;
    }

    this.addNewProductUnit();
  }

}
