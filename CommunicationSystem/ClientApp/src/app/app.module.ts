import { NgModule } from '@angular/core';
import { environment } from '../environments/environment.prod';
import { AuthGuard } from "./guards/auth.guard"
import { AuthDataService } from "./auth/auth.data.service"
///////////////////////COMPONENTS///////////////////////////////////
import { AuthComponent } from "./auth/auth.component";
import { AppComponent } from './app.component';
import { RegistrationComponent } from './registration/registration.component';
import { AccountComponent } from './account/account.component';
import { MessengerComponent } from './messenger/messenger.component'
///////////////////////COMPONENTS///////////////////////////////////

//////////////////////MODULES///////////////////////////////////////
import { BrowserModule } from '@angular/platform-browser';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from "@angular/router";
import { JwtModule } from '@auth0/angular-jwt';
import { AuthModule } from "./auth/auth.module";
import { HttpClientModule } from '@angular/common/http';
//import { ChartsModule } from 'ng2-charts';
import { RegistrationModule } from "./registration/registration.module";
import { AccountModule } from "./account/account.module";
import { MessengerModule } from './messenger/messenger.module'
//////////////////////MODULES///////////////////////////////////////


const routes = [
  { path: "", component: AuthComponent },
  { path: "registration", component: RegistrationComponent },
  { path: "account", component: AccountComponent, canActivate: [AuthGuard]},
  { path: "messenger", component: MessengerComponent, canActivate: [AuthGuard] },
]
export function tokenGetter() {
  return localStorage.getItem("COMMUNICATION_ACCESS_TOKEN_KEY");
}

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    NgbModule,
    AuthModule,
    RegistrationModule,
    AccountModule,
    MessengerModule,
    HttpClientModule,
    RouterModule.forRoot(routes),
    JwtModule.forRoot({
      config: {
        tokenGetter,
        allowedDomains: environment.whiteListedHosts
      }
    }),
    //ChartsModule
  ],
  providers: [
    AuthGuard,
    AuthDataService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
