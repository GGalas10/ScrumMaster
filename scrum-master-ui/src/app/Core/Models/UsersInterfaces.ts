export interface RegisterCommand{
    firstName:string;
    lastName:string;
    userName:string;
    email:string;
    password:string;
    confirmPassword:string;
}
export interface LoginCommand{
    email:string;
    password:string;
}