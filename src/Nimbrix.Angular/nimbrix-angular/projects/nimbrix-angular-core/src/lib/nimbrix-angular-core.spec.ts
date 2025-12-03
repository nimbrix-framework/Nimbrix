import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NimbrixAngularCore } from './nimbrix-angular-core';

describe('NimbrixAngularCore', () => {
  let component: NimbrixAngularCore;
  let fixture: ComponentFixture<NimbrixAngularCore>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NimbrixAngularCore]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NimbrixAngularCore);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
