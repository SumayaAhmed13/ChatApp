import { User } from './../_models/user';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model:any={};

  constructor(public account:AccountService,private router:Router,private toastr:ToastrService) { }

  ngOnInit(){
    
  }
  Login(){
   console.log(this.model);
    this.account.login(this.model).subscribe(res=>{
      this.router.navigateByUrl('/members');
    },error=>{
     this.toastr.error(error.error);
    })
  }
  logout(){
    
    this.account.logout();
    this.router.navigateByUrl('/')
  }
  cancel(){
    
  }


}
