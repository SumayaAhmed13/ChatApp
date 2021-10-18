import { MemberService } from 'src/app/_services/member.service';
import { Member } from './../../_models/member';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  member:Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  constructor(private memberService:MemberService,private route:ActivatedRoute) { }

  ngOnInit() {
    this.loadMember();
    this.galleryOptions=[{
      width:'500px',
      height:'500px',
      imagePercent:100,
      thumbnailsColumns:4,
      imageAnimation:NgxGalleryAnimation.Slide,
      preview:false
    }]
 
  }
 getImages():NgxGalleryImage[]{
   const imageUrl=[];
   for(const photo of this.member.photos){
    imageUrl.push({
      small:photo?.url,
      medium:photo?.url,
      big:photo?.url
    })
  
   }
   return imageUrl;
 }
  loadMember(){
    this.memberService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(res=>{
      this.member=res;
      this.galleryImages= this.getImages()
    })
  }

}
