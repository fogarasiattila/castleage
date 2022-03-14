import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlayersListComponent } from './players-list/players-list.component';
import { MaterialModule } from '../material/material.module';
import { GroupsListComponent } from './groups-list/groups-list.component';

@NgModule({
  declarations: [PlayersListComponent, GroupsListComponent],
  imports: [CommonModule, MaterialModule],
  exports: [PlayersListComponent, GroupsListComponent],
})
export class PlayerModule {}
