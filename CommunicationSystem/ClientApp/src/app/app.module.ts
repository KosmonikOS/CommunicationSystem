import { NgModule } from '@angular/core';
import { environment } from '../environments/environment.prod';
///////////////////////COMPONENTS///////////////////////////////////
import { AuthComponent } from "./auth/auth.component";
import { AppComponent } from './app.component';
import { RegistrationComponent } from './registration/registration.component';
///////////////////////COMPONENTS///////////////////////////////////

//////////////////////MODULES///////////////////////////////////////
import { BrowserModule } from '@angular/platform-browser';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from "@angular/router";
import { JwtModule } from '@auth0/angular-jwt';
import { AuthModule } from "./auth/auth.module";
import { HttpClientModule } from '@angular/common/http';
import { RegistrationModule } from "./registration/registration.module";
//////////////////////MODULES///////////////////////////////////////


const routes = [
  { path: "", component: AuthComponent },
  { path: "registration", component: RegistrationComponent }
]
export function tokenGetter() {
  return localStorage.getItem("ACCESS_TOKEN_KEY")
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
    HttpClientModule,
    RouterModule.forRoot(routes),
    JwtModule.forRoot({
      config: {
        tokenGetter,
        allowedDomains: environment.whiteListedHosts
      }
      })
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
