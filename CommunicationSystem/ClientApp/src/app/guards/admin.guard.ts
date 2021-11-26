import { Injectable } from "@angular/core";
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";
import { AccountDataService } from "../account/account.data.service"

@Injectable({
  providedIn: "root",
})
export class AdminGuard implements CanActivate {
  constructor(private accountDataService: AccountDataService, private router: Router) { };
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.accountDataService.currentAccount.role != 3) {
      this.router.navigate(["/messenger"]);
    }
    return true;
  }
}
