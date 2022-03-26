import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import {
  _const_newGroupName,
  _const_newGroupNameREgxp,
} from 'src/app/enums/groupEnum';
import { PlayerService } from 'src/app/services/player.service';
import { Group } from 'src/interfaces/group';

@Component({
  selector: 'app-edit-group',
  templateUrl: './edit-group.component.html',
  styleUrls: ['./edit-group.component.css'],
})
export class EditGroupComponent implements OnInit {
  @Output() deleteGroup = new EventEmitter<Group>();
  @Input() groupChange$: Observable<Group>;

  form = new FormGroup({
    groupName: new FormControl({ value: null, disabled: true }, [
      Validators.required,
      this.newGroupValidator.bind(this),
    ]),
  });

  set groupFromForm(value: string) {
    this.form.patchValue({ groupName: value });
  }
  get groupFromForm(): string {
    return this.form.get('groupName').value;
  }

  buttonActive = false;

  constructor(private playerService: PlayerService) {}

  ngOnInit(): void {
    this.groupChange$.subscribe({
      next: (g) => {
        this.groupFromForm = g.name;
      },
    });
  }

  newGroupValidator(control: FormControl): { [s: string]: boolean } {
    if (_const_newGroupNameREgxp.exec(control.value))
      return { invalidGroupName: true };
    // if (this.groupNames.includes(control.value)) return { nameExists: true };
    return null;
  }

  onReset() {
    // if (this.groupSelected) {
    //   this.form.reset();
    //   this.form.get('groupName').setErrors(null);
  }

  onSave() {}

  onDelete() {
    // this.deleteGroup.emit(this.groupSelected);
  }
}
