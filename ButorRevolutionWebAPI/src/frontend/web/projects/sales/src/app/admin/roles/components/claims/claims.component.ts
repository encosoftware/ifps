import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { RolesService } from '../../services/roles.service';
import { IModule } from '../../models/claims.model';

@Component({
  selector: 'butor-claim-list',
  templateUrl: './claims.component.html',
  styleUrls: ['./claims.component.scss']
})
export class ClaimsComponent {

  @Input()
  model: IModule[];

  constructor(
  ) { }

  handleCheckbox() {
    for (let i = 0; i < this.model.length; i++) {
      this.updateModuleCheckboxClass(i);
    }
  }

  toggleClaim(mainIndex: number, subIndex: number) {
    this.model[mainIndex].claims[subIndex].enabled = !this.model[mainIndex].claims[subIndex].enabled;
    this.updateModuleCheckboxClass(mainIndex);
  }

  toggleModule(index: number) {
    const currentClass = this.model[index].checkmarkClass;
    const currentEnabled = this.model[index].enabled;
    if (currentEnabled === false) {
      this.model[index].checkmarkClass = 'form__checkbox-button';
      this.model[index].enabled = true;
      for (let item of this.model[index].claims) {
        item.enabled = true;
      }
    }
    if (currentEnabled === true && currentClass === 'form__checkbox-button') {
      this.model[index].enabled = false;
      for (let item of this.model[index].claims) {
        item.enabled = false;
      }
    }
    if (currentEnabled === true && currentClass === 'form__checkbox-button-line') {
      this.model[index].enabled = false;
      for (let item of this.model[index].claims) {
        item.enabled = false;
      }
    }

  }

  updateModuleCheckboxClass(mainIndex: number) {
    let claimIsAllTrue: boolean;
    let claimIsAllFalse: boolean;
    claimIsAllTrue = this.model[mainIndex].claims.every((value) => this.isAllTrue(value.enabled));
    claimIsAllFalse = this.model[mainIndex].claims.every((value) => this.isAllFalse(value.enabled));
    if (claimIsAllFalse) {
      this.model[mainIndex].checkmarkClass = 'form__checkbox-button-line';
      this.model[mainIndex].enabled = false;
    }
    if (!claimIsAllFalse && !claimIsAllTrue) {
      this.model[mainIndex].checkmarkClass = 'form__checkbox-button-line';
      this.model[mainIndex].enabled = true;
    }
    if (claimIsAllTrue) {
      this.model[mainIndex].checkmarkClass = 'form__checkbox-button';
      this.model[mainIndex].enabled = true;
    }
  }

  isAllTrue(currentValue) {
    return currentValue === true;
  }

  isAllFalse(currentValue) {
    return currentValue === false;
  }

}
