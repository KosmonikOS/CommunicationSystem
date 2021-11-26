import { Injectable } from "@angular/core";
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";
import { AccountDataService } from "../account/account.data.service"

@Injectable({
  providedIn: "root",
})
export class TeacherGuard implements CanActivate {
  constructor(private accountDataService: AccountDataService, private router: Router) { };
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.accountDataService.currentAccount.role == 1) {
      this.router.navigate(["/messenger"]);
    }
    return true;
  }

}
