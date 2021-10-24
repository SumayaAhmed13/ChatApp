import { ToastrService } from 'ngx-toastr';
import { MemberService } from 'src/app/_services/member.service';
import { AccountService } from './../../_services/account.service';
import { User } from './../../_models/user';
import { Member } from './../../_models/member';
import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { take } from 'rxjs/operators';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editform')editform:NgForm;
  member:Member;
  user:User;
  @HostListener('window:beforeunload',['$event'])unloadNotification($event:any){
    if(this.editform.dirty){
      $event.returnValue=true;
    }
  }
  constructor(private accountServices:AccountService,private memberService:MemberService,private toastr:ToastrService) {
  this.accountServices.currentUser$.pipe(take(1)).subscribe(user=>this.user=user);

   }

  ngOnInit(): void {
    this.loadMember();
  }
  loadMember(){
    this.memberService.getMember(this.user.username).subscribe(member=>{
      this.member=member;
    })
  }
  updateMember(){
   this.memberService.updateMember(this.member).subscribe(()=>{
    this.toastr.success('Profile Updated Successfully');
    this.editform.reset(this.member);
   })

  }

}
