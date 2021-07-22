import { Component, OnInit, Inject, OnDestroy, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { IProductsFrontDetailsViewModel, IMaterialCodeViewModel } from '../../models/front.model';
import { ProductsService } from '../../services/products.service';
import { IFoilListModel } from '../../models/products.models';


@Component({
  selector: 'butor-front',
  templateUrl: './front.component.html'
})
export class FrontComponent implements OnInit, OnDestroy {

  materials: IMaterialCodeViewModel[];
  currentMaterial: IMaterialCodeViewModel;
  foils: IFoilListModel[];
  checkBoxAble = false;
  destroy$ = new Subject();
  frontForm: FormGroup;
  componentTitle = 'ADD FRONT';

  front: IProductsFrontDetailsViewModel;
  frontId: string;

  isLoading = false;
  submitted = false;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<any>,
    public snackBar: MatSnackBar,
    private service: ProductsService
  ) { }

  ngOnInit() {
    this.front = {
      furnitureUnitId: null,
      id: null,
      amount: null,
      bottomId: null,
      name: '',
      leftId: null,
      materialId: null,
      picture: {
        containerName: 'MaterialPictures',
        fileName: ''
      },
      rightId: null,
      size: {
        width: null,
        height: null
      },
      topId: null
    };
    this.currentMaterial = {
      code: '',
      containerName: '',
      description: '',
      fileName: '',
      id: null,
      src: ''
    };
    this.service.getDecorMaterialCodes().subscribe(res => this.materials = res);
    this.buildForm();
    this.allCheckboxDisable();
    this.front.furnitureUnitId = this.data.unitId;
    this.frontId = this.data.componentId ? this.data.componentId : null;
    if (this.frontId !== null) {
      this.isLoading = true;
      this.componentTitle = 'EDIT FRONT';
      this.service.getFurnitureComponent(this.frontId).subscribe(res => {
        this.front.picture.containerName = res.picture.containerName;
        this.front.picture.fileName = res.picture.fileName;
        this.frontForm.controls.name.setValue(res.name);
        this.frontForm.controls.code.setValue(res.materialId);
        this.frontForm.controls.amount.setValue(res.amount);
        this.frontForm.controls.size.get('width').setValue(res.size.width);
        this.frontForm.controls.size.get('height').setValue(res.size.height);
        this.frontForm.controls.edging.get('top').setValue(res.topId);
        this.frontForm.controls.edging.get('bottom').setValue(res.bottomId);
        this.frontForm.controls.edging.get('right').setValue(res.rightId);
        this.frontForm.controls.edging.get('left').setValue(res.leftId);
        if (
          res.topId === res.bottomId
          && res.topId === res.rightId
          && res.topId === res.leftId
        ) {
          this.frontForm.controls.edging.get('all').setValue(res.bottomId);
          this.allCheckboxEnable();
        } else {
          this.allCheckboxDisable();
        }
        this.service.getDecorMaterialCodes().subscribe(result => {
          this.materials = result;
          if (this.materials.find(x => x.id === this.frontForm.controls.code.value) !== undefined) {
            this.currentMaterial = this.materials.find(x => x.id === this.frontForm.controls.code.value);
          }
        });
      },
        () => { },
        () => this.isLoading = false);
    }
    this.service.getFoils().subscribe(res => {
      this.foils = [];
      res.forEach(foil => {
        const tempFoil: IFoilListModel = {
          code: foil.code,
          id: foil.id
        };
        this.foils.push(tempFoil);
      });
    });
    this.frontForm.valueChanges.pipe(
      takeUntil(this.destroy$)
    ).subscribe();
  }

  get f() { return this.frontForm.controls; }

  onSubmit() {
    this.submitted = true;

    if (this.frontForm.invalid) {
      return;
    }

    this.save();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.unsubscribe();
  }

  toggleDisabled(event: any) {
    if (event !== true || event !== false) {
      event.stopPropagation();
    }
    if (this.checkBoxAble) {
      this.allCheckboxDisable();
    } else {
      this.allCheckboxEnable();
    }
  }

  changeMaterial(id: string): void {
    this.currentMaterial = this.materials.find(x => x.id === id);
    this.frontForm.controls.materialDescription.setValue(this.currentMaterial.description);
  }

  cancel(): void {
    this.dialogRef.close();
  }

  save() {
    this.front.name = this.frontForm.controls.name.value;
    this.front.materialId = this.frontForm.controls.code.value;
    this.front.amount = this.frontForm.controls.amount.value;
    this.front.picture.containerName = this.currentMaterial.containerName;
    this.front.picture.fileName = this.currentMaterial.fileName;
    this.front.size.width = this.frontForm.controls.size.get('width').value;
    this.front.size.height = this.frontForm.controls.size.get('height').value;
    this.front.typeId = this.data.frontTypeId;
    if (!this.checkBoxAble) {
      this.front.bottomId = this.frontForm.controls.edging.get('bottom').value;
      this.front.topId = this.frontForm.controls.edging.get('top').value;
      this.front.rightId = this.frontForm.controls.edging.get('right').value;
      this.front.leftId = this.frontForm.controls.edging.get('left').value;
    } else {
      this.front.bottomId = this.frontForm.controls.edging.get('all').value;
      this.front.topId = this.frontForm.controls.edging.get('all').value;
      this.front.rightId = this.frontForm.controls.edging.get('all').value;
      this.front.leftId = this.frontForm.controls.edging.get('all').value;
    }
    if (this.frontId === null) {
      this.service.postFront(this.front).subscribe(res => this.dialogRef.close('done'));
    } else {
      this.service.putFurnitureComponent(this.frontId, this.front).subscribe(res => this.dialogRef.close('done'));
    }

  }

  changeAll(event) {
    this.frontForm.controls.edging.get('top').setValue(event);
    this.frontForm.controls.edging.get('bottom').setValue(event);
    this.frontForm.controls.edging.get('right').setValue(event);
    this.frontForm.controls.edging.get('left').setValue(event);
  }

  getFileName(event): void {
    this.front.picture.fileName = event;
  }

  getFolderName(event): void {
    this.front.picture.containerName = event;
  }

  allCheckboxEnable(): void {
    this.checkBoxAble = true;
    this.frontForm.controls.edging.get('all').enable();
    this.frontForm.controls.edging.get('top').disable();
    this.frontForm.controls.edging.get('bottom').disable();
    this.frontForm.controls.edging.get('right').disable();
    this.frontForm.controls.edging.get('left').disable();
  }

  allCheckboxDisable(): void {
    this.checkBoxAble = false;
    this.frontForm.controls.edging.get('all').disable();
    this.frontForm.controls.edging.get('all').setValue(undefined);
    this.frontForm.controls.edging.get('top').enable();
    this.frontForm.controls.edging.get('bottom').enable();
    this.frontForm.controls.edging.get('right').enable();
    this.frontForm.controls.edging.get('left').enable();
  }

  buildForm(): void {
    this.frontForm = this.formBuilder.group({
      name: ['', Validators.required],
      code: ['', Validators.required],
      size: this.formBuilder.group({
        width: [undefined, Validators.required],
        height: [undefined, Validators.required]
      }),
      materialDescription: '',
      amount: [undefined, Validators.required],
      edging: this.formBuilder.group({
        all: [undefined, Validators.required],
        top: [undefined, Validators.required],
        bottom: [undefined, Validators.required],
        right: [undefined, Validators.required],
        left: [undefined, Validators.required]
      })
    });
  }
}
