import { Component, OnInit, Input } from '@angular/core';
import { ComponentRendering } from '@sitecore-jss/sitecore-jss-angular';

@Component({
  selector: 'app-text-teaser',
  templateUrl: './text-teaser.component.html',
  styleUrls: ['./text-teaser.component.css']
})
export class TextTeaserComponent implements OnInit {
  @Input() rendering: ComponentRendering;

  constructor() { }

  ngOnInit() {
    // remove this after implementation is done
    console.log('TextTeaser component initialized with component data', this.rendering);
  }
}
