import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlayersListComponent } from './players-list/players-list.component';
import { MaterialModule } from '../material/material.module';
import { GroupsListComponent } from './groups-list/groups-list.component';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { EditGroupsComponent } from './edit-groups/edit-groups.component';

@NgModule({
  declarations: [
    PlayersListComponent,
    GroupsListComponent,
    EditGroupsComponent,
  ],
  imports: [CommonModule, MaterialModule, ReactiveFormsModule, FormsModule],
  exports: [PlayersListComponent, GroupsListComponent, EditGroupsComponent],
})
export class PlayerModule {}
