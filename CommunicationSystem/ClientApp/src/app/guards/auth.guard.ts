import { Injectable } from "@angular/core";
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";
import { AuthDataService } from "../auth/auth.data.service"

@Injectable({
  providedIn: "root",
})
export class AuthGuard implements CanActivate {
  constructor(private authDataService: AuthDataService, private router: Router) { };
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (!this.authDataService.isAuthenticated()) {
      this.router.navigate([""]);
    }
    return true;
    }
}
