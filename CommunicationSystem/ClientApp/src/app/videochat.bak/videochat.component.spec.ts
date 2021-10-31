import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideochatComponent } from './videochat.component';

describe('VideochatComponent', () => {
  let component: VideochatComponent;
  let fixture: ComponentFixture<VideochatComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VideochatComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VideochatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
