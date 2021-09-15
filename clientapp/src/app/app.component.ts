import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'clientapp';
  users:any;
  constructor(private http:HttpClient){}
  ngOnInit() {
    this.getUser();
    }

    getUser(){
      this.http.get("https://localhost:44335/api/User").subscribe(res=>{
        this.users=res;
      },
      err=>{
        console.log(err)
      }
      
      )
    }
}
