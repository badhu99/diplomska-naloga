import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthUserResponse } from 'src/app/interfaces/auth-user-response';
import { UsersService } from 'src/app/services/api/users.service';
import { StateStorageService } from 'src/app/services/common/state-storage.service';

@Component({
  selector: 'app-users-overview',
  templateUrl: './users-overview.component.html',
  styleUrls: ['./users-overview.component.scss']
})
export class UsersOverviewComponent {


  userLoggedIn = false;
  isUserAdmin = false;
  userId:string | undefined = undefined;

  users!: AuthUserResponse[];


  constructor(
    private router: Router,
    protected serviceStateStorage : StateStorageService,
    private userService: UsersService
  ) {}
  ngOnInit(): void {

    this.serviceStateStorage.checkIfJwtSaved()

    this.serviceStateStorage.getAuthData.subscribe((value) => {
      this.userLoggedIn = value
    })

    this.serviceStateStorage.isUserAdmin.subscribe(value => {
      this.isUserAdmin = value
    })

    this.serviceStateStorage.getUserId.subscribe(value => {
      this.userId = value
    })

    this.getUserData()

  }

  logout() {
    this.serviceStateStorage.removeJwt();
  }

  navigateToLogin(){
    this.router.navigate(['../auth'])
  }

  navigateViewUsers(){
    this.router.navigate(["../users"]);
  }

  toggleActivate (user: AuthUserResponse, activate:boolean){
    this.userService.activateUser(user.id, activate).subscribe(_ => {
      this.getUserData()
    })
  }

  private getUserData(){
    this.userService.getSensorGroupsPagination().subscribe(val => {
      this.users = val;
    })
  }
}
