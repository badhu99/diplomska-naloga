import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/auth/authentication.service';
import { StateStorageService } from 'src/app/services/common/state-storage.service';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss']
})
export class SigninComponent {
  singInForm!: FormGroup;

  constructor(private fb: FormBuilder,
    private authService: AuthenticationService,
    private stateService: StateStorageService,
    private router: Router){}

  ngOnInit() {
    this.singInForm = this.fb.group({
      username : ['', [Validators.required, Validators.minLength(4)]],
      password : ['', [Validators.required, Validators.minLength(4)]],
    })
  }

  onSubmit(form: FormGroup) {
    if(form.valid){
      this.authService.SignIn(form.value["username"], form.value["password"]).subscribe(data => {
        this.stateService.saveJwt(data.accessToken);
        this.router.navigate(["sensors"]);
      },err => console.error(err));      
    }
    
  }
}
