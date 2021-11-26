import { Pipe, PipeTransform } from "@angular/core"

@Pipe({
  name: "filter"
})
export class FilterPipe implements PipeTransform {
  transform(array: any, filter: string) {
    if (array && filter) {
      return array.filter((item:any) => this.containsProperty(item, filter) == true);
    }
    return array;
  }
  containsProperty(object:any,filter:string) {
    var state = false;
    filter = filter.toLowerCase();
    for (var i in object) {
      var value = String(object[i]);
      if (value.toLowerCase().indexOf(filter) !== -1) {
        state = true;
        break;
      }
    }
    return state;
  }
}
