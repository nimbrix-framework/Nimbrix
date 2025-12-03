import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NimbrixAngularCqrs } from './nimbrix-angular-cqrs';

describe('NimbrixAngularCqrs', () => {
  let component: NimbrixAngularCqrs;
  let fixture: ComponentFixture<NimbrixAngularCqrs>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NimbrixAngularCqrs]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NimbrixAngularCqrs);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
