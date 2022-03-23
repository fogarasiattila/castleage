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

type GroupRename = { srcGrp: number; dstGrp: Group; compId: number };

@Component({
  selector: 'app-edit-group',
  templateUrl: './edit-group.component.html',
  styleUrls: ['./edit-group.component.css'],
})
export class EditGroupComponent implements OnInit {
  _group: Group;
  @Output() groupRename = new EventEmitter<GroupRename>();
  @Input() compId: number;

  @Input()
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
    return null;
  }

  onReset() {
    if (this._group.name) {
      this.form.reset();
      this.form.get('groupName').setErrors(null);

      this.groupName = this._group.name;
    }
  }

  onSave() {
    if (!this.form.valid) return;
    const saveGroup: Group = {
      id: this._group.id,
      name: this.groupName,
    };
    this.playerService.sendGroup(saveGroup).subscribe({
      next: (r) => {
        this.groupRename.emit({
          srcGrp: saveGroup.id,
          dstGrp: r,
          compId: this.compId,
        });
      },
      error: (e) => console.log(e.message),
    });
  }
}
