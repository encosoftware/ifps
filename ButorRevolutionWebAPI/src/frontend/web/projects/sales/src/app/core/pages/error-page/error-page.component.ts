import { Component, OnInit, HostListener } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'butor-error-page',
  templateUrl: './error-page.component.html',
  styleUrls: ['./error-page.component.scss']
})
export class ErrorPageComponent implements OnInit {
  id: number;
 //TODO 
  easterEgg = '';
  @HostListener('window:keyup', ['$event'])
  keyEvent(event: KeyboardEvent) {
    this.easterEgg = this.easterEgg + event.key;
    if ((this.easterEgg.length > 6) && (this.easterEgg !== 'andris')) {
      this.easterEgg = '';
    } else if(event.key === 'a') {
      this.easterEgg = 'a'
    };
  }
  
  constructor(
    private route: ActivatedRoute,
    private router: Router,

  ) { }

  ngOnInit() {
    this.id = +this.route.snapshot.paramMap.get('id');
    if (this.id === 404 || 0) {
      this.id = this.id;
    } else if (this.id === 403) {
      this.id = this.id;
    } else {
      this.id = 0;
    }
  }

  activateToggle() {
    this.router.navigate(['/sales/dashboard']);
  }

}
