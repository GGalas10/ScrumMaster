import { Routes } from '@angular/router';
import { RegisterComponent } from './features/auth/register/register.component';
import { LoginComponent } from './features/auth/login/login.component';
import { HomeComponent } from './features/home/home.component';
import { BoardComponent } from './features/board/board.component';
import { authGuard } from './Core/Guards/auth.guard';
import { checkLoginGuard } from './Core/Guards/check-login.guard';
import { ProjectComponent } from './features/project/project.component';
import { DetailsComponent } from './features/Users/details/details.component';
import { SprintComponent } from './features/board/sprint/sprint.component';
import { SprintBoardComponent } from './features/board/sprint-board/sprint-board.component';

export const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [checkLoginGuard] },
  { path: 'Login', component: LoginComponent, canActivate: [checkLoginGuard] },
  {
    path: 'Register',
    component: RegisterComponent,
    canActivate: [checkLoginGuard],
  },
  {
    path: 'Board/:id',
    component: BoardComponent,
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
  },
  {
    path: 'Project',
    component: ProjectComponent,
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
  },
  {
    path: 'User/Details/:id',
    runGuardsAndResolvers: 'always',
    component: DetailsComponent,
    canActivate: [authGuard],
  },
  {
    path: 'Sprint/:id',
    runGuardsAndResolvers: 'always',
    component: SprintComponent,
    canActivate: [authGuard],
  },
  {
    path: 'Sprint/:id/Board/:sprintId',
    runGuardsAndResolvers: 'always',
    component: SprintBoardComponent,
    canActivate: [authGuard],
  },
];
