import { Component, inject, input, OnInit, output, signal } from '@angular/core';
import {AbstractControl, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {RegisterCreds, User} from '../../../../types/user'
import { AccountService } from '../../../core/services/account-service';
import { JsonPipe } from '@angular/common';
import { TextInput } from "../../../shared/text-input/text-input";

@Component({
  selector: 'app-register',
  //imports: [FormsModule],
  imports: [ReactiveFormsModule, JsonPipe, TextInput],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register{// implements OnInit {

  private accountService = inject(AccountService);
  //  membersFromHome = input.required<User[]>();
  private fg = inject(FormBuilder);
   cancelRegister = output<boolean>();
  protected creds = {} as RegisterCreds;
  //protected registerForm : FormGroup = new FormGroup({});
  //protected registerForm:FormGroup;
  protected credentialForm: FormGroup;
  protected profileForm: FormGroup;
  protected currentStep = signal(1);
  // ngOnInit(): void {
  //  this.initializeForm();
  // }
  constructor(){
        this.credentialForm = this.fg.group({
      email : ['',[Validators.required,Validators.email]],
      displayName:['',[Validators.required]],
      password: ['',[Validators.required,Validators.minLength(4),Validators.maxLength(10)]],
      confirmPassword: ['',[Validators.required,this.matchValues('password')]],
    });

    this.profileForm = this.fg.group({
        gender:['',Validators.required],
        dateOfBirth:['',Validators.required],
        city:['',Validators.required],
        country:['',Validators.required],
    })
    
    this.credentialForm.controls['password'].valueChanges.subscribe(() =>{
      this.credentialForm.controls['confirmPassword'].updateValueAndValidity();
    })
  }

  // initializeForm(){
  //   this.registerForm = new FormGroup({
  //     email : new FormControl('',[Validators.required,Validators.email]),
  //     displayName: new FormControl('',[Validators.required]),
  //     password: new FormControl('',[Validators.required,Validators.minLength(4),Validators.maxLength(10)]),
  //     confirmPassword: new FormControl('',[Validators.required,this.matchValues('password')]),
  //   });
    
  //   this.registerForm.controls['password'].valueChanges.subscribe(() =>{
  //     this.registerForm.controls['confirmPassword'].updateValueAndValidity();
  //   })
  // }
  matchValues(matchTo: string): ValidatorFn{
    return(control:AbstractControl): ValidationErrors | null =>{
      const parent =control.parent;
      if(!parent) return null;

      const matchValue = parent.get(matchTo)?.value;
      return control.value ===matchValue?null:{passwordMismach:true}
    }
  }
  nextStep(){
    if(this.credentialForm.valid){
      this.currentStep.update(prevStep => prevStep -1);
    }
  }
  prevStep(){
    this.currentStep.update(prevStep => prevStep -1);
  }
  register(){
    if(this.profileForm.valid && this.credentialForm.valid){
      const formData = {...this.credentialForm.value, ...this.profileForm.value};
      console.log('Form data:',formData);
    }
    console.log(this.creds);
    this.accountService.register(this.creds).subscribe({
      next: response =>{
        console.log(response);
        this.cancel();
      },
      error:error => console.log(error)
    })

    console.log(this.credentialForm.value);
  }
  cancel(){
 this.cancelRegister.emit(false);
  }
}
