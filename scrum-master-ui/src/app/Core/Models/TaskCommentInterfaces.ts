export interface Comment {
  id: string;
  content: string;
  senderId: string;
  fromSender: boolean;
}
export interface CreateCommentCommand {
  taskId: string;
  content: string;
}
