import { Directive, ElementRef, Renderer2,Input, OnChanges, SimpleChanges } from "@angular/core"

@Directive({
  selector: "[error]",
})
export class ErrorDirective implements OnChanges {
  @Input("error") error: any;
  constructor(private elem: ElementRef, private render: Renderer2) {
  }
  ngOnChanges(changes: SimpleChanges): void {
    if (this.error.Error) {
      this.render.addClass(this.elem.nativeElement, "border-danger");
      this.render.addClass(this.elem.nativeElement, "input-danger");
      this.render.setAttribute(this.elem.nativeElement, "placeholder", this.error.Error[0]);
    } else {
      this.render.setAttribute(this.elem.nativeElement, "placeholder", this.error.PlaceHolder);
      this.render.removeClass(this.elem.nativeElement, "border-danger");
      this.render.removeClass(this.elem.nativeElement, "input-danger");
    }
    }
}
