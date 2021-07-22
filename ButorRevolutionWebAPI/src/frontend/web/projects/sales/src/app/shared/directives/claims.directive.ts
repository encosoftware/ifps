import { Directive, TemplateRef, ViewContainerRef, Input, OnInit, OnDestroy } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { map } from 'rxjs/operators';
import { BehaviorSubject, combineLatest, Subscription } from 'rxjs';
import { coreClaims } from '../../core/store/selectors/core.selector';
import { Claims } from '../../core/store/actions/core.actions';

@Directive({
  selector: '[butorClaims]'
})
export class ClaimsDirective implements OnInit, OnDestroy {
  private getClaimsBehSubj: BehaviorSubject<string | string[]> = new BehaviorSubject('');
  private subscription: Subscription;
  private hasView = false;


  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private store: Store<any>,

  ) { }

  ngOnInit(): void {
    if (localStorage.getItem('m-claim') !== 'undefined') {
      const claim = localStorage.getItem('m-claim');
      this.store.dispatch(new Claims(JSON.parse(claim)));
    }
    this.subscription = combineLatest([
      this.store.pipe(
        select(coreClaims),
        map((claim) => claim),
      ),
      this.getClaimsBehSubj.asObservable()
    ]).subscribe(([claims, requiredRoles]: [string[], string | string[]]) => {
      const condition = (typeof requiredRoles === 'string') ?
        !!claims.find(claim => claim === requiredRoles) :
        this.butorClaimsCondition(claims, requiredRoles);
      if (condition && !this.hasView) {
        this.viewContainer.createEmbeddedView(this.templateRef);
        this.hasView = true;
      } else if (!condition && this.hasView) {
        this.viewContainer.clear();
        this.hasView = false;
      }
    });
  }

  @Input() set butorClaims(claim: string) {
    this.getClaimsBehSubj.next(claim);
  }
  private butorClaimsCondition(mainA: string[], claimA: string[]): boolean {
    const arrayClaim = claimA.map(claim => !!mainA.find((c) => c === claim));
    return arrayClaim.includes(true);

  }
  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
