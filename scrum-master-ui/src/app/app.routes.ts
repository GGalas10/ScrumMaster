import { Routes } from '@angular/router';
import { RegisterComponent } from './features/auth/register/register.component';
import { LoginComponent } from './features/auth/login/login.component';
import { HomeComponent } from './features/home/home.component';

export const routes: Routes = 
    [
        {path: '', component: HomeComponent},
        {path: 'Login', component: LoginComponent},
        {path: 'Register', component: RegisterComponent}
    ];