import { Member } from './../../_models/member';
import { Component, OnInit } from '@angular/core';
import { MemberService } from 'src/app/_services/member.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  member$:Observable<Member[]>
  constructor(private memberService:MemberService) { }

  ngOnInit(): void {
    this.member$=this.memberService.getMembers()
  }
 
}
