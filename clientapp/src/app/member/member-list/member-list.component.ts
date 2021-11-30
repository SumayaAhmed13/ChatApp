import { AccountService } from './../../_services/account.service';
import { UserParams } from './../../_models/userParams';
import { Pagination } from './../../_models/pagination';
import { Member } from './../../_models/member';
import { Component, OnInit } from '@angular/core';
import { MemberService } from 'src/app/_services/member.service';
import { User } from 'src/app/_models/user';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  members:Member[];
  pagination:Pagination;
  userparams:UserParams;
  user:User;
  genderList=[{value:'male',display:'Males'},{value:'female',display:'Females'}]
  constructor(private memberService:MemberService) {
  this.userparams=memberService.getUserParam();
    
   }

  ngOnInit() {
    this.loadMember();
  }
  loadMember(){
    this.memberService.setUserParam(this.userparams);
    this.memberService.getMembers(this.userparams).subscribe(response=>{
      this.members=response.result;
      this.pagination=response.pagination
    })

  }
  resetFilters(){
    this.userparams=this.memberService.resetUserParam();
    this.loadMember();
  }
  pageChanged(event:any){
    this.userparams.pageNumber=event.page;
    this.memberService.setUserParam(this.userparams);
    this.loadMember();

  }
  // pageChanged(event: any) {
  //   if (this.pageNumber !== event.page) {
  //      this.pageNumber = event.page;
  //      this.loadMessages()
  //    }
  //  }
 
}
