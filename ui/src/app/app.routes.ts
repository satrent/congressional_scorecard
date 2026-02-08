
import { Routes } from '@angular/router';
import { Home } from './home/home';
import { State } from './state/state';

export const routes: Routes = [
    { path: '', component: Home },
    { path: 'state/:id', component: State },
    { path: '**', redirectTo: '' }
];
