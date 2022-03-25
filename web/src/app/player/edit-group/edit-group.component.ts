import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { Form, FormControl, FormGroup, Validators } from '@angular/forms';
import {
  GroupEnum,
  _const_newGroupName,
  _const_newGroupNameREgxp,
} from 'src/app/enums/groupEnum';
import { PlayerService } from 'src/app/services/player.service';
import { Group } from 'src/interfaces/group';
import { GroupsComponent } from '../groups/groups.component';

@Component({
  selector: 'app-edit-group',
  templateUrl: './edit-group.component.html',
  styleUrls: ['./edit-group.component.css'],
})
export class EditGroupComponent implements OnInit {
  _group: Group;
  @Output() createGroup = new EventEmitter<string>();
  @Output() deleteGroup = new EventEmitter<Group>();
  @Input() compId: number;
  @Input() groupNames: string[];
  set groupSelected(value: Group) {
    if (!value) return;
    this._group = value;
    this.form.patchValue({
      groupName: value.name,
    });
    if (value.id === GroupEnum.Mindenki) {
      this.form.get('groupName').disable();
      this.buttonActive = false;
    } else {
      this.form.get('groupName').enable();
      this.buttonActive = true;
    }
  }
  get groupSelected() {
    return this._group;
  }

  set groupName(value: string) {
    this.form.patchValue({
      groupName: value,
    });
  }
  get groupName() {
    return this.form.get('groupName').value;
  }

  form = new FormGroup({
    groupName: new FormControl({ value: null, disabled: true }, [
      Validators.required,
      this.newGroupValidator.bind(this),
    ]),
  });

  buttonActive = false;

  constructor(private playerService: PlayerService) {}

  ngOnInit(): void {}

  newGroupValidator(control: FormControl): { [s: string]: boolean } {
    if (_const_newGroupNameREgxp.exec(control.value))
      return { invalidGroupName: true };
    if (this.groupNames.includes(control.value)) return { nameExists: true };
    return null;
  }

  onReset() {
    if (this.groupSelected) {
      this.form.reset();
      this.form.get('groupName').setErrors(null);

      this.groupName = this.groupSelected.name;
    }
  }

  onSave() {
    if (!this.form.valid || this.groupNames.includes(this.groupName)) return;

    if (this.groupSelected.id === GroupEnum.NewGroup) {
      //ha <Új Csoport>, akkor előbb hozzon létre egy új csoportot
      this.createGroup.emit(this.groupName);
    } else {
      this.groupSelected.name = this.groupName;
      this.onReset();
    }
  }

  onDelete() {
    this.deleteGroup.emit(this.groupSelected);
  }
}
