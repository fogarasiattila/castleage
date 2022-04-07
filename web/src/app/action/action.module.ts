import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActionComponent } from './action.component';
import { ActionRoutingModule } from './action-routing.module';
import { PlayerModule } from '../player/player.module';
import { MaterialModule } from '../material/material.module';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [ActionComponent],
  imports: [CommonModule, PlayerModule, MaterialModule, FormsModule],
  exports: [ActionRoutingModule],
})
export class ActionModule {}
