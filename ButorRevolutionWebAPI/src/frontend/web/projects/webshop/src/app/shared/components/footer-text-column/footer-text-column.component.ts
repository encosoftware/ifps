import { Component, OnInit, Input } from "@angular/core";

@Component({
  selector: "app-footer-text-column",
  templateUrl: "./footer-text-column.component.html",
  styleUrls: ["./footer-text-column.component.scss"]
})
export class FooterTextColumnComponent implements OnInit {
  @Input() title: string;

  constructor() {}

  ngOnInit() {}
}
