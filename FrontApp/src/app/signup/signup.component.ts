import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import ValidateForm from '../helpers/validateform';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  signUpForm! : FormGroup;
constructor(private fb: FormBuilder,private auth:AuthService,private router:Router,private toast :NgToastService){}

ngOnInit(): void {
    this.signUpForm=this.fb.group({
      nomAgence: ['',Validators.required],
      codeAgence:['',Validators.required],
      email:['',Validators.required],
      password:['',Validators.required],

    })
}

hideShowPass(){
  this.isText = !this.isText;
  this.isText ? this.eyeIcon = "fa-eye": this.eyeIcon = "fa-eye-slash";
  this.isText ? this.type = "text" : this.type = "password";
}

onSignup(){
  if(this.signUpForm.valid){
    //perform logic for signup
    this.auth.signUp(this.signUpForm.value)
    .subscribe({
      next:(res=>{
        this.toast.success({detail:"SUCCESS",summary:res.message,duration: 5000});
        this.signUpForm.reset();
        this.router.navigate(['login']);
      }),
      error:(err=>{
        this.toast.error({detail:"ERROR",summary:"ERROR",duration: 5000});
        console.log(err);
       
      })
    })
    console.log(this.signUpForm.value)
  }else{
    ValidateForm.validateAllFormFields(this.signUpForm)

  }

}






}
