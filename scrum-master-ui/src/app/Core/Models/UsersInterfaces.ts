export interface RegisterCommand {
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  password: string;
  confirmPassword: string;
}
export interface LoginCommand {
  email: string;
  password: string;
}
export interface MemberDTO {
  id: string;
  firstName: string;
  lastName: string;
  shortcut: string;
}

export interface UserDetails {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  registerAt: Date;
}
