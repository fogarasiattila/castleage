import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../material/material.module';
import { GroupsListComponent } from './groups-list/groups-list.component';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { GroupsComponent } from './groups/groups.component';
import { PlayerRoutingModule } from './player-routing.module';
import { EditGroupComponent } from './edit-group/edit-group.component';

@NgModule({
  declarations: [GroupsListComponent, GroupsComponent, EditGroupComponent],
  imports: [CommonModule, MaterialModule, ReactiveFormsModule, FormsModule],
  exports: [GroupsListComponent, GroupsComponent, PlayerRoutingModule],
})
export class PlayerModule {}
