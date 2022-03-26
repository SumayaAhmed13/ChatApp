import { Pagination } from './../_models/pagination';
import { Message } from './../_models/message';
import { Component, OnInit } from '@angular/core';
import { MessageService } from '../_services/message.service';
import { TabDirective } from 'ngx-bootstrap/tabs';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {

  messages:Message[]=[];
  pagination:Pagination;
  pageNumber=1;
  pageSize=5;
  container='Unread';
  loading=false;
  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
    this.loadMessage();
  }
  
  loadMessage(){
    this.loading=true;
    this.messageService.getMessages(this.pageNumber,this.pageSize,this.container).subscribe(res=>{
      this.messages=res.result;
      this.pagination=res.pagination;
      this.loading=false;
    })
  }
  deleteMessage(id:number){
    this.messageService.deleteMessage(id).subscribe(()=>{
      this.messages.splice(this.messages.findIndex(m=>m.id === id),1)
    })

  }
  pageChanged(event:any){
    if(this.pageNumber !==event.page){
      this.pageNumber=event.page;
      this.loadMessage();
    }
  }
 

}
