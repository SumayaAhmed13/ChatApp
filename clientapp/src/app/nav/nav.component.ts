import { User } from './../_models/user';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model:any={};

  constructor(public account:AccountService) { }

  ngOnInit(){
    
  }
  Login(){
   console.log(this.model);
    this.account.login(this.model).subscribe(res=>{
      
    })
  }
  logout(){
    
    this.account.logout();
  }
  cancel(){
    
  }


}
