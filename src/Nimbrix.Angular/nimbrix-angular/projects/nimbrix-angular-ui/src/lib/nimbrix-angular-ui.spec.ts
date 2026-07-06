import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NimbrixAngularUi } from './nimbrix-angular-ui';

describe('NimbrixAngularUi', () => {
  let component: NimbrixAngularUi;
  let fixture: ComponentFixture<NimbrixAngularUi>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NimbrixAngularUi]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NimbrixAngularUi);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
