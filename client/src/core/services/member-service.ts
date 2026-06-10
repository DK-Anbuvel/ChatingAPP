import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { tap } from 'rxjs/operators';
import {EditableMember, Member, Photo} from '../../../types/Members';
import { AccountService } from './account-service';
@Injectable({
  providedIn: 'root',
})
export class MemberService {
   private http =inject(HttpClient);
   private accountService = inject(AccountService);
   private baseUrl =environment.apiUrl;
   editMode =signal(false);
   member = signal<Member | null>(null);
   getMembers(){
        return this.http.get<Member[]>(this.baseUrl+'members');
    //return this.http.get<Member[]>(this.baseUrl+'members',this.getHttpOptions());
   }

   
  getMember(id: string) {
    // return this.http.get<Member>(this.baseUrl+'members/'+id);
    return this.http.get<Member>(this.baseUrl + 'members/' + id).pipe(
      tap((member: Member) => {
        this.member.set(member)
      })
    )
  }
  
  //  private getHttpOptions(){
  //   return{
  //     headers:new HttpHeaders({
  //       Authorization:'Bearer '+ this.accountService.currentUser()?.token
  //     })
  //   }
  //  }

  getMemberPhotos(id:string){
    return this.http.get<Photo[]>(this.baseUrl +'members/'+id+'/photos');
  }

    updateMember(member:EditableMember){
    return this.http.put(this.baseUrl+'members',member)
  }
  uploadPhoto(file:File){
    const formData = new FormData();
    formData.append('file',file);
    return this.http.post<Photo>(this.baseUrl+'members/add-photo',formData);
  }

  setMainPhoto(photo:Photo){
    return this.http.put(this.baseUrl+ 'members/set-main-photo/'+photo.id,{});
  }
  deletePhoto(photoId: number){
    return this.http.get(this.baseUrl+'members/delete-photo/'+photoId);
  }
}
