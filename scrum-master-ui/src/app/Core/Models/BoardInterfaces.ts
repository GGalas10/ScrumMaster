import { MemberDTO } from './UsersInterfaces';

export interface BoardDto {
  projectName: string;
  projectDescription: string;
  members: MemberDTO[];
}
