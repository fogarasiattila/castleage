import { Group } from './group';
import { Player } from './player';

export interface GroupsAndPlayersDto {
  groups: Group[];
  players: Player[];
}
