import { Component } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.scss']
})
export class AuthenticationComponent {
  currentUrl = ""

  constructor(private router: Router){}

  ngOnInit(){
    this.checkUrlEndPath(this.router.url)

    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.checkUrlEndPath(event.url)
      }
    })
  
  }
  navigateTo(path:string){
    this.router.navigate(["auth", path]);
  }

  private checkUrlEndPath(url:string){
    if(url.endsWith('signup')){
      this.currentUrl = 'signup'
    }
    else if(url.endsWith('signin')){
      this.currentUrl = 'signin'
    }
  }
}
