import { Routes } from '@angular/router';
import { RegisterComponent } from './features/auth/register/register.component';
import { LoginComponent } from './features/auth/login/login.component';
import { HomeComponent } from './features/home/home.component';
import { BoardComponent } from './features/board/board.component';
import { authGuard } from './Core/Guards/auth.guard';
import { checkLoginGuard } from './Core/Guards/check-login.guard';
import { ProjectComponent } from './features/project/project.component';

export const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [checkLoginGuard] },
  { path: 'Login', component: LoginComponent, canActivate: [checkLoginGuard] },
  {
    path: 'Register',
    component: RegisterComponent,
    canActivate: [checkLoginGuard],
  },
  { path: 'Board', component: BoardComponent, canActivate: [authGuard] },
  { path: 'Project', component: ProjectComponent, canActivate: [authGuard] },
];
