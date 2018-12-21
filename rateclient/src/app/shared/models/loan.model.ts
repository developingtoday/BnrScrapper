export interface Loan {
  id: string;
  email: string;
  bankRate: number;
  bankMargin: number;
  sendEmail: boolean;
  ammount: number;
  rateDateOfPayment: Date;
  months: number;
  name:string;
}
