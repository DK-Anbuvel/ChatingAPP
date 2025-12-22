import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom, single } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private http = inject(HttpClient); // Inject HttpClient service for modern days
  protected readonly title = 'Chatting App';
  protected members = signal<any>([]);//  used for state management ,  change detection

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
   }
  async getMembers(){
    try{
      return lastValueFrom(this.http.get('https://localhost:5001/api/members'));
    }catch(error){
      console.log(error);
      throw error;
    }
  }
}
