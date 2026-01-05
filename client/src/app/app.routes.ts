import { Routes } from '@angular/router';
import { Home } from '../Features/home/home';
import { MemberDetailed } from '../Features/members/member-detailed/member-detailed';
import { Lists } from '../Features/lists/lists';
import { Messages } from '../Features/messages/messages';

export const routes: Routes = [

    {path:'',component: Home},
    {path:'members',component:MemberDetailed},
    {path:'members/:id',component:MemberDetailed},
    {path:'lists',component:Lists},
    {path:'messages',component:Messages},
    {path:'**',component:Home} , // wildcard root
];
