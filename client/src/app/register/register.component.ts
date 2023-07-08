import { Component, EventEmitter, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  @Output() cancelRegister = new EventEmitter();
  model: any = {}

  constructor(private accountService: AccountService ){ }

  register(){
    this.accountService.register(this.model).subscribe({
      next: () => {
        this.cancel();
      }
    })
  }

  cancel(){
    this.cancelRegister.emit(false);
  }
}
