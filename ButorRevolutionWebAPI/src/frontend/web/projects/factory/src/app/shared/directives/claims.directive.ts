import { Directive, OnInit, OnDestroy, TemplateRef, ViewContainerRef, Input } from '@angular/core';
import { BehaviorSubject, Subscription, combineLatest } from 'rxjs';
import { Store, select } from '@ngrx/store';
import { Claims } from '../../core/store/actions/core.actions';
import { coreClaims } from '../../core/store/selectors/core.selector';
import { map } from 'rxjs/operators';

@Directive({
  selector: '[factoryClaims]'
})
export class ClaimsDirective implements OnInit, OnDestroy  {
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
        this.factoryClaimsCondition(claims, requiredRoles);
      if (condition && !this.hasView) {
        this.viewContainer.createEmbeddedView(this.templateRef);
        this.hasView = true;
      } else if (!condition && this.hasView) {
        this.viewContainer.clear();
        this.hasView = false;
      }
    });
  }

  @Input() set factoryClaims(claim: string) {
    this.getClaimsBehSubj.next(claim);
  }
  private factoryClaimsCondition(mainA: string[], claimA: string[]): boolean {
    const arrayClaim = claimA.map(claim => !!mainA.find((c) => c === claim));
    return arrayClaim.includes(true);

  }
  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

}
