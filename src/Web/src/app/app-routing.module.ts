import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CheckEntryComponent } from './check-entry/check-entry.component';
import { AuthGuardService } from './services/auth-guard.service';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { EnterComponent } from './enter/enter.component';
import { ResourcesComponent } from './resources/resources.component';

const routes: Routes = [
  {
    path: '',
    component: EnterComponent,
    children: []
  },
  {
    path: 'checkEntry',
    component: CheckEntryComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'auth-callback',
    component: AuthCallbackComponent
  },
  {
    path: 'resources',
    component: ResourcesComponent
  },
  
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
