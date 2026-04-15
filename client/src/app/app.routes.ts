import { Routes } from '@angular/router';
import { Home } from '../Features/home/home';
import { MemberDetailed } from '../Features/members/member-detailed/member-detailed';
import { Lists } from '../Features/lists/lists';
import { Messages } from '../Features/messages/messages';
import { MemberList } from '../Features/members/member-list/member-list';
import { authGuard } from '../core/guards/auth-guard';
import { TestErrors } from '../Features/test-errors/test-errors';
import { NotFound } from '../shared/errors/not-found/not-found';
import { ServerError } from '../shared/errors/server-error/server-error';
import { MemberPhotos } from '../Features/members/member-photos/member-photos';
import { MemberProfile } from '../Features/members/member-profile/member-profile';
import { MemberMessages } from '../Features/members/member-messages/member-messages';
import { memberResolver } from '../Features/members/member-resolver';

export const routes: Routes = [

  { path: '', component: Home },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      { path: 'members', component: MemberList, canActivate: [authGuard] },
      { path: 'members/:id', 
        resolve:{member:memberResolver},
        runGuardsAndResolvers:'always',
        component: MemberDetailed,
        children:[
          {path:'', redirectTo:'profile',pathMatch:'full'},
          {path:'profile',component:MemberProfile,title:"Profile"},
          {path:'photos',component:MemberPhotos,title:"Photos"},
          {path:'messages',component:MemberMessages,title:"Messages"},
        ] 
      },
      { path: 'lists', component: Lists },
      { path: 'messages', component: Messages },
    ]
  },
{path:'errors',component:TestErrors},
{path:'server-error',component:ServerError},
  { path: '**', component: NotFound }, // wildcard root
];