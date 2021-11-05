import { Router } from '@angular/router';
import { AccountService } from './../_services/account.service';
import { Component, Input, OnInit, Output,EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, NgForm, ValidatorFn, Validators } from '@angular/forms';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  //@Input() userFormHomeComponent:any
  @Output()cancelRegister:any=new EventEmitter();

  registerForm:FormGroup;
  maxDate:Date;
  validationErrors:string[]=[];


constructor(private account:AccountService,private toastr:ToastrService,private fb:FormBuilder,private router:Router) { }

  ngOnInit() {
    this.intitializeForm();
    this.maxDate=new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear()-18);
    
  }
  intitializeForm()
  {
    this.registerForm = this.fb.group({
     username:['',Validators.required],
     gender:['male'],
     knowAs:['',Validators.required],
     dateOfBrith:['',Validators.required],
     country:['',Validators.required],
     city:['',Validators.required],
     password:['',[Validators.required,Validators.minLength(4),Validators.maxLength(8)]],
     confirmPassword:['',[Validators.required,this.matchValues('password')]]

   })
   this.registerForm.controls.password.valueChanges.subscribe(()=>{
     this.registerForm.controls.confirmPassword.updateValueAndValidity()
    })

  }

  matchValues(matchTo:string):ValidatorFn{
    return (control:AbstractControl)=>{
      return control?.value===control?.parent?.controls[matchTo].value? null:{isMatching: true}
    }

   
  }
  register(){
    //console.log(this.registerForm.value);
    this.account.register(this.registerForm.value).subscribe(res=>{
     this.router.navigateByUrl('/members');

    },error=>{
      //this.toastr.error(error.error);
      this.validationErrors=error
    })
  }
  cancel(){
    this.cancelRegister.emit(false);
  }

}
