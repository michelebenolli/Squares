import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GameComponent } from './components/game/game.component';
import { GameResolver } from './resolvers/game.resolver';

const routes: Routes = [
  {
    path: '',
    component: GameComponent,
    resolve: { data: GameResolver }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GameRoutingModule { }
