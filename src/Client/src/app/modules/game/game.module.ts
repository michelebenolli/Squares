import { NgModule } from '@angular/core';
import { GameRoutingModule } from './game-routing.module';
import { GameComponent } from './components/game/game.component';
import { SharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    GameComponent
  ],
  imports: [
    SharedModule,
    GameRoutingModule
  ]
})
export class GameModule { }
