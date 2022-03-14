import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { PlayerService } from 'src/app/services/player.service';
import { Player } from 'src/interfaces/player';
import { SelectionModel } from '@angular/cdk/collections';

@Component({
  selector: 'app-players-list',
  templateUrl: './players-list.component.html',
  styleUrls: ['./players-list.component.css'],
})
export class PlayersListComponent implements OnInit {
  players: Player[] = [];
  columnsToDisplay: string[] = ['PlayerDisplayName', 'Selection'];
  initialSelection: Player[] = [];
  allowMultiSelect = true;
  selection = new SelectionModel<Player>(
    this.allowMultiSelect,
    this.initialSelection
  );

  constructor(private playerService: PlayerService) {}

  ngOnInit(): void {
    this.playerService.getPlayers().subscribe({
      next: (response: Player[]) => {
        this.players = response;
      },
      error: () => console.log('error'),
    });
  }

  selectPlayer(player: Player) {}

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.players.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected()
      ? this.selection.clear()
      : this.players.forEach((p) => {
          this.selection.select(p);
        });
  }
}
