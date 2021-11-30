import { PaginatedResult } from './../_models/pagination';
import { Member } from './../_models/member';
import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams} from '@angular/common/http';
import { of } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { UserParams } from '../_models/userParams';
import { User } from '../_models/user';
import { AccountService } from './account.service';



@Injectable({
  providedIn: 'root'
})
export class MemberService {
  baseUrl=environment.apiUrl;
  members:Member[]=[];
  memberCache =new Map();
  userparams:UserParams;
  user:User;
  constructor(private http:HttpClient,private accountService:AccountService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user=>{
      this.user = user;
      this.userparams = new UserParams(user);
      })
  }
  getUserParam(){
    return this.userparams
  }
  setUserParam(param:UserParams){
    return this.userparams=param;
  }
  resetUserParam(){
    this.userparams=new UserParams(this.user);
    return  this.userparams;
  }

  getMembers(userParams:UserParams){
    var response=this.memberCache.get(Object.values(userParams).join('-'));
    if(response){
      return of(response)
    }
   
    let params =this.getPaginationHeader(userParams.pageNumber,userParams.pageSize);

    params=params.append('minAge',userParams.minAge.toString());
    params=params.append('maxAge',userParams.maxAge.toString());
    params=params.append('gender',userParams.gender);
    params=params.append('orderBy',userParams.orderBy);

    return this.getPaginatedResult<Member[]>(this.baseUrl + 'user',params).pipe(map(response=>{
      this.memberCache.set(Object.values(userParams).join('-'),response)
      return response;
    }))
  }


  getMember(username:string){
   console.log(this.memberCache);
   const member=[...this.memberCache.values()]
   .reduce((arr,ele)=>arr.concat(ele.result),[])
   .find((member:Member)=>member.userName===username)
  
   if(member){
     return of(member)
   }
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
  private getPaginatedResult<T>(url,params) {
    const  paginatedResult:PaginatedResult<T>=new PaginatedResult<T>();
     return this.http.get<T>(url, { observe: "response", params }).pipe(map(response => {
       paginatedResult.result = response.body;
       if (response.headers.get("Pagination") !== null) {
         paginatedResult.pagination = JSON.parse(response.headers.get("Pagination"));
       }
 
       return paginatedResult;
     }));
   }
 
   private getPaginationHeader(pageNumber:number, pageSize:number){
       let params= new HttpParams();
       params= params.append('pageNumber', pageNumber.toString());
       params = params.append('pageSize', pageSize.toString());
      
     return params;
   }

}
