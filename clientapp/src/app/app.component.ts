import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'clientapp';
  user:any;
  
  constructor(private http:HttpClient){}

  ngOnInit() {
  this.getUser();
  }
  getUser(){
    this.http.get("").subscribe(res=>{
      this.user=res;
    },
    err=>{
      console.log(err)
    }
    
    )
  }

}
