import { Component, inject } from '@angular/core';
import { MemberService } from '../../../core/services/member-service';
import { Observable } from 'rxjs';
import { Member } from '../../../../types/Members';
import { AsyncPipe } from '@angular/common';
import { MemberCard } from "../member-card/member-card";

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [AsyncPipe, MemberCard],
  templateUrl: './member-list.html',
  styleUrl: './member-list.css',
})
export class MemberList {
  
  //Asyn pipe for call api's
   private memberService = inject (MemberService);
   protected members$ :Observable<Member[]>;
   constructor(){
    this.members$ = this.memberService.getMembers();
   }
}
