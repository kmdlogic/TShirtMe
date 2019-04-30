import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckEntryComponent } from './check-entry.component';

describe('CheckEntryComponent', () => {
  let component: CheckEntryComponent;
  let fixture: ComponentFixture<CheckEntryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CheckEntryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
