import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { EmpleadoListComponent } from './components/empleado-list/empleado-list.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'empleados', component: EmpleadoListComponent, canActivate: [AuthGuard] },
  { path: '**',   redirectTo: '/empleados', pathMatch: 'full' }, // redirect to
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
