import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlayersListComponent } from './players-list/players-list.component';
import { MaterialModule } from '../material/material.module';

@NgModule({
  declarations: [PlayersListComponent],
  imports: [CommonModule, MaterialModule],
  exports: [PlayersListComponent],
})
export class PlayerModule {}
