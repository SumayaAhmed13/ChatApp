import { ToastrService } from 'ngx-toastr';
import { AccountService } from './../_services/account.service';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private AccountService:AccountService,private toaster:ToastrService){}
  canActivate(): Observable<boolean>{
    return this.AccountService.currentUser$.pipe(map(user=>{
     if(user)return true;
     this.toaster.error('Your are not allow!');
     return false;
  }))
    
  }
  
}
