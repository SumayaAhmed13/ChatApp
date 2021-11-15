import { Pagination } from './../../_models/pagination';
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

  members:Member[];
  pagination:Pagination;
  pageNumber=1;
  pageSize=5;
  constructor(private memberService:MemberService) { }

  ngOnInit() {
    this.loadMember();
  }
  loadMember(){
    this.memberService.getMembers(this.pageNumber,this.pageSize).subscribe(response=>{
      this.members=response.result;
      this.pagination=response.pagination
    })

  }
  pageChanged(event:any){
    this.pageNumber=event.page;
    this.loadMember();

  }
 
}
