import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom, single } from 'rxjs';
import {Nav} from '../layout/nav/nav';
import {AccountService} from '../core/services/account-service';
import {Home} from '../Features/home/home';
import { User } from '../../types/user';
@Component({
  selector: 'app-root',
  imports: [Nav,Home],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private accountService = inject(AccountService);
  private http = inject(HttpClient); // Inject HttpClient service for modern days
  protected readonly title = 'Chatting App';
  protected members = signal<User[]>([]);//  used for state management ,  change detection

  //constructor(private http:HttpClient) {}
  // ngOnInit(): void {
  //   this.http.get('https://localhost:5001/api/members').subscribe({
  //     next:(Response)=>{ 
  //       console.log(Response);
  //       this.members.set(Response);
  //     //  this.title.set(Response);
  //     },error:(error)=>{
  //       console.log(error);
  //     },complete:()=>console.log('Request Completed')
  //   })
  // }
   async ngOnInit() {
     this.members.set(await this.getMembers());
    this.setCurrentUser();
   }
   setCurrentUser(){
    const userString = localStorage.getItem('user');
    if(!userString) return;
    const user = JSON.parse(userString);
    this.accountService.currentUser.set(user);
   }
  async getMembers(){
    try{
      return lastValueFrom(this.http.get<User[]>('https://localhost:5001/api/members'));
    }catch(error){
      console.log(error);
      throw error;
    }
  }
}
