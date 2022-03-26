import { NgForm } from '@angular/forms';
import { Message } from './../../_models/message';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-member-message',
  templateUrl: './member-message.component.html',
  styleUrls: ['./member-message.component.css']
})
export class MemberMessageComponent implements OnInit {
 @ViewChild ('messageForm')messageForm:NgForm;
 @Input()messages:Message[];
 @Input() userName:string
  messageContent:string;
  constructor(private messageService:MessageService) { }

  ngOnInit(): void {
  

  }
  sendMessages(){
    this.messageService.sendMessage(this.userName, this.messageContent).subscribe(mess=>{
    this.messages.push(mess);
    this.messageForm.reset();
    })
  }
 

}
