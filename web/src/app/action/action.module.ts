import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActionComponent } from './action.component';
import { ActionRoutingModule } from './action-routing.module';

@NgModule({
  declarations: [ActionComponent],
  imports: [CommonModule],
  exports: [ActionRoutingModule],
})
export class ActionModule {}
