export class ErrorModel {
  private _title: string;
  private _messages: string[];
  show: boolean;
  constructor() {
    this._title = '';
    this._messages = [];
    this.show = false;
  }
  showMoreErrors(messages: string[], title: string) {
    this.show = true;
    this._title = title;
    this._messages = messages;
  }
  showOneBadRequest(message: string, title: string) {
    this._messages = [];
    this.show = true;
    this._title = title;
    this._messages.push(ErrorsNameSwitch(message));
  }
  showOneInternal() {
    this._messages = [];
    this.show = true;
    this._title = 'Errors.GeneralTitle';
    this._messages.push('Errors.SomethingWrong');
  }
  hide() {
    this.show = false;
  }
  public get title(): string {
    return this._title;
  }
  public get messages(): string[] {
    return this._messages;
  }
}
export function ErrorsNameSwitch(apiMessage: string) {
  return ERROR_MESSAGES[apiMessage] ?? 'Errors.SomethingWrong';
}
const ERROR_MESSAGES: Record<string, string> = {
  Wrong_Credentials: 'Errors.WrongCredentials',
  SomethingWrong: 'Errors.SomethingWrong',
  Command_Is_Null: 'Errors.SomethingWrong',
  Email_Cannot_Be_Null: 'Errors.EmaiRequired',
  Password_Cannot_Be_Null: 'Errors.PasswordRequired',
  FirstName_Cannot_Be_Null: 'Errors.FirstNameRequired',
  LastName_Cannot_Be_Null: 'Errors.LastNameRequired',
  UserName_Cannot_Be_Null: 'Errors.UserNameRequired',
  Passwords_Incorrect: 'Errors.IncorrectPasswords',
  User_Email_Already_Exist: 'Errors.EmailAlreadyExist',
  User_Name_Already_Exist: 'Errors.UserNameAlreadyExist',
  Password_Is_Too_Short: 'Errors.PasswordMinLength',
  PasswordRequiresDigit: 'Errors.PasswordRequiresDigit',
  PasswordRequiresUpper: 'Errors.PasswordRequiresUpper',
  PasswordRequiresNonAlphanumeric: 'Errors.NonAlphanumeric',
};
