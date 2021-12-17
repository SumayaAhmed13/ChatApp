import { Pagination, PaginatedResult } from './../_models/pagination';
import { MemberService } from './../_services/member.service';
import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {

  members:Partial<Member[]>;
  predicate = 'liked';
  pageNumber=1;
  pageSize=2;
  pagination:Pagination;
  constructor(private memberService:MemberService) { }

  ngOnInit(): void {
    this.loadLikes();
  }
  loadLikes(){
    this.memberService.getLikes(this.predicate,this.pageNumber,this.pageSize).subscribe(reponse=>{
      console.log(this.predicate,this.pageNumber,this.pageSize);
      this.members=reponse.result;
      this.pagination=reponse.pagination;
    })
  }
  pageChanged(event:any){
    this.pageNumber = event.page;
    this.loadLikes();
  }

}
