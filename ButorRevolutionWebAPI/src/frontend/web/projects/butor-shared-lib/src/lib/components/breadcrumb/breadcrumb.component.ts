import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, PRIMARY_OUTLET } from '@angular/router';
import { BreadCrumb } from '../../models/breadcrumb.model';
import { Location } from '@angular/common';

@Component({
  selector: 'butor-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.scss']
})
export class BreadcrumbComponent implements OnInit {

  public breadcrumbs: BreadCrumb[] = [];

  constructor(
    public router: Router,
    public route: ActivatedRoute,
    public location: Location
  ) { }

  ngOnInit() {
    this.breadcrumbs = this.getBreadcrumbs(this.route.root);
  }
  capitalize = (s) => {
    if (typeof s !== 'string') return ''
    return s.charAt(0).toUpperCase() + s.slice(1);
  }
  getBreadcrumbs(route: ActivatedRoute, url: string = '', breadcrumbs: BreadCrumb[] = []): BreadCrumb[] {
    const ROUTE_DATA_BREADCRUMB = 'breadcrumb';
    // get the child routes
    let children: ActivatedRoute[] = route.children;
    // return if there are no more children
    if (children.length === 0) {
      return breadcrumbs;
    }

    // iterate over each children
    for (let child of children) {
      // verify primary route
      if (child.outlet !== PRIMARY_OUTLET || child.snapshot.url.length === 0) {
        continue;
      }
      // verify the custom data property "breadcrumb" is specified on the route
      if (!child.snapshot.data.hasOwnProperty(ROUTE_DATA_BREADCRUMB)) {
        return this.getBreadcrumbs(child, url, breadcrumbs);
      }

      // get the route's URL segment
      let routeURL: string = child.snapshot.url.map(segment => segment.path).join('/');

      // append route URL to URL
      url += `/${routeURL}`;

      // add breadcrumb
      let breadcrumb: BreadCrumb = {
        // label: child.snapshot.data[ROUTE_DATA_BREADCRUMB],
        label: this.capitalize(routeURL),
        url
      };
      breadcrumbs.push(breadcrumb);

      // recursive
      return this.getBreadcrumbs(child, url, breadcrumbs);
    }
    return breadcrumbs;
  }

  back() {
    this.location.back();
  }
}
