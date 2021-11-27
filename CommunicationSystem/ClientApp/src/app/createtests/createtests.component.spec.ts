import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatetestsComponent } from './createtests.component';

describe('CreatetestsComponent', () => {
  let component: CreatetestsComponent;
  let fixture: ComponentFixture<CreatetestsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreatetestsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreatetestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
