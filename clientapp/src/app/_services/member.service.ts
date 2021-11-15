import { PaginatedResult } from './../_models/pagination';
import { Member } from './../_models/member';
import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams} from '@angular/common/http';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';



@Injectable({
  providedIn: 'root'
})
export class MemberService {
  baseUrl=environment.apiUrl;
  members:Member[]=[];
  paginatedResult:PaginatedResult<Member[]>=new PaginatedResult<Member[]>();
  constructor(private http:HttpClient) { }

  getMembers(page?:number,itemPerPage?:number){
    let params= new HttpParams();
    if(page !==null && itemPerPage !==null)
    {
      params= params.append('PageNumber', page.toString());
       params = params.append('PageSize', itemPerPage.toString());
     
    }
   
    return this.http.get<Member[]>(this.baseUrl +'user',{observe:"response", params}).pipe(map(response=>{
        this.paginatedResult.result=response.body;
        if(response.headers.get("Pagination")!==null){
          this.paginatedResult.pagination=JSON.parse(response.headers.get("Pagination"));
        }
  
       return this.paginatedResult;
    }))
  }

  getMember(username:string){
    const member=this.members.find(x=>x.userName===username);
    if(member!==undefined)return of(member);
    return this.http.get<Member>(this.baseUrl+'user/'+ username)
  }
  updateMember(member:Member){
   return this.http.put(this.baseUrl+'user',member).pipe(map(()=>{
     const index=this.members.indexOf(member);
     this.members[index]=member;
   }))
  }
  setMainPhoto(photoId:number){
    return this.http.put(this.baseUrl+ 'user/set-main-photo/'+ photoId,{})

  }

  deletePhoto(photoId:number){
    return this.http.delete(this.baseUrl +'user/delete-photo/' + photoId)
  }


}
