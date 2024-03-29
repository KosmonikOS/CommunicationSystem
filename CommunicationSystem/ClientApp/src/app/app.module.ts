import { NgModule } from '@angular/core';
import { environment } from '../environments/environment.prod';
import { AuthGuard } from "./guards/auth.guard"
import { AdminGuard } from "./guards/admin.guard"
import { TeacherGuard } from "./guards/teacher.guard"
import { AuthDataService } from "./auth/auth.data.service"
import { AccountDataService } from "./account/account.data.service"
import '@angular/common/locales/global/ru';
///////////////////////COMPONENTS///////////////////////////////////
import { AuthComponent } from "./auth/auth.component";
import { AppComponent } from './app.component';
import { RegistrationComponent } from './registration/registration.component';
import { ConfirmationComponent } from './confirmation/confirmation.component';
import { CreateVideochatComponent } from './createvideochat/createvideochat.component';
import { RecoveryComponent } from './recovery/recovery.component';
import { AccountComponent } from './account/account.component';
import { MessengerComponent } from './messenger/messenger.component'
import { VideochatComponent } from './videochat/videochat.component'
import { UsereditComponent } from "./useredit/useredit.component"
import { CreatetestsComponent } from './createtests/createtests.component'
import { SubjectsComponent } from './subjects/subjects.component'
import { TestsComponent } from "./tests/tests.component"
///////////////////////COMPONENTS///////////////////////////////////

//////////////////////MODULES///////////////////////////////////////
import { BrowserModule } from '@angular/platform-browser';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from "@angular/router";
import { JwtModule } from '@auth0/angular-jwt';
import { AuthModule } from "./auth/auth.module";
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RegistrationModule } from "./registration/registration.module";
import { ConfirmationModule } from "./confirmation/confirmation.module";
import { CreateVideochatModule } from "./createvideochat/createvideochat.module";
import { RecoveryModule } from './recovery/recovery.module';
import { AccountModule } from "./account/account.module";
import { MessengerModule } from './messenger/messenger.module';
import { VideochatModule } from './videochat/videochat.module';
import { UsereditModule } from "./useredit/useredit.module";
import { CreatetestsModule } from './createtests/createtests.module';
import { SubjectsModule } from './subjects/subjects.module'
import { TestsModule } from "./tests/tests.module"
//////////////////////MODULES///////////////////////////////////////


const routes = [
  { path: "", component: AuthComponent },
  { path: "registration", component: RegistrationComponent },
  { path: "recovery", component: RecoveryComponent },
  { path: "confirmation/:token", component: ConfirmationComponent },
  { path: "createvideochat", component: CreateVideochatComponent, canActivate: [AuthGuard] },
  { path: "account", component: AccountComponent, canActivate: [AuthGuard] },
  { path: "messenger", component: MessengerComponent, canActivate: [AuthGuard] },
  { path: "videochat/:roomId", component: VideochatComponent, canActivate: [AuthGuard] },
  { path: "users", component: UsereditComponent, canActivate: [AuthGuard, AdminGuard] },
  { path: "subjects", component: SubjectsComponent, canActivate: [AuthGuard, AdminGuard] },
  { path: "createtests", component: CreatetestsComponent, canActivate: [AuthGuard, TeacherGuard] },
  { path: "tests", component: TestsComponent, canActivate: [AuthGuard] },
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
    ConfirmationModule,
    CreateVideochatModule,
    RecoveryModule,
    AccountModule,
    SubjectsModule,
    MessengerModule,
    CreatetestsModule,
    TestsModule,
    UsereditModule,
    VideochatModule,
    HttpClientModule,
    RouterModule.forRoot(routes),
    JwtModule.forRoot({
      config: {
        tokenGetter,
        allowedDomains: environment.whiteListedHosts,
      }
    }),
  ],
  providers: [
    AuthGuard,
    AdminGuard,
    TeacherGuard,
    AuthDataService,
    AccountDataService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
