import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-overlay',
  templateUrl: './overlay.component.html',
  styleUrls: ['./overlay.component.scss']
})
export class OverlayComponent {
  
  @Input() dismissable = true;
  @Input() style?: { [name: string]: any }
  @Output() onShow = new EventEmitter();
  @Output() onHide = new EventEmitter();
  isOpen = false;
  target: any;

  toggle(target: any) {
    this.isOpen ? this.hide() : this.show(target);
  }

  show(target: any) {
    this.target = target;
    this.isOpen = true;
    this.onShow.emit();
  }

  hide() {
    this.isOpen = false;
    this.onHide.emit();
  }
}