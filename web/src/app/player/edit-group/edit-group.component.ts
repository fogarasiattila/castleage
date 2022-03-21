import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-edit-group',
  templateUrl: './edit-group.component.html',
  styleUrls: ['./edit-group.component.css'],
})
export class EditGroupComponent implements OnInit {
  form = new FormGroup({
    groupName: new FormControl(),
  });

  constructor() {}

  ngOnInit(): void {
    this.form.patchValue({
      groupName: 'foo',
    });
  }
}
