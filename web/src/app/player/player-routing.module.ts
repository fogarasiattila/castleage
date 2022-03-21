import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GroupsComponent } from './groups/groups.component';

const routes: Routes = [{ path: 'players', component: GroupsComponent }],
  config = { onSameUrlNavigation: 'reload' };

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [],
})
export class PlayerRoutingModule {}
