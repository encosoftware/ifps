import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { IProductsCorpusDetailsViewModel } from '../../models/corpus.model';
import { ProductsService } from '../../services/products.service';
import { IFoilListModel } from '../../models/products.models';
import { IMaterialCodeViewModel } from '../../models/front.model';

@Component({
  selector: 'butor-corpus',
  templateUrl: './corpus.component.html'
})
export class CorpusComponent implements OnInit, OnDestroy {

  materials: IMaterialCodeViewModel[];
  currentMaterial: IMaterialCodeViewModel;
  foils: IFoilListModel[];
  checkBoxAble = false;
  destroy$ = new Subject();
  corpusForm: FormGroup;
  componentTitle = 'ADD CORPUS';

  corpus: IProductsCorpusDetailsViewModel;
  corpusId: string;

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
    this.corpus = {
      furnitureUnitId: null,
      id: null,
      amount: null,
      name: '',
      picture: {
        containerName: 'MaterialPictures',
        fileName: ''
      },
      size: {
        depth: null,
        height: null,
        width: null
      },
      bottomId: null,
      leftId: null,
      rightId: null,
      topId: null,
      materialId: null,
      typeId: null
    };
    this.currentMaterial = {
      code: '',
      containerName: '',
      description: '',
      fileName: '',
      id: null,
      src: ''
    };
    this.buildForm();
    this.allCheckboxDisable();
    this.corpus.furnitureUnitId = this.data.unitId;
    this.corpusId = this.data.componentId ? this.data.componentId : null;
    if (this.corpusId !== null) {
      this.isLoading = true;
      this.componentTitle = 'EDIT CORPUS';
      this.service.getFurnitureComponent(this.corpusId).subscribe(res => {
        this.corpus.picture.containerName = res.picture.containerName;
        this.corpus.picture.fileName = res.picture.fileName;
        this.corpusForm.controls.name.setValue(res.name);
        this.corpusForm.controls.code.setValue(res.materialId);
        this.corpusForm.controls.amount.setValue(res.amount);
        this.corpusForm.controls.size.get('width').setValue(res.size.width);
        this.corpusForm.controls.size.get('height').setValue(res.size.height);
        this.corpusForm.controls.edging.get('top').setValue(res.topId);
        this.corpusForm.controls.edging.get('bottom').setValue(res.bottomId);
        this.corpusForm.controls.edging.get('right').setValue(res.rightId);
        this.corpusForm.controls.edging.get('left').setValue(res.leftId);
        if (
          res.topId === res.bottomId
          && res.topId === res.rightId
          && res.topId === res.leftId
        ) {
          this.corpusForm.controls.edging.get('all').setValue(res.bottomId);
          this.allCheckboxEnable();
        } else {
          this.allCheckboxDisable();
        }
        this.service.getDecorMaterialCodes().subscribe(result => {
          this.materials = result;
          if (this.materials.find(x => x.id === this.corpusForm.controls.code.value) !== undefined) {
            this.currentMaterial = this.materials.find(x => x.id === this.corpusForm.controls.code.value);
          }
        });
      },
        () => { },
        () => this.isLoading = false);
    } else {
      this.service.getDecorMaterialCodes().subscribe(result => {
        this.materials = result;
      });
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
    this.corpusForm.valueChanges.pipe(
      takeUntil(this.destroy$)
    ).subscribe();
  }

  get f() { return this.corpusForm.controls; }

  onSubmit() {
    this.submitted = true;

    if (this.corpusForm.invalid) {
      return;
    }

    this.save();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.unsubscribe();
  }

  toggleDisabled(event) {
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
    this.corpusForm.controls.materialDescription.setValue(this.currentMaterial.description);
  }

  cancel() {
    this.dialogRef.close();
  }

  save() {
    this.corpus.name = this.corpusForm.controls.name.value;
    this.corpus.materialId = this.corpusForm.controls.code.value;
    this.corpus.amount = this.corpusForm.controls.amount.value;
    this.corpus.size.width = this.corpusForm.controls.size.get('width').value;
    this.corpus.size.height = this.corpusForm.controls.size.get('height').value;
    this.corpus.typeId = this.data.corpusTypeId;
    if (!this.checkBoxAble) {
      this.corpus.bottomId = this.corpusForm.controls.edging.get('bottom').value;
      this.corpus.topId = this.corpusForm.controls.edging.get('top').value;
      this.corpus.rightId = this.corpusForm.controls.edging.get('right').value;
      this.corpus.leftId = this.corpusForm.controls.edging.get('left').value;
    } else {
      this.corpus.bottomId = this.corpusForm.controls.edging.get('all').value;
      this.corpus.topId = this.corpusForm.controls.edging.get('all').value;
      this.corpus.rightId = this.corpusForm.controls.edging.get('all').value;
      this.corpus.leftId = this.corpusForm.controls.edging.get('all').value;
    }
    if (this.corpusId === null) {
      this.service.postCorpus(this.corpus).subscribe(res => this.dialogRef.close('done'));
    } else {
      this.service.putFurnitureComponent(this.corpusId, this.corpus).subscribe(res => this.dialogRef.close('done'));
    }

  }

  changeAll(event: any) {
    this.corpusForm.controls.edging.get('top').setValue(event);
    this.corpusForm.controls.edging.get('bottom').setValue(event);
    this.corpusForm.controls.edging.get('right').setValue(event);
    this.corpusForm.controls.edging.get('left').setValue(event);
  }

  getFileName(event): void {
    this.corpus.picture.fileName = event;
  }

  getFolderName(event): void {
    this.corpus.picture.containerName = event;
  }

  allCheckboxEnable(): void {
    this.checkBoxAble = true;
    this.corpusForm.controls.edging.get('all').enable();
    this.corpusForm.controls.edging.get('top').disable();
    this.corpusForm.controls.edging.get('bottom').disable();
    this.corpusForm.controls.edging.get('right').disable();
    this.corpusForm.controls.edging.get('left').disable();
  }

  allCheckboxDisable(): void {
    this.checkBoxAble = false;
    this.corpusForm.controls.edging.get('all').disable();
    this.corpusForm.controls.edging.get('all').setValue(undefined);
    this.corpusForm.controls.edging.get('top').enable();
    this.corpusForm.controls.edging.get('bottom').enable();
    this.corpusForm.controls.edging.get('right').enable();
    this.corpusForm.controls.edging.get('left').enable();
  }

  buildForm(): void {
    this.corpusForm = this.formBuilder.group({
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
