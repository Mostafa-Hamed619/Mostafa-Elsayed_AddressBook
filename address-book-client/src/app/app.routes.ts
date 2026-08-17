import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [

  {
    path: 'address-book/new',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/address-book/address-form/address-form.component')
        .then(m => m.AddressFormComponent)
  },

  {
    path: 'address-book',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/address-book/address-list/address-list.component')
        .then(m => m.AddressList)
  },

  {
    path: 'job-titles',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/job-title/job-title.component')
        .then(m => m.JobTitleComponent)
  },

  {
    path: 'departments',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/department/department.component')
        .then(m => m.DepartmentComponent)
  },

  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },

  {
    path: 'login',
    component: LoginComponent
  },

  {
    path: 'register',
    component: RegisterComponent
  },

  // لازم يكون آخر route
  {
    path: '**',
    redirectTo: 'login'
  }
];