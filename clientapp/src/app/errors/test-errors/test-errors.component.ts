import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  templateUrl: './test-errors.component.html',
  styleUrls: ['./test-errors.component.css']
})
export class TestErrorsComponent implements OnInit {

  baseUrl="https://localhost:44335/api/";
  constructor(private http:HttpClient) { }
  validationErrors:string[]=[];
  ngOnInit(): void {
  }

  get404Error(){
    this.http.get(this.baseUrl + 'bug/not-found').subscribe(res=>{
      console.log(res)
    })
  }
  get400Error(){
    this.http.get(this.baseUrl + 'bug/bad-request').subscribe(res=>{
      console.log(res)
    })
  }
  get500Error(){
    this.http.get(this.baseUrl + 'bug/server-error').subscribe(res=>{
      console.log(res)
    })
  }
  get401Error(){
    this.http.get(this.baseUrl + 'bug/auth').subscribe(res=>{
      console.log(res)
    })
  }
  get400ValidationError(){
    this.http.post(this.baseUrl + 'Account/Register',{}).subscribe(res=>{
      console.log(res)
    },error=>{
      console.log(error);
      this.validationErrors=error;
      
      
    })
  }

}
