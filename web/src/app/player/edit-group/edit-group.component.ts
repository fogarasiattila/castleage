import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import {
  GroupEnum,
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
export class EditGroupComponent implements OnInit, OnDestroy {
  @Output() deleteGroup = new EventEmitter<Group>();
  @Input() groupChange$: BehaviorSubject<Group>;
  groupChangeSubscription: Subscription;
  groupStateSubscription: Subscription;

  groupNames: string[] = [];
  group: Group;

  form = new FormGroup({
    groupName: new FormControl({ value: null }, [
      Validators.required,
      this.newGroupValidator.bind(this),
    ]),
  });

  set groupName(value: string) {
    this.form.patchValue({ groupName: value });
  }
  get groupName(): string {
    return this.form.get('groupName').value;
  }

  buttonActive = false;

  constructor(private playerService: PlayerService) {}

  ngOnInit(): void {
    this.groupChangeSubscription = this.groupChange$.subscribe({
      next: (g) => {
        this.reset(g);
      },
    });
    this.groupStateSubscription = this.playerService.groupsState$.subscribe({
      next: (groups) => (this.groupNames = groups.map((g) => g.name)),
    });
  }

  newGroupValidator(control: FormControl): { [s: string]: boolean } {
    if (_const_newGroupNameREgxp.exec(control.value))
      return { invalidGroupName: true };
    if (this.groupNames.includes(control.value)) return { nameExists: true };
    return null;
  }

  reset(group: Group) {
    this.form.reset();
    this.groupName = group.name;
    this.form.get('groupName').setErrors(null);
    this.group = group;
    if (group.id === GroupEnum.Mindenki || group.id === GroupEnum.NewGroup)
      this.form.get('groupName').disable();
    else this.form.get('groupName').enable();
  }

  onSave() {
    if (
      this.group.id === GroupEnum.Mindenki ||
      this.group.id === GroupEnum.NewGroup
    )
      return;
    this.group.name = this.groupName;
    this.group.touched = true;
  }

  onReset() {
    this.reset(this.group);
  }

  onDelete() {
    this.group.touched = true;
    this.group.deleted = true;
    this.deleteGroup.emit(this.group);
  }

  ngOnDestroy(): void {
    this.groupChangeSubscription.unsubscribe();
    this.groupStateSubscription.unsubscribe();
  }
}
