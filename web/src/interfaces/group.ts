export class Group {
  constructor(id, name) {
    this.id = id;
    this.name = name;
  }

  id: number;
  name: string;
  touched = false;
  deleted = false;
}
