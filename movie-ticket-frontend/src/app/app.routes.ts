import { Routes } from '@angular/router';

import { Login } from './pages/auth/login/login';
import { Register } from './pages/auth/register/register';
import { Dashboard } from './pages/customer/dashboard/dashboard';
import { MovieDetails } from './pages/customer/movie-details/movie-details';
import { SeatSelection } from './pages/customer/seat-selection/seat-selection';

export const routes: Routes = [

  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },

  {
    path: 'login',
    component: Login
  },

  {
    path: 'register',
    component: Register
  },

  {
    path: 'customer/dashboard',
    component: Dashboard
  },

  {
    path: 'customer/movies/:id',
    component: MovieDetails
  },
  {
  path: 'customer/seats/:showId',
  component: SeatSelection
},

  {
    path: '**',
    redirectTo: 'login'
  }

];