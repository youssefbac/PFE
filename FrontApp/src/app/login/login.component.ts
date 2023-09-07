import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import ValidateForm from '../helpers/validateform';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  loginForm !: FormGroup;
  constructor(private fb: FormBuilder,private auth:AuthService,private router:Router,private toast:NgToastService){
}

ngOnInit(): void {
 this.loginForm= this.fb.group({
  nomAgence: ['',Validators.required],
  password: ['',Validators.required]
 })
}

hideShowPass(){
      this.isText = !this.isText;
      this.isText ? this.eyeIcon = "fa-eye": this.eyeIcon = "fa-eye-slash";
      this.isText ? this.type = "text" : this.type = "password";
    }

onLogin(){
  if(this.loginForm.valid){
     console.log(this.loginForm.value);
     //send to database
     this.auth.login(this.loginForm.value)
     .subscribe({
      next:(res)=>{
        this.toast.success({detail:"SUCCESS",summary:res.message,duration: 5000});
        this.loginForm.reset();
        this.router.navigate(['dashboard']);
      },
      error:(err)=>{
        
        this.toast.error({detail:"ERROR",summary:"ERROR",duration: 5000});
        console.log(err);
      }
     })
  }
  else{
    
     ValidateForm.validateAllFormFields(this.loginForm);
    alert("your form is invalid") 
  }

}

}
