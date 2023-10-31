import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DeleteDialogComponent } from 'src/app/shared/components/delete-dialog/delete-dialog.component';
import { TableAction } from 'src/app/shared/components/table/models/table-action';
import { TableColumn } from 'src/app/shared/components/table/models/table-column';
import { TableComponent } from 'src/app/shared/components/table/table.component';
import { PagedList } from 'src/app/shared/models/paged-list';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Game } from '../../models/game';
import { GameService } from '../../services/game.service';
import { DataType } from 'src/app/shared/components/table/models/data-type';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.scss']
})
export class GameComponent implements OnInit {

  items?: PagedList<Game>;
  @ViewChild(TableComponent) table?: TableComponent;

  columns: TableColumn<Game>[] = [
    { name: 'Utente', value: x => x.user?.firstName + ' ' + x.user?.lastName },
    { name: 'Punteggio', value: x => x.score, sort: 'score' },
    { name: 'Data', value: x => x.dateTime, type: DataType.DateTime, sort: 'dateTime' },
  ];

  actions: TableAction<Game>[] = [
    { name: 'delete', action: this.remove, icon: 'trash', dialog: DeleteDialogComponent, type: 'row' }
  ];

  constructor(
    private gameService: GameService,
    private notification: NotificationService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe((x: any) => this.items = x.data);
  }

  openAction(event: any) {
    (this as any)[event.action](event.data);
  }

  remove(game: Game) {
    this.gameService.delete(game.id).subscribe(() => {
      this.table?.getItems();
      this.notification.success('message.deleted');
    });
  }
}
