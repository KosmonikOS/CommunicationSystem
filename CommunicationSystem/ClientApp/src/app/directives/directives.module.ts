import { NgModule } from "@angular/core"
import { CommonModule } from "@angular/common"
import { ErrorDirective } from "./error.directive"

@NgModule({
  imports: [
    CommonModule,
  ],
  declarations: [
    ErrorDirective
  ]
  ,
  exports: [
    ErrorDirective
  ]
})
export class DirectivesModule { }
