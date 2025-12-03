import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NimbrixAngularTesting } from './nimbrix-angular-testing';

describe('NimbrixAngularTesting', () => {
  let component: NimbrixAngularTesting;
  let fixture: ComponentFixture<NimbrixAngularTesting>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NimbrixAngularTesting]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NimbrixAngularTesting);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
