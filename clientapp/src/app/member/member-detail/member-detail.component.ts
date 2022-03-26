import { MemberService } from 'src/app/_services/member.service';
import { Member } from './../../_models/member';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_models/message';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;
  member:Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  activeTab:TabDirective;
  messages:Message[]=[];
  constructor(private route:ActivatedRoute,private messageService: MessageService) { }

  ngOnInit():void {
    this.route.data.subscribe(data=>{
      this.member=data.member;
    })
    //this.loadMember();
    // this.route.queryParams.subscribe(params=>{
    //   params.tab?this.selectTab(params.tab):this.selectTab(0);
    // })

    this.route.queryParams.subscribe(params => {
      params.tab ? this.selectTab(params.tab) : this.selectTab(0);
    })
    this.galleryOptions=[{
      width:'500px',
      height:'500px',
      imagePercent:100,
      thumbnailsColumns:4,
      imageAnimation:NgxGalleryAnimation.Slide,
      preview:false
    }]
    this.galleryImages= this.getImages()
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
  
  onTabActivited(data:TabDirective){
    this.activeTab=data;
    if(this.activeTab.heading==='Massages' && this.messages.length===0)
    {
      this.loadMessage();
    }
  }
   selectTab(tabId: number){  
   
     this.memberTabs.tabs[tabId].active = true;
   }
  

    loadMessage(){
  
      this.messageService.getMessageThread(this.member.userName).subscribe(mess=>this.messages=mess)
    }

}
