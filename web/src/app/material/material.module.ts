import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatCheckboxModule } from '@angular/material/checkbox';

const imports = [MatButtonModule, MatTableModule, MatCheckboxModule];
const exports = imports;

@NgModule({
  declarations: [],
  imports: [imports],
  exports: [exports],
})
export class MaterialModule {}
