import { Component } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/auth/authentication.service';
import { StateStorageService } from 'src/app/services/common/state-storage.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent {
  errorMessage ="";
  singUpForm!: FormGroup;
  constructor(private formBuilder: FormBuilder,
    private authService:AuthenticationService,
    private stateService:StateStorageService,
    private router:Router) {}
  ngOnInit() {
    this.singUpForm =this.formBuilder.group({
      firstname: (['', Validators.required]),
      lastname: (['', Validators.required]),
      email: (['', [Validators.required, Validators.email]]),
      username: (['', Validators.required]),
      passwords: this.formBuilder.group({
        password: ['', [Validators.required, Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).+$')]],
        confirmPassword: ['', Validators.required]
      }, { validator: this.passwordMatchValidator })
    });
  }

  onSubmit(form: FormGroup) {
    if(form.valid){
      this.signUp(form);
      this.errorMessage = "";
    }
    else{
      if(form.controls['firstname'].errors) this.errorMessage = "First name is required!"
      else if(form.controls['lastname'].errors) this.errorMessage = "Last name is required!"
      else if(form.controls['email'].errors) this.errorMessage = "Email is required!"
      else if(form.controls['username'].errors) this.errorMessage = "Username is required!"
      else if((form.controls['passwords'] as FormGroup).controls['password'].errors) this.errorMessage = "Password must contain capital letter and number!"
      else if((form.controls['passwords'] as FormGroup).controls['confirmPassword'].errors) this.errorMessage = "Password must match!"
    }
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    if (password !== confirmPassword) {
      form.get('confirmPassword')?.setErrors({ passwordMismatch: true });
    } else {
      form.get('confirmPassword')?.setErrors(null);
    }
  }

  private signUp(form:FormGroup){
    this.authService.SignUp(
      form.value.firstname,
      form.value.lastname,
      form.value.email,
      form.value.username,
      form.value.passwords.password,
    ).subscribe(data => {
      this.stateService.saveJwt(data.accessToken);
      this.router.navigate(["sensors"]);
    })
  }

}
