import { Component, OnInit } from '@angular/core';
import { Test } from "../tests/test"

@Component({
  selector: 'app-createtests',
  templateUrl: './createtests.component.html',
  styleUrls: ['./createtests.component.css', '../app.component.css']
})
export class CreatetestsComponent implements OnInit {
  tests: Test[] = [];
  currentTest: Test = new Test;
  currentRow: number = -1;
  search: string = "";
  constructor() { }

  fileSelected(event: any) {
    var file = <File>event.target.files[0];
  }

  ngOnInit(): void {
  }

}
