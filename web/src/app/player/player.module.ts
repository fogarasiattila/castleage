import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlayersListComponent } from './players-list/players-list.component';
import { MaterialModule } from '../material/material.module';
import { GroupsListComponent } from './groups-list/groups-list.component';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [PlayersListComponent, GroupsListComponent],
  imports: [CommonModule, MaterialModule, ReactiveFormsModule, FormsModule],
  exports: [PlayersListComponent, GroupsListComponent],
})
export class PlayerModule {}
