import { AccountService } from './../_services/account.service';
import { Component, Input, OnInit, Output,EventEmitter } from '@angular/core';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  //@Input() userFormHomeComponent:any
  @Output()cancelRegister:any=new EventEmitter();
 model:any={}

constructor(private account:AccountService) { }

  ngOnInit() {
    
  }
  register(){
    this.account.register(this.model).subscribe(res=>{
      console.log(res);
      this.cancel();

    },error=>{
      console.log('Error');
    })
  }
  cancel(){
    this.cancelRegister.emit(false);
  }

}
