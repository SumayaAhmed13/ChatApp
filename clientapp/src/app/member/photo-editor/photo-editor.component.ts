import { Photo } from './../../_models/photo';
import { MemberService } from 'src/app/_services/member.service';
import { AccountService } from './../../_services/account.service';
import { User } from './../../_models/user';
import { Member } from './../../_models/member';
import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {

@Input() member:Member
  uploader:FileUploader;
  hasBaseDropzoneOver=false;
  baseUrl=environment.apiUrl;
  user:User;
  constructor(private accountservice:AccountService,private memberService:MemberService) {
    this.accountservice.currentUser$.pipe(take(1)).subscribe(user=>{this.user=user})
   }

  ngOnInit(): void {
    this.initializeUploader();
  }

 
  fileOverBase(e:any){
    this.hasBaseDropzoneOver=e;
  }
  initializeUploader()
  {

   this.uploader=new FileUploader({
     url:this.baseUrl+'user/add-photo',
     authToken:'Bearer '+ this.user.token,
     isHTML5:true,
     allowedFileType:['image'],
     removeAfterUpload:true,
     autoUpload:false,
     maxFileSize:10 * 1024 * 1024

   });
   this.uploader.onAfterAddingFile=(file)=>{
     file.withCredentials=false;
   }
    this.uploader.onSuccessItem=(item,respone,status,headers)=>{
      if(respone){
        const photo=JSON.parse(respone);
        this.member.photos.push(photo);
        
      }
    }

  }
  setMainPhoto(photo:Photo){
   this.memberService.setMainPhoto(photo.id).subscribe(()=>{
     this.user.photoUrl=photo.url,
     this.accountservice.setCurrentUser(this.user),
     this.member.photoURL=photo.url;
     this.member.photos.forEach(p=>{
       if(p.isMain) p.isMain=false;
       if(p.id===photo.id) p.isMain=true;
     })
   })
  }


  deletePhoto(photoId: number) {
    this.memberService.deletePhoto(photoId).subscribe(() => {
      this.member.photos = this.member.photos.filter(x => x.id !== photoId);
    })
  }

}
